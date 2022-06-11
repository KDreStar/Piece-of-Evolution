using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance {
        get { return instance; }
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
    }

    public void ChangeScene(string name) {
        SceneManager.LoadScene(name);
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
        string result = BattleManager.Instance.result;
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
