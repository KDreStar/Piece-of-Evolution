using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

using System.Text; 
using System.IO; 
using System.Runtime.InteropServices; 

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance {
        get { return instance; }
    }

    private string learningPath;

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
            return SkillDatabase.Instance.GetRandomSkill(1);

        if (second < 80)
            return SkillDatabase.Instance.GetRandomSkill(2);

        if (second < 92)
            return SkillDatabase.Instance.GetRandomSkill(3);

        if (second < 98)
            return SkillDatabase.Instance.GetRandomSkill(4);

        return SkillDatabase.Instance.GetRandomSkill(5);
    }
}
