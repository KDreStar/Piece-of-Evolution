using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkill : Skill
{
    public PassiveSkillData passiveSkillData;

    public void Calculate()
    {
        Debug.Log(passiveSkillData.StatFormula);
    }
}
