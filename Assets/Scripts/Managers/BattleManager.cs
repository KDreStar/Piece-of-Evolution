using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using Unity.MLAgents.Policies;
using System.Runtime.InteropServices;
using System.Data;
using System;

public class BattleManager
{
    public float baseSpeed = 0.3f;

    public bool isLearning; //true면 학습용도 전투 false면 1번 전투후 결과

    public float startCounter;
    public string result;

    public string judgeMessage;
    public Skill getSkill;

    public Process trainer;

    //적의 정보를 세팅
    //학습 버튼을 클릭한 경우 mlagents-learn.exe 매개변수 관리후 실행
    //없는 경우 실행 불가
    public void BattleSetting(bool isLearning, CharacterData character, CharacterData enemy) {
        this.isLearning = isLearning;
        
        //저장하는 이유 = 학습시 유니티 게임을 다시 실행해서 공유해야됨
        //적 AI 가지고 와야함 우선 파일로 대체
        if (isLearning == false)
            Managers.Data.SaveBattleData(character, enemy);
        else
            Managers.Data.SaveLearningData(character, enemy);


        if (isLearning) {
            int characterNo = character.no;

            //유니티 에디터에서는 그냥 1씬으로 함
            #if (UNITY_EDITOR)
                string path = Application.persistentDataPath + "/";
                string arg = "mlagents-learn "
                        + path + "models/Character.yaml "
                        + "--run-id=" + characterNo + " "
                        + "--results-dir=" + (path + "models") + " ";

                bool exist = File.Exists(string.Format("{0}models/{1}/Character.onnx", path, characterNo));
                if (exist == true)
                    arg += "--resume ";
                else
                    arg += "--force ";

                UnityEngine.Debug.Log(arg);

                trainer = new Process();
                trainer.StartInfo.FileName = "cmd";
                trainer.StartInfo.Arguments = "/c " + arg;
                trainer.StartInfo.RedirectStandardError = false;
                trainer.StartInfo.RedirectStandardInput = false;
                trainer.StartInfo.RedirectStandardOutput = false;
                trainer.StartInfo.UseShellExecute = true;
                trainer.StartInfo.Verb = "runas";
                trainer.EnableRaisingEvents = true;

                
                trainer.OutputDataReceived += new DataReceivedEventHandler((s, e) => 
                { 
                    UnityEngine.Debug.Log("[Trainer] " + e.Data); 
                });
                trainer.ErrorDataReceived += new DataReceivedEventHandler((s, e) =>
                { 
                    UnityEngine.Debug.Log("[Trainer] " + e.Data); 
                });
                

                trainer.Start();
                //trainer.BeginOutputReadLine();
                //trainer.BeginErrorReadLine();
                //GameManager.Instance.ChangeScene("Learning");

            #else
                //빈 파일을 생성함
                //열때 빈파일이 있으면 바로 러닝씬으로감
                //유니티 종료시 빈 파일 삭제
                GameManager.Instance.CreateLearningFile();

                string path = Application.persistentDataPath + "/";
                string arg = ""
                        + path + "models/Character.yaml "
                        + "--run-id=" + characterNo + " "
                        + "--env=Piece-of-Evolution "
                        //+ "--num-envs=4 " //나중에 전역 수정가능하게
                        + "--width=480 "
                        + "--height=270 "
                        + "--time-scale=1 "
                        //+ "--no-graphics "
                        //+ "--torch-device=cuda "
                        + "--results-dir=" + (path + "models") + " ";

                bool exist = File.Exists(string.Format("{0}models/{1}/Character.onnx", path, characterNo));
                if (exist == true)
                    arg += "--resume ";
                else
                    arg += "--force ";

                Process.Start("mlagents-learn.exe", arg);

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

        Managers.Data.LoadLearningData();

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

    public float CalculateFormula(string formula, Status attacker) {
        return CalculateFormula(formula, attacker, attacker);
    }

    public float CalculateDamage(string formula, Status attacker, Status defender) {
        return CalculateFormula(formula, attacker, defender) - defender.CurrentDEF;
    }

    public float CalculateFormula(string formula, Status attacker, Status defender) {
        DataTable table = new DataTable();

        string[] statList = {
            "CHP", "HP", "MHP", "ATK", "DEF", "SPD", //없으면 공격자 기준
            "A.CHP", "A.MHP", "A.ATK", "A.DEF", "A.SPD",
            "D.CHP", "D.MHP", "D.ATK", "D.DEF", "D.SPD"
        };

        float[] statValue = {
            attacker.CurrentHP, attacker.MaxHP, attacker.MaxHP, attacker.CurrentATK, attacker.CurrentDEF, attacker.CurrentSPD,
            attacker.CurrentHP, attacker.MaxHP, attacker.CurrentATK, attacker.CurrentDEF, attacker.CurrentSPD,
            defender.CurrentHP, defender.MaxHP, defender.CurrentATK, defender.CurrentDEF, defender.CurrentSPD
        };

        List<string> formulaWords = new List<string>();
        formulaWords.AddRange(formula.Split(" "));

        //열 생성
        for (int i=0; i<statList.Length; i++) {
            if (formulaWords.Contains(statList[i]))
                table.Columns.Add(statList[i], typeof(float));
        }

        table.Columns.Add("Result").Expression = formula;

        //값 생성
        DataRow row = table.Rows.Add();
        
        for (int i=0; i<statList.Length; i++) {
            if (formulaWords.Contains(statList[i]))
                row[statList[i]] = statValue[i];
        }

        //계산
        table.BeginLoadData();
        table.EndLoadData();

        return float.Parse(row["Result"].ToString());
    }

    public void EndBattle() {
        if (isLearning) {
            //mlagents-learn 종료
        }

        getSkill = null;
        getSkill = GameManager.Instance.JudgeGetSkill();

        if (getSkill != null) {
            judgeMessage = "스킬 획득!";
            Managers.Data.gameData.skillInventoryData.AddSkill(getSkill);
            Managers.Data.SaveGameData();
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
