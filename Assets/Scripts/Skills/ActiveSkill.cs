using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
    public ActiveSkillData activeSkillData;

    private float currentCooltime;

    public void SetData(ActiveSkillData data) {
        skillData = data;
        activeSkillData = data;
    }

    //스킬을 사용하면 프리펩을 생성하면 됨
    //프리펩에는 이펙트랑 스크립트를 가지고 있음
    //각 스킬 마다 구현하면 될 듯? ㅁ?ㄹ
    public void Use(GameObject caster)
    {
        if (currentCooltime > 0)
            return;

        GameObject effect = Instantiate(activeSkillData.Effect, caster.transform.position, caster.transform.rotation);
        effect.GetComponent<SkillEffect>().Init(caster);

        Debug.Log(activeSkillData.Name + "사용");
        Debug.Log(activeSkillData.DamageFormula);
    }
}
