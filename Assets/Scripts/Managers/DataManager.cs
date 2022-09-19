using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using Unity.Barracuda.ONNX;
using Unity.MLAgents.Policies;
using System.IO;
using UnityEngine.UI;
using System;

//데이터
public class DataManager
{
    public List<CharacterData> characterDataList;

    public int currentCharacterIndex = 0;
    public CharacterData characterData;
    public CharacterData enemyData;

    public SkillInventoryData skillInventoryData;

    public void Init() {
        characterDataList = new List<CharacterData>();
        characterData = new CharacterData();
        enemyData = new CharacterData();

        skillInventoryData = new SkillInventoryData();

        characterData.tag = "Character";
        enemyData.tag = "Enemy";
    }

    public void SaveCharacterDataList() {
        //for (int i=0; i<characterDataList.Length(); i++)
    }

    public void SaveCharacterData() {
        characterData.Save();
    }

    public void SaveEnemyData() {
        enemyData.Save();
    }

    public void LoadCharacterData() {
        characterData.Load();
    }

    public void LoadEnemyData() {
        enemyData.Load();
    }


}
