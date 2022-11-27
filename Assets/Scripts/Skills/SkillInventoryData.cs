using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//수량 없이 ㄱ 
[Serializable]
public class SkillInventoryData
{
    public List<int> skillNos;
    
    public SkillInventoryData() {
        skillNos = new List<int>();
    }

    public void AddSkill(int skillNo) {
        if (skillNo == 0) {
            skillNos.Add(skillNo);
            return;
        }

        for (int i=0; i<skillNos.Count; i++) {
            if (skillNos[i] == 0) {
                skillNos[i] = skillNo;
                return;
            }
        }

        skillNos.Add(skillNo);
    }

    public void AddSkill(Skill skill) {
        if (skill == null)
            return;
        
        AddSkill(skill.No);
    }
}
