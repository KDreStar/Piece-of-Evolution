using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;
using Unity.MLAgents.Policies;

[Serializable]
public class CharacterJSONData
{
    public string name;
    public float baseHP;
    public float baseATK;
    public float baseDEF;
    public float baseSPD;

    public int[] skillNoList;
    
    public BehaviorType behaviorType;

    public string aiName;
    public string modelPath;
    public PathType modelPathType;
    
    public string spritePath;
    public PathType spritePathType;
}
