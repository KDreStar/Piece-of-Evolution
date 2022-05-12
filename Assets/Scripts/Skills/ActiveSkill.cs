using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
    public ActiveSkillData activeSkillData;

    private float currentCooltime;

    //스킬을 사용하면 각 스킬에 맞는 오브젝트를 생성하여 보여주는 방식으로 코딩
    //TO-DO ActiveSkillData에 오브젝트(스킬 이펙트)들을 할당해야함
    public void Use()
    {
        Debug.Log(activeSkillData.Name + "사용");
        Debug.Log(activeSkillData.DamageFormula);
    }
}
