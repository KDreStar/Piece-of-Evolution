using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
플레이어 이름

캐릭터 목록
스킬 목록


*/

[Serializable]
public class GameData
{
    public string playerName;
    public int currentCharacterIndex;
    public int characterCreateCount;

    public List<CharacterData> characterDatas = new List<CharacterData>();
    public SkillInventoryData skillInventoryData = new SkillInventoryData();
}
