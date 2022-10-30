using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using Unity.MLAgents.Policies;

public class BattleManager
{
    public bool isLearning; //true면 학습용도 전투 false면 1번 전투후 결과

    public float startCounter;
    public string result;

    public string judgeMessage;
    public Skill getSkill;

    //적의 정보를 세팅
    //학습 버튼을 클릭한 경우 mlagents-learn.exe 매개변수 관리후 실행
    //없는 경우 실행 불가
    public void BattleSetting(bool isLearning, CharacterData character, CharacterData enemy, bool playerControl=false) {
        this.isLearning = isLearning;
        
        Managers.Data.characterData.Save(character);
        Managers.Data.enemyData.Save(enemy);

        if (playerControl == true)
            Managers.Data.characterData.behaviorType = BehaviorType.HeuristicOnly;

        //저장하는 이유 = 학습시 유니티 게임을 다시 실행해서 공유해야됨
        //적 AI 가지고 와야함 우선 파일로 대체
        Managers.Data.SaveBattleData();

        int currentCharacterIndex = Managers.Data.currentCharacterIndex;

        if (isLearning) {
            //유니티 에디터에서는 그냥 1씬으로 함
            #if (UNITY_EDITOR)
                /*
                string path = Application.persistentDataPath + "/";
                string arg = "/c mlagents-learn "
                        + path + "models/Character.yaml "
                        + "--run-id=" + currentCharacterIndex + " "
                        + "--results-dir=" + (path + "models") + " ";

                if (Directory.Exists(path + "models/" + currentCharacterIndex) == true)
                    arg += "--resume ";

                UnityEngine.Debug.Log(arg);
                Process.Start("cmd.exe", arg);
                */

                GameManager.Instance.ChangeScene("Learning");

            #else
                //빈 파일을 생성함
                //열때 빈파일이 있으면 바로 러닝씬으로감
                //유니티 종료시 빈 파일 삭제
                GameManager.Instance.CreateLearningFile();

                int currentCharacterIndex = Managers.Data.currentCharacterIndex; 

                string path = Application.persistentDataPath + "/";
                string arg = "/c mlagents-learn "
                        + path + "models/Character.yaml "
                        + "--run-id=" + currentCharacterIndex + " "
                        + "--env=Piece-of-Evolution "
                        + "--num-envs=4 " //나중에 전역 수정가능하게
                        + "--width=480 "
                        + "--height=270 "
                        + "--results-dir=" + (path + "models");

                if (Directory.Exists(path + "models/" + currentCharacterIndex) == true)
                    arg += "--resume ";

                Process.Start("cmd.exe", arg);
            
            #endif
        } else {
            Time.timeScale = 0;
            Managers.Data.LoadBattleData();
            GameManager.Instance.ChangeScene("Battle");
        }
    }

    public void StartBattle() {
        UnityEngine.Debug.Log("배틀 시작?");

        if (isLearning == false) {
            Time.timeScale = 0;
            startCounter = 4.0f;
            Managers.Instance.StartCoroutine(DecreaseCount());
        } else {
            //Time.timeScale = 1.0f;
        }
    }

    public void StartLearning() {
        isLearning = true;

        Managers.Data.LoadBattleData();

        GameManager.Instance.ChangeScene("Learning");
    }

    IEnumerator DecreaseCount() {
        while (startCounter > 1) {
            startCounter -= Time.unscaledDeltaTime;
            yield return null;
        }

        //움직일 수 있음
        Time.timeScale = 1.0f;

        while (startCounter > 0) {
            startCounter -= Time.unscaledDeltaTime;
            yield return null;
        }

        //끝
    }

    public void EndBattle() {
        if (isLearning) {
            //mlagents-learn 종료
        }

        getSkill = null;
        getSkill = GameManager.Instance.JudgeGetSkill();

        if (getSkill != null) {
            judgeMessage = "스킬 획득!";
            Managers.Data.skillInventoryData.AddSkill(getSkill);
        } else {
            judgeMessage = "스킬 획득 실패...";
        }

        GameManager.Instance.ChangeScene("Result");        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
