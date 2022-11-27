using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.MLAgents.Policies;

public class PvPListController : MonoBehaviour
{
    public CharacterList characterList;

    public void ChangeList() {
        StartCoroutine(DownloadPvPCharacters());
    }

    public void StartPvP() {
        CharacterData character = Managers.Data.GetCurrentCharacterData();
        CharacterData enemy     = characterList.GetCurrentCharacterData();

        character.behaviorType = BehaviorType.Default;
        enemy.behaviorType     = BehaviorType.InferenceOnly;

        Managers.Battle.BattleSetting(false, character, enemy);
    }

    IEnumerator DownloadPvPCharacters() {
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

            LoadPvPCharacters(www.downloadHandler.text);
        } else {
            Debug.Log("error");
        }
    }

    public void LoadPvPCharacters(string json) {
        PvPData pvpData = new PvPData();

        pvpData = JsonUtility.FromJson<PvPData>(json);
        characterList.SetDatas(pvpData.characterDatas);
    }

    void Start() {
        ChangeList();

        //characterList.SetDatas(pvpDataList);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
