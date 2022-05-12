using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ActiveSkillData")]
public class ActiveSkillData : SkillData
{
    //기본 쿨타임
    [SerializeField]
    private float baseCooltime;
    public float BaseCooltime {
        get { return baseCooltime; }
    }

    //데미지 공식
    [SerializeField]
    private string damageFormula;
    public string DamageFormula {
        get { return damageFormula; }
    }
}
