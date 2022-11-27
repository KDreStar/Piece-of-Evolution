using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer sr;
    public ActiveSkill activeSkill;

    [HideInInspector]
    public Character attacker;

    [HideInInspector]
    public Character defender;

    public BoxCollider2D collider;

    [HideInInspector]
    public Animator anim;

    public int direction;
    public bool isAttacked = false;

    Field field;
    BattleEnvController bc;

    public float currentCastingTime;
    public float currentDuration;

    protected static WaitForFixedUpdate wait = new WaitForFixedUpdate();

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //collider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    //준비
    public virtual IEnumerator Casting() {
        if (anim.GetCurrentAnimatorStateInfo(0).length > 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("Casting")) {
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) {
                currentCastingTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                Debug.Log("캐스팅 시간" + currentCastingTime);

                yield return wait;
            }

            anim.SetTrigger("FinishCasting");
        }

        currentCastingTime = 1;
        StartCoroutine(Active());
    }

    //활성
    public virtual IEnumerator Active() {
        yield return wait;
    }

    public virtual void Initialize(GameObject caster, int direction) {
        transform.parent = caster.transform.parent.transform;

        field = transform.GetComponentInParent<Field>();
        bc    = transform.GetComponentInParent<BattleEnvController>();

        gameObject.tag = caster.gameObject.tag + "Skill";

        if (gameObject.tag == "CharacterSkill") {
            attacker = field.transform.Find("Character").GetComponent<Character>();
            defender = field.transform.Find("Enemy").GetComponent<Character>();
        } else {
            attacker = field.transform.Find("Enemy").GetComponent<Character>();
            defender = field.transform.Find("Character").GetComponent<Character>();
        }

        isAttacked = false;

        if (collider != null)
            EnableCollider();

        
        this.direction = direction;

        Vector3 angle = new Vector3(0, 0, 135 - direction * 45);
  
        //스킬 관리 인스턴스에서 이펙트 빌림
        //그후 위치 세팅
        transform.position = caster.transform.position;
        transform.rotation = Quaternion.Euler(angle);

        field.Add(this);

        currentCastingTime = 0;
        currentDuration = 0;
        StartCoroutine(Casting());
    }

    public int GetSkillNo() {
        return activeSkill.No;
    }

    public string GetAttackerTag() {
        return attacker.tag;
    }

    public void EnableCollider() {
        if (collider != null)
            collider.enabled = true;
    }

    public void DisableCollider() {
        if (collider != null)
            collider.enabled = false;
    }

    //생성 좌표 (슬래시는 자기보다 앞쪽 기준이 좌표)
    //메테오는 콜라이더는 0이지만 생성좌표가 (4, 0)
    public Vector2 GetCreatePos() {
        Vector2 result = activeSkill.CreateOffset;
        Vector2 offset = collider.offset * (transform.lossyScale / 2);

        result += offset;

        return result;
    }

    //실제 콜라이더 좌표
    public Vector2 GetCurrentPos() {
        Vector2 fieldPos = field.transform.position;
        Vector2 skillPos = collider.bounds.center;

        return skillPos - fieldPos;
    }

    public Vector2 GetColliderRange() {
        //Size = 스프라이트 크기 / ppu
        //실제 크기 size * localScale

        Vector2 size = collider.size;

        Debug.Log(activeSkill + "Scale" + transform.lossyScale + " " + transform.localScale + " " + size + " " + size * transform.localScale);

        return size * transform.localScale;
    }

    public Vector2 GetVelocity() {
        //                         x   ↑  ↗   →  ↘   ↓  ↙   ←   ↖
        float[] dx = new float[] { 0,  0,  1,  1,  1,  0, -1, -1, -1};
        float[] dy = new float[] { 0,  1,  1,  0, -1, -1, -1,  0,  1};

        float shoot = activeSkill.Range == 0 ? 0 : 1;

        return new Vector2(dx[direction], dy[direction]) * shoot;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        
    }

    public virtual void ReservationDestroy() {
        Invoke("DestroySkillEffect", 1);
    }

    public void DestroySkillEffect() {
        Debug.Log("Destroyed");

        attacker.isStopping = false;
        field.Remove(this);
        Managers.Pool.ReturnSkillEffect(this);
    }

    //기본 작동 = 데미지 입히기
    public virtual void OnTriggerStay2D(Collider2D col) {
        if (isAttacked)
            return;
        
        //시전자가 아닌 캐릭터에게 부딛칠때;
        if (defender == null)
            return;

        if (col.CompareTag(defender.tag) == true) {
            isAttacked = true;
            TriggerEffect();

            /*
            BattleAgent attackerAgent = attacker.GetComponent<BattleAgent>();
            BattleAgent defenderAgent = defender.GetComponent<BattleAgent>();

            if (attackerAgent != null && defenderAgent != null) {
                attackerAgent.AddReward(0.2f);
                defenderAgent.AddReward(-0.2f);
            }
            */
        }
    }

    public virtual void TriggerEffect() {
		float atk = Managers.Battle.CalculateFormula(activeSkill.Effects[0].Formula, attacker.status, defender.status);
        float def = defender.status.CurrentDEF;
        float damage = atk - def;

        damage = damage <= 0 ? 1 : damage;

		defender.status.TakeDamage(damage);

        bc.AddHitReward(attacker, defender, damage);
    }
}
