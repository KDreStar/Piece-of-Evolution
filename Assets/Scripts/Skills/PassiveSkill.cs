using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "PassiveSkill")]
public class PassiveSkill : Skill
{
    //스탯 적용 공식
    [SerializeField]
    private string statFormula;
    public string StatFormula {
        get { return statFormula; }
    }
}
