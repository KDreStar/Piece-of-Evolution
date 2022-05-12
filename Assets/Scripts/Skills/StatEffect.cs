using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat {
    HP, ATK, DEF, SPD
}

[CreateAssetMenu(menuName = "StatEffect")]
public class StatEffect : ScriptableObject
{  
    // 스탯 * multiple + plus
    public double multiple = 0;
    public double plus = 0;

    public Stat stat;
}
