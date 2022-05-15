using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDatabase : MonoBehaviour
{
    //다른 방법으로 오브젝트를 생성하여 오브젝트 1개당 1개의 컴포넌트를 할당하는 방식이 있음
    //그게 더 좋아보임
    //public List<GameObject> skillList = new List<GameObject>();

    /*
    public List<Skill> skillList = new List<Skill>();
    public SkillData[] dataList;
    

    void Awake()
    {
        dataList = Resources.LoadAll<SkillData>("Skills");

        for (int i=0; i<dataList.Length; i++) {
            SkillData data = dataList[i];

            if (data is ActiveSkillData) {
                ActiveSkill temp = gameObject.AddComponent<ActiveSkill>();
                temp.activeSkillData = data as ActiveSkillData;
                skillList.Add(temp);
            } else if (data is PassiveSkillData) {
                PassiveSkill temp = gameObject.AddComponent<PassiveSkill>();
                temp.passiveSkillData = data as PassiveSkillData;
                skillList.Add(temp);
            }
        }

        if (skillList[0] is PassiveSkill) {
            PassiveSkill temp = skillList[0] as PassiveSkill;
            temp.Calculate();
        }

        if (skillList[1] is ActiveSkill) {
            ActiveSkill temp = skillList[1] as ActiveSkill;
            temp.Use();
        }
    }

    */

    
    public List<GameObject> skillList = new List<GameObject>();
    public SkillData[] dataList;

    private static SkillDatabase instance = null;
    public static SkillDatabase Instance {
        get { return instance; }
    }

    public Skill GetSkillByIndex(int i) {
        return skillList[i].GetComponent<Skill>();
    }

    public Skill GetSkill(int no) {
        for (int i=0; i<skillList.Count; i++) {
            Skill skill = GetSkillByIndex(i);
            int sno = -1;

            if (skill != null && skill.skillData.No == no)
                return skill;
        }

        return null;
    }

    void Awake()
    {
        dataList = Resources.LoadAll<SkillData>("Skills");

        for (int i=0; i<dataList.Length; i++) {
            SkillData data = dataList[i];
            
            //SkillDatabase의 자식으로
            GameObject skill = new GameObject();
            skill.name = "Skill" + data.No;
            skill.transform.parent = gameObject.transform;
            skillList.Add(skill);

            if (data is ActiveSkillData) {
                ActiveSkill temp = skill.AddComponent<ActiveSkill>();
                temp.SetData(data as ActiveSkillData);
            } else if (data is PassiveSkillData) {
                PassiveSkill temp = skill.AddComponent<PassiveSkill>();
                temp.SetData(data as PassiveSkillData);
            }
        }

        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        /*
        Skill temp1 = skillList[0].GetComponent<Skill>();

        if (temp1 is PassiveSkill) {
            PassiveSkill temp = skillList[0].GetComponent<PassiveSkill>();
            temp.Calculate();
        }

        Skill temp2 = skillList[1].GetComponent<Skill>();

        if (temp2 is ActiveSkill) {
            ActiveSkill temp = skillList[1].GetComponent<ActiveSkill>();
            temp.Use();
        }
        */
    }
}
