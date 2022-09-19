using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//수량 없이 ㄱ 
public class SkillInventoryData
{
    public int page = 1;
    public List<Skill> skillList;
    
    public void Init()
    {
        skillList = new List<Skill>();

        for (int i=0; i<100; i++) {
            skillList.Add(null);
        }
    }

    public Skill GetSkill(int i) {
        return skillList[i];
    }

    public void AddSkill(Skill skill) {
        for (int i=0; i<skillList.Count; i++) {
            if (skillList[i] == null) {
                skillList[i] = skill;
                return;
            }
        }

        skillList.Add(skill);
    }

    public List<Skill> GetSkillList() {
        return skillList;
    }
}
