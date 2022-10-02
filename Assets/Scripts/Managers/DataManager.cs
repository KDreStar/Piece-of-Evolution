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

    public bool SelectCharacter(int i) {
        if (i >= characterDataList.Count)
            return false;

        characterData = characterDataList[i];
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
        
        battleData.characterModel = currentCharacterIndex + ".onnx";
        battleData.enemyModel = "Enemy.onnx";

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
            temp.LoadModel(i + ".onnx");

            characterDataList.Add(temp);
        }

        List<Skill> skillList = skillInventoryData.skillList;
        for (int i=0; i<gameData.skillInvNoList.Count; i++) {
            int skillNo = gameData.skillInvNoList[i];

            Debug.Log(SkillDatabase.Instance.GetSkill(skillNo));
            skillList[i] = skillNo == 0 ? null : SkillDatabase.Instance.GetSkill(skillNo);
        }

        SelectCharacter(lastCharacterIndex);
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

    public void LoadCharacterData(GameObject character) {
        characterData.Load(character);
    }

    public void LoadCharacterData(int i, GameObject character) {
        if (i >= characterDataList.Count)
            return;

        characterDataList[i].Load(character);
    }

    public void LoadEnemyData(GameObject character) {
        enemyData.Load(character);
    }

    public void SaveCharacterData(GameObject character) {
        characterData.Save(character);
    }

    public void SaveEnemyData(GameObject character) {
        enemyData.Save(character);
    }

    public void ChangePvPList(List<CharacterData> datas) {
        GameManager.Instance.StartCoroutine(DownloadPvPCharacters(datas));
    }

    IEnumerator DownloadPvPCharacters(List<CharacterData> datas) {
        string pvpURL = "http://localhost:8080/getPvPCharacters.jsp";
        WWWForm form = new WWWForm();
        /*
        string id = "아이디";
        string pw = "비밀번호";
        form.AddField("Username", id);
        form.AddField("Password", pw);
        */

        form.AddField("count", 8);
        UnityWebRequest www = UnityWebRequest.Get(pvpURL);  // 보낼 주소와 데이터 입력

        yield return www.SendWebRequest();  // 응답 대기

        if (www.error == null) {
            Debug.Log("다운로드 완료");
            Debug.Log(www.downloadHandler.text);    // 데이터 출력
            Debug.Log(www.downloadHandler.data);    // 데이터 출력

            Managers.Data.LoadPvPCharacters(datas, www.downloadHandler.text);
        } else {
            Debug.Log("error");
        }
    }

    public void LoadPvPCharacters(List<CharacterData> datas, string json) {
        PvPData pvpData = new PvPData();

        pvpData = JsonUtility.FromJson<PvPData>(json);

        datas.Clear();
        for (int i=0; i<pvpData.characterList.Count; i++) {
            CharacterJSONData characterJSONData = pvpData.characterList[i];

            CharacterData temp = new CharacterData();
            temp.SetJSONData(characterJSONData);

            //임시 이거는 배틀 들어갈때 다운로드 하게 ㄱㄱ
            //temp.LoadModel("Enemy.onnx");

            datas.Add(temp);
        }

        //Debug.Log("datas" + pvpDataList + " " + pvpDataList.Count);
    }
}
