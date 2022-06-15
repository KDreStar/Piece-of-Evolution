using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    public ActiveSkill activeSkill;

    public GameObject attacker;
    public GameObject defender;
    public Status attackerStatus;
    public Status defenderStatus;

    public int direction;
    private bool isAttacked = false;

    Field field;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Initialize() {
        field = transform.GetComponentInParent<Field>();

        if (gameObject.tag == "CharacterSkill") {
            attacker = field.transform.Find("Character").gameObject;
            defender = field.transform.Find("Enemy").gameObject;
        } else {
            attacker = field.transform.Find("Enemy").gameObject;
            defender = field.transform.Find("Character").gameObject;
        }

        attackerStatus = attacker.GetComponent<Status>();
        defenderStatus = defender.GetComponent<Status>();

        isAttacked = false;
        /*
        공식
        135 - 45 * direction = z
        direction = -(z - 135) / 45
        direction = (135 - z) / 45
        z는 90부터 시작해서 45씩 낮아짐
        */

        Debug.Log("부모 Start 실행" + attacker.tag + " " + defender.tag);
        direction = (int)((135 - transform.rotation.z) / 45);

        Invoke("DestroySkillEffect", activeSkill.BaseDestroyTime);

        field.Add(this);
    }

    public int GetSkillNo() {
        return activeSkill.No;
    }

    public string GetAttackerTag() {
        return attacker.tag;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public void DestroySkillEffect() {
        Debug.Log("Destroyed");

        field.Remove(this);
        SkillPool.Instance.ReturnSkillEffect(this);
    }

    public void OnTriggerStay2D(Collider2D col) {
        if (isAttacked)
            return;
        //시전자가 아닌 캐릭터에게 부딛칠때;
        if (defender == null)
            return;

        if (col.CompareTag(defender.tag) == true) {
            isAttacked = true;
            CalculateDamage();

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

    public virtual void CalculateDamage() {
		float atk = attackerStatus.Calculate(activeSkill.DamageFormula);
        float def = defenderStatus.CurrentDEF;
        float damage = atk - def;

        damage = damage <= 0 ? 1 : damage;

		defenderStatus.TakeDamage(damage);
    }
}
