using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using Unity.Barracuda.ONNX;
using Unity.MLAgents.Policies;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;



//게임 저장 데이터 관련

/*
파일 <-> 메모리 <-> 오브젝트


*/

public class DataManager
{
    public List<CharacterData> characterDataList;

    public int currentCharacterIndex = 0;

    public GameData gameData;
    
    //UI 불러오기용도
    public CharacterData currentCharacterData;

    //배틀용
    public CharacterData characterData;
    public CharacterData enemyData;

    public SkillInventoryData skillInventoryData;

    public AIFactory AIFactory;

    public void Init() {
        characterDataList = new List<CharacterData>();

        currentCharacterData = new CharacterData();

        characterData = new CharacterData();
        enemyData = new CharacterData();

        skillInventoryData = new SkillInventoryData();
        skillInventoryData.Init();

        AIFactory = new AIFactory();

        if (File.Exists(Application.persistentDataPath + "/Game.json") == false)
            SaveGameData();

        LoadGameData();
    }

    public void CreateCharacter(string name) {
        CharacterData temp = new CharacterData();

        temp.name = name;
        temp.baseHP = 500;
        temp.baseATK = 20;
        temp.baseDEF = 10;
        temp.baseSPD = 10;

        temp.aiName = "" + characterDataList.Count;
        temp.modelPath = "models/" + characterDataList.Count + "/Character.onnx";
        temp.modelPathType = PathType.Local;

        temp.spritePath = "Characters/Mask Dude/Fall (32x32)";
        temp.spritePathType = PathType.Resources;
        //temp.spritePath = "images/" + characterDataList.Count + ".png";
        //temp.spritePathType = PathType.Local;

        characterDataList.Add(temp);
        SaveGameData();
    }

    public bool DeleteCharacter(int i) {
        if (i >= characterDataList.Count)
            return false;

        characterDataList.RemoveAt(i);
        SaveGameData();

        return true;
    }

    public bool SelectCharacter(int i) {
        if (i >= characterDataList.Count)
            return false;

        currentCharacterData = characterDataList[i];
        currentCharacterIndex = i;

        return true;
    }

    public void SaveGameData() {
        gameData = new GameData();

        string path = Application.persistentDataPath + "/Game.json";

        gameData.playerName = "Temp";
        gameData.lastCharacterIndex = currentCharacterIndex;

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
        
        string json = JsonUtility.ToJson(battleData);

        File.WriteAllText(path, json);
    }

    //public void LoadPvPCharacters(string json) {
    //    pvpData = JsonUtility.FromJson<PvPData>(json);
    //}

    public void LoadGameData() {
        string path = Application.persistentDataPath + "/Game.json";
        string json = File.ReadAllText(path);

        gameData = JsonUtility.FromJson<GameData>(json);

        Debug.Log(gameData);

        characterDataList.Clear();
        for (int i=0; i<gameData.characterList.Count; i++) {
            CharacterJSONData characterJSONData = gameData.characterList[i];

            if (characterJSONData == null) {
                characterDataList.Add(null);
                continue;
            }

            CharacterData temp = new CharacterData();
            temp.SetJSONData(characterJSONData);
            //temp.LoadModel(i + ".onnx");
            temp.LoadSprite();
            characterDataList.Add(temp);
        }

        List<Skill> skillList = skillInventoryData.skillList;
        for (int i=0; i<gameData.skillInvNoList.Count; i++) {
            int skillNo = gameData.skillInvNoList[i];

            Debug.Log(Managers.DB.SkillDB.GetSkill(skillNo));
            skillList[i] = skillNo == 0 ? null : Managers.DB.SkillDB.GetSkill(skillNo);
        }

        SelectCharacter(gameData.lastCharacterIndex);
    }

    public void LoadBattleData() {
        string path = Application.persistentDataPath + "/Battle.json";
        string json = File.ReadAllText(path);

        BattleData battleData = JsonUtility.FromJson<BattleData>(json);

        characterData.SetJSONData(battleData.character);
        characterData.LoadModel();

        enemyData.SetJSONData(battleData.enemy);
        enemyData.LoadModel();
    }

    public void LoadCharacterData(GameObject character) {
        characterData.Load(character);
    }

    public void LoadCurrentCharacterData(GameObject character) {
        currentCharacterData.Load(character);
    }

    public void LoadCharacterData(int i, GameObject character) {
        if (i >= characterDataList.Count)
            return;

        characterDataList[i].Load(character);
    }

    public void LoadEnemyData(GameObject character) {
        enemyData.Load(character);
    }

    public void SaveCurrentCharacterData(GameObject character) {
        currentCharacterData.Save(character);
    }

    public void SaveCharacterData(GameObject character) {
        characterData.Save(character);
    }

    public void SaveEnemyData(GameObject character) {
        enemyData.Save(character);
    }
}
