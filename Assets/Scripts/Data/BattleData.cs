using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
배틀시 캐릭터와 적 정보
*/
[Serializable]
public class BattleData
{
    public CharacterJSONData character;
    public CharacterJSONData enemy;

    public string characterModel;
    public string enemyModel;
}