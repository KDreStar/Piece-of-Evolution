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

    public GameData gameData;
    //배틀용
    public CharacterData characterData;
    public CharacterData enemyData;

    public SkillInventoryData skillInventoryData;

    public void Init() {
        characterDataList = new List<CharacterData>();
        characterData = new CharacterData();
        enemyData = new CharacterData();

        skillInventoryData = new SkillInventoryData();
        skillInventoryData.Init();

        LoadGameData();
    }

    public void CreateCharacter(string name) {
        CharacterData temp = new CharacterData();

        temp.name = name;
        temp.baseHP = 500;
        temp.baseATK = 20;
        temp.baseDEF = 10;
        temp.baseSPD = 10;

        characterDataList.Add(temp);
        SaveGameData();
    }

    public bool DeleteCharacter() {
        if (currentCharacterIndex >= characterDataList.Count)
            return false;

        characterDataList.RemoveAt(currentCharacterIndex);
        SaveGameData();

        return true;
    }

    public bool SelectCharacter() {
        if (currentCharacterIndex >= characterDataList.Count)
            return false;

        characterData = characterDataList[currentCharacterIndex];

        return true;
    }

    public void SaveGameData() {
        gameData = new GameData();

        string path = Application.persistentDataPath + "/Game.json";

        gameData.playerName = "Temp";
        gameData.lastCharacterIndex = 0;

        for (int i=0; i<characterDataList.Count; i++) {
            CharacterData data = characterDataList[i];

            gameData.characterList.Add(data == null ? null : data.CreateJSONData());
        }

        List<Skill> skillList = skillInventoryData.skillList;

        for (int i=0; i<skillList.Count; i++) {
            Skill skill = skillList[i];
            int skillNo = skillList[i] == null ? 0 : skill.No;
            gameData.skillInvNoList.Add(skillNo);
        }

        string json = JsonUtility.ToJson(gameData);

        File.WriteAllText(path, json);
    }

    public void SaveBattleData() {
        BattleData battleData = new BattleData();
        string path = Application.persistentDataPath + "/Battle.json";

        battleData.character = characterData.CreateJSONData();
        battleData.enemy = enemyData.CreateJSONData();
        
        battleData.characterModel = currentCharacterIndex + ".onnx";
        battleData.enemyModel = "Enemy.onnx";

        string json = JsonUtility.ToJson(battleData);

        File.WriteAllText(path, json);
    }

    public void LoadGameData() {
        string path = Application.persistentDataPath + "/Game.json";
        string json = File.ReadAllText(path);

        gameData = JsonUtility.FromJson<GameData>(json);

        Debug.Log(gameData);

        characterDataList = new List<CharacterData>();
        for (int i=0; i<gameData.characterList.Count; i++) {
            CharacterJSONData characterJSONData = gameData.characterList[i];

            if (characterJSONData == null) {
                characterDataList.Add(null);
                continue;
            }

            CharacterData temp = new CharacterData();
            temp.SetJSONData(characterJSONData);
            temp.LoadModel(i + ".onnx");

            characterDataList.Add(temp);
        }

        List<Skill> skillList = skillInventoryData.skillList;
        for (int i=0; i<gameData.skillInvNoList.Count; i++) {
            int skillNo = gameData.skillInvNoList[i];

            Debug.Log(SkillDatabase.Instance.GetSkill(skillNo));
            skillList[i] = skillNo == 0 ? null : SkillDatabase.Instance.GetSkill(skillNo);
        }
    }

    public void LoadBattleData() {
        string path = Application.persistentDataPath + "/Battle.json";
        string json = File.ReadAllText(path);

        BattleData battleData = JsonUtility.FromJson<BattleData>(json);

        characterData.SetJSONData(battleData.character);
        characterData.LoadModel(battleData.characterModel);

        enemyData.SetJSONData(battleData.enemy);
        enemyData.LoadModel(battleData.enemyModel);
    }

    public bool IsCharacter(int i) {
        if (i >= characterDataList.Count)
            return false;
 
        return true;
        //return characterDataList[i] != null ? true : false;
    }

    public void GetCharacterData(GameObject character) {
        characterData.Get(character);
    }

    public void GetCharacterData(int i, GameObject character) {
        if (i >= characterDataList.Count)
            return;

        characterDataList[i].Get(character);
    }

    public void GetEnemyData(GameObject character) {
        enemyData.Get(character);
    }

    public void SetCharacterData(GameObject character) {
        characterData.Set(character);
    }

    public void SetEnemyData(GameObject character) {
        enemyData.Set(character);
    }
}
