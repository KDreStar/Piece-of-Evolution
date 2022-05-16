using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    public ActiveSkillData activeSkillData;

    public GameObject attacker;
    public GameObject defender;
    public Status attackerStatus;
    public Status defenderStatus;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "CharacterSkill") {
            attacker = GameObject.FindGameObjectWithTag("Character");
            defender = GameObject.FindGameObjectWithTag("Enemy");
        } else {
            attacker = GameObject.FindGameObjectWithTag("Enemy");
            defender = GameObject.FindGameObjectWithTag("Character");
        }

        attackerStatus = attacker.GetComponent<Status>();
        defenderStatus = defender.GetComponent<Status>();

        //Debug.Log(gameObject.tag + " " + activeSkillData.Name + " Created");
        Debug.Log("Destroy");
        //TO-DO 스킬 이펙트 데이터에서 DestroyTime 가져와서 제거하기
        Destroy(gameObject, 2.5f);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log(col.gameObject.tag);
        Debug.Log(defender.tag);
        //즉 스킬이 시전자가 아닌 캐릭터에게 부딛칠때;
        if (col.CompareTag(defender.tag) == true) {
            CalculateDamage();
        }
    }

    public virtual void CalculateDamage() {

    }
}
