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
    private bool isAttacked = false;

    Field field;

    public float currentCastingTime;
    public float currentDuration;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //collider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public virtual IEnumerator Casting() {
        if (anim.GetCurrentAnimatorStateInfo(0).length > 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("Casting")) {
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) {
                currentCastingTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                Debug.Log("캐스팅 시간" + currentCastingTime);

                yield return null;
            }

            anim.SetTrigger("FinishCasting");
        }

        currentCastingTime = 1;
        StartCoroutine(Hitting());
    }

    public virtual IEnumerator Hitting() {
        yield return null;
    }

    public virtual void Initialize() {
        field = transform.GetComponentInParent<Field>();

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
        /*
        공식
        135 - 45 * direction = z
        direction = -(z - 135) / 45
        direction = (135 - z) / 45
        z는 90부터 시작해서 45씩 낮아짐
        */

        Debug.Log("부모 Start 실행" + attacker.tag + " " + defender.tag);
        direction = (int)((135 - transform.rotation.z) / 45);

        //거리로 해야됨 임시로 1


        //Invoke("DestroySkillEffect", activeSkill.Range / activeSkill.Speed);

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
        Bounds col = collider.bounds;

        Vector2 a = col.center - transform.localPosition;
        result += a;

        return result;
    }

    public Vector2 GetColliderRange() {
        Bounds col = collider.bounds;

        return col.size;
    }

    public Vector2 GetVelocity() {
        //                         x   ↑  ↗   →  ↘   ↓  ↙   ←   ↖
        float[] dx = new float[] { 0,  0,  1,  1,  1,  0, -1, -1, -1};
        float[] dy = new float[] { 0,  1,  1,  0, -1, -1, -1,  0,  1};

        float shoot = activeSkill.Range == 0 ? 0 : 1;

        return new Vector2(dx[direction], dy[direction]) * shoot;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void ReservationDestroy() {
        Invoke("DestroySkillEffect", 1);
    }

    public void DestroySkillEffect() {
        Debug.Log("Destroyed");

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
    }
}
