using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDatabase
{

    /*
    스킬을 그냥 스크립터블 오브젝트로 하고
    변하는 정보는 슬롯에 저장함
    */
    public Skill[] skillList;
    public Dictionary<int, Skill> skillTable = new Dictionary<int, Skill>();

    public SkillDatabase() {
        skillList = Resources.LoadAll<Skill>("Skills");

        for (int i=0; i<skillList.Length; i++)
            skillTable.Add(skillList[i].No, skillList[i]);
    }

    public Skill GetSkill(int no) {
        for (int i=0; i<skillList.Length; i++) {
            if (skillList[i] != null && skillList[i].No == no)
                return skillList[i];
        }

        return null;
    }

    public Skill GetRandomSkill() {
        return skillList[Random.Range(0, skillList.Length)];
    }

    private List<Skill> GetSkillList(int cost) {
        List<Skill> temp = new List<Skill>();

        for (int i=0; i<skillList.Length; i++) {
            if (skillList[i].Cost == cost)
                temp.Add(skillList[i]);
        }

        return temp;
    }

    public Skill GetRandomSkill(int cost) {
        List<Skill> temp = GetSkillList(cost);

        return temp[Random.Range(0, temp.Count)];
    }




}
