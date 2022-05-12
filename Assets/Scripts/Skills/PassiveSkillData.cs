using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PassiveSkillData")]
public class PassiveSkillData : SkillData
{
    //스탯 적용 공식
    [SerializeField]
    private string statFormula;
    public string StatFormula {
        get { return statFormula; }
    }
}
