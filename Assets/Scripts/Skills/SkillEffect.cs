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

    // Start is called before the first frame update
    public virtual void Start()
    {
        Transform field = transform.parent;

        if (gameObject.tag == "CharacterSkill") {
            attacker = field.Find("Character").gameObject;
            defender = field.Find("Enemy").gameObject;
        } else {
            attacker = field.Find("Enemy").gameObject;
            defender = field.Find("Character").gameObject;
        }

        attackerStatus = attacker.GetComponent<Status>();
        defenderStatus = defender.GetComponent<Status>();

        /*
        공식
        135 - 45 * direction = z
        direction = -(z - 135) / 45
        direction = (135 - z) / 45
        z는 90부터 시작해서 45씩 낮아짐
        */

        Debug.Log("부모 Start 실행" + attacker.tag + " " + defender.tag);
        direction = (int)((135 - transform.rotation.z) / 45);

        field.GetComponent<Field>().Add(this);
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

    public void OnTriggerStay2D(Collider2D col) {
        if (isAttacked)
            return;
        //시전자가 아닌 캐릭터에게 부딛칠때;
        if (defender == null)
            return;

        Debug.Log("부딛침");
        if (col.CompareTag(defender.tag) == true) {
            isAttacked = true;
            Debug.Log("부딛침");
            CalculateDamage();

            BattleAgent agent = attacker.GetComponent<BattleAgent>();

            if (agent != null)
                agent.AddReward(2.0f);
        }
    }

    public virtual void CalculateDamage() {

    }
}
