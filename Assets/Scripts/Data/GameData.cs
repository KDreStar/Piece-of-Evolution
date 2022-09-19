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
    public int lastCharacterIndex;

    public List<CharacterJSONData> characterList = new List<CharacterJSONData>();
    public List<int> skillInvNoList = new List<int>();
}
