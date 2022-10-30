using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PvPListController : MonoBehaviour
{
    public CharacterList characterList;
    public List<CharacterData> pvpDataList = new List<CharacterData>();

    public void ChangeList() {
        StartCoroutine(DownloadPvPCharacters());
    }

    public void StartPvP() {
        Managers.Battle.BattleSetting(false, Managers.Data.currentCharacterData, pvpDataList[characterList.GetCurrentIndex()]);
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

        pvpDataList.Clear();
        for (int i=0; i<pvpData.characterList.Count; i++) {
            CharacterJSONData characterJSONData = pvpData.characterList[i];

            CharacterData temp = new CharacterData();
            temp.SetJSONData(characterJSONData);

            temp.modelPath = "Temp.onnx";
            temp.modelPathType = PathType.Local;

            //임시 이거는 배틀 들어갈때 다운로드 하게 ㄱㄱ
            //temp.LoadModel("Enemy.onnx");

            pvpDataList.Add(temp);
        }

        //Debug.Log("datas" + pvpDataList + " " + pvpDataList.Count);
    }

    void Start() {
        ChangeList();

        characterList.SetDatas(pvpDataList);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
