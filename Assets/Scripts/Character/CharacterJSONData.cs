using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;

[Serializable]
public class CharacterJSONData
{
    public string name;
    public float baseHP;
    public float baseATK;
    public float baseDEF;
    public float baseSPD;

    public int[] skillNoList;
}
