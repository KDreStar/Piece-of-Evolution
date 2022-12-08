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
파일 -> 임시 테이블 -> 메모리 -> 오브젝트
임시 테이블 -> 오브젝트도 되긴 하지만
모델 로드시 여러개 로드해야되면 1개만 로드하고 해야하므로

*/

public class DataManager
{
    public GameData gameData = new GameData();

    //배틀용
    public BattleData battleData = new BattleData();

    public LearningData learningData = new LearningData();

    public const int MaxEnemyCount = 5;

    public void Init() {
        if (File.Exists(Application.persistentDataPath + "/Game.json") == false)
            SaveFirstGameData();

        LoadGameData();
        LoadLearningData();
    }

    public void CreateCharacter(string name) {
        CharacterData temp = new CharacterData();

        temp.no = gameData.characterCreateCount;

        temp.name = name;
        temp.baseHP = 500;
        temp.baseATK = 20;
        temp.baseDEF = 10;
        temp.baseSPD = 10;

        temp.aiName = "" + gameData.characterCreateCount;
        temp.modelPath = "models/" + gameData.characterCreateCount + "/Character.onnx";
        temp.modelPathType = PathType.Local;

        temp.spritePath = "Characters/Mask Dude/Fall (32x32)";
        temp.spritePathType = PathType.Resources;

        gameData.characterCreateCount++;

        gameData.characterDatas.Add(temp);
        SaveGameData();
    }

    public bool DeleteCharacter(int i) {
        if (i >= gameData.characterDatas.Count)
            return false;

        gameData.characterDatas.RemoveAt(i);
        SaveGameData();

        return true;
    }

    public bool SelectCharacter(int i) {
        if (i >= gameData.characterDatas.Count)
            return false;

        gameData.currentCharacterIndex = i;

        SaveGameData();

        return true;
    }

    public bool ExistCharacter(int i) {
        if (i >= gameData.characterDatas.Count)
            return false;

        if (gameData.characterDatas[i] == null)
            return false;

        return true;
    }

    //스킬 주고
    public void SaveFirstGameData() {
        gameData.playerName = "Temp";
        gameData.currentCharacterIndex = 0;
        gameData.characterCreateCount = 0;

        SkillInventoryData inv = gameData.skillInventoryData;

        inv.AddSkill(1);
        inv.AddSkill(2);
        inv.AddSkill(3);
        inv.AddSkill(4);
        inv.AddSkill(21);
        inv.AddSkill(22);
        inv.AddSkill(23);
        inv.AddSkill(25);
        inv.AddSkill(1);
        inv.AddSkill(1);

        for (int i=0; i<90; i++)
            inv.AddSkill(0);

        SaveGameData();
    }

    public void SaveGameData() {
        string path = Application.persistentDataPath + "/Game.json";

        string json = JsonUtility.ToJson(gameData);

        File.WriteAllText(path, json);
    }

    public void SaveBattleData(CharacterData character, CharacterData enemy) {
        string path = Application.persistentDataPath + "/Battle.json";

        battleData.characterData = character;
        battleData.enemyData = enemy;

        string json = JsonUtility.ToJson(battleData);

        File.WriteAllText(path, json);
    }

    public void SaveLearningData(CharacterData character, CharacterData enemy) {
        string path = Application.persistentDataPath + "/Learning.json";

        learningData.characterData = character;
        learningData.enemyDatas.Add(enemy);

        if (learningData.enemyDatas.Count > MaxEnemyCount)
            learningData.enemyDatas.RemoveAt(0);

        string json = JsonUtility.ToJson(learningData);

        File.WriteAllText(path, json);
    }


    public void LoadGameData() {
        string path = Application.persistentDataPath + "/Game.json";

        if (File.Exists(path) == false)
            return;

        string json = File.ReadAllText(path);

        gameData = JsonUtility.FromJson<GameData>(json);

        for (int i=0; i<gameData.characterDatas.Count; i++) {
            gameData.characterDatas[i].LoadSprite();
        }
    }

    public void LoadBattleData() {
        string path = Application.persistentDataPath + "/Battle.json";

        if (File.Exists(path) == false)
            return;

        string json = File.ReadAllText(path);

        battleData = JsonUtility.FromJson<BattleData>(json);

        battleData.characterData.LoadSprite();
        battleData.enemyData.LoadSprite();

        battleData.characterData.LoadModel();
        battleData.enemyData.LoadModel();
    }

    public void LoadLearningData() {
        string path = Application.persistentDataPath + "/Learning.json";

        if (File.Exists(path) == false)
            return;

        string json = File.ReadAllText(path);


        learningData = JsonUtility.FromJson<LearningData>(json);

        learningData.characterData.LoadSprite();
        learningData.characterData.LoadModel();

        for (int i=0; i<learningData.enemyDatas.Count; i++) {
            learningData.enemyDatas[i].LoadModel();
            learningData.enemyDatas[i].LoadSprite();
        }
    }

    public void SetCurrentCharacterData(Character character) {
        int cur = gameData.currentCharacterIndex;

        gameData.characterDatas[cur].SetData(character);
    }

    public CharacterData GetCurrentCharacterData() {
        int cur = gameData.currentCharacterIndex;

        return gameData.characterDatas[cur];
    }

    public void SetCharacterData(int i, Character character) {
        gameData.characterDatas[i].SetData(character);
    }

    public CharacterData GetCharacterData(int i) {
        return gameData.characterDatas[i];
    }

    public void SetBattleCharacterData(Character character) {
        battleData.characterData.SetData(character);
    }

    public CharacterData GetBattleCharacterData() {
        return battleData.characterData;
    }

    public void SetBattleEnemyData(Character character) {
        battleData.enemyData.SetData(character);
    }

    public CharacterData GetBattleEnemyData() {
        return battleData.enemyData;
    }

    public CharacterData GetLearningCharacterData() {
        return learningData.characterData;
    }

    public CharacterData GetLearningEnemyData(int i) {
        if (i >= learningData.enemyDatas.Count)
            i = learningData.enemyDatas.Count - 1;

        return learningData.enemyDatas[i];
    }
}
