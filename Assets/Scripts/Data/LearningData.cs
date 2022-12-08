using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.MLAgents.Policies;

/*
학습시 캐릭터와 적 정보
*/

[Serializable]
public class LearningData
{
    public CharacterData characterData;
    public List<CharacterData> enemyDatas;

    public LearningData() {
        characterData = new CharacterData();
        enemyDatas = new List<CharacterData>();
    }
}