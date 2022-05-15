using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkill : Skill
{
    public PassiveSkillData passiveSkillData;

    public void SetData(PassiveSkillData data) {
        skillData = data;
        passiveSkillData = data;
    }

    public void Calculate()
    {
        Debug.Log(passiveSkillData.StatFormula);
    }
}
