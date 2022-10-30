using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

using System.Text; 
using System.IO; 
using System.Runtime.InteropServices; 
using UnityEngine.Networking;


//게임 진행 관련
public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance {
        get { return instance; }
    }

    private string learningPath;

    public string monsterFieldName;

    public void SetMonsterFieldName(string name) {
        monsterFieldName = name;
    }

    //싱글톤
    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
  
        learningPath = Application.persistentDataPath + "/Learning";
    }

    void Start() {
        if (File.Exists(learningPath)) {
            Managers.Battle.StartLearning();
        }
    }

    public void ChangeScene(string name) {
        SceneManager.LoadScene(name);
    }

    public void CreateLearningFile() {
        File.WriteAllText(learningPath, "");
    }

    public void DeleteLearningFile() {
        if (File.Exists(learningPath))
            File.Delete(learningPath);
    }

    public Vector2 GetCanvasSize() {
        return new Vector2(1920, 1080);
    }

    void OnApplicationQuit() {
        DeleteLearningFile();
    }



    /*
    승리 => 기본 20%
    패배 => 기본 10%
    무승부 => 기본 15%

    1성 50%
    2성 30%
    3성 12%
    4성 6%
    5성 2%
    */
    public Skill JudgeGetSkill() {
        string result = Managers.Battle.result;
        int percent = 15;
        int first = Random.Range(0, 100);

        Debug.Log("Dice: " + first);

        if (result.Equals("승리"))
            percent = 20;
        if (result.Equals("패배"))
            percent = 10;

        //0~19 까지 통과 [승리]
        if (first >= percent)
            return null;

        int second = Random.Range(0, 100);

        Debug.Log("Dice: " + second);

        if (second < 50)
            return Managers.DB.SkillDB.GetRandomSkill(1);

        if (second < 80)
            return Managers.DB.SkillDB.GetRandomSkill(2);

        if (second < 92)
            return Managers.DB.SkillDB.GetRandomSkill(3);

        if (second < 98)
            return Managers.DB.SkillDB.GetRandomSkill(4);

        return Managers.DB.SkillDB.GetRandomSkill(5);
    }
}
