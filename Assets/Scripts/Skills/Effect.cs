using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EffectTag {
    NONE,
    ATTACKER_MHP,
    ATTACKER_CHP,
    ATTACKER_ATK,
    ATTACKER_DEF,
    ATTACKER_SPD,
    DEFENDER_MHP,
    DEFENDER_CHP, //데미지
    DEFENDER_ATK,
    DEFENDER_DEF,
    DEFENDER_SPD,
    SHIELD,
}

public enum EffectOperator {
    SET,
    ADD,
    SUB,
    MUL,
    DIV
}

[Serializable]
public class Effect
{
    [SerializeField]
    private EffectTag effectTag;
    public EffectTag EffectTag {
        get { return effectTag; }
    }

    [SerializeField]
    private EffectOperator effectOperator;
    public EffectOperator EffectOperator {
        get { return effectOperator; }
    }

    [SerializeField]
    private string formula;
    public string Formula {
        get { return formula; }
    }
}
