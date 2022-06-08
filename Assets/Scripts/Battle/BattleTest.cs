using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//임시 스킬 부여
public class BattleTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        Debug.Log("캐릭터 " + characters.Length);
        Debug.Log("적 " + enemys.Length);

        for (int i=0; i<characters.Length; i++) {
            Debug.Log("생성" + i);
            EquipSkills equipSkills = characters[i].GetComponent<EquipSkills>();

            equipSkills.AddSkill(0, SkillDatabase.Instance.GetSkill(13));
            equipSkills.AddSkill(1, SkillDatabase.Instance.GetSkill(14));
            equipSkills.AddSkill(2, SkillDatabase.Instance.GetSkill(15));
        }

        for (int i=0; i<enemys.Length; i++) {
            Debug.Log("생성" + i);
            EquipSkills equipSkills = enemys[i].GetComponent<EquipSkills>();

            equipSkills.AddSkill(0, SkillDatabase.Instance.GetSkill(16));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
