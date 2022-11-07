using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Buff
{
    [SerializeField]
    public Effect effect;

    [SerializeField]
    public float duration;

    [SerializeField]
    public bool infinity = false;
}
