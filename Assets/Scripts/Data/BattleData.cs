using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.MLAgents.Policies;

/*
배틀시 캐릭터와 적 정보
*/

[Serializable]
public class BattleData
{
    public CharacterData characterData;
    public CharacterData enemyData;
}