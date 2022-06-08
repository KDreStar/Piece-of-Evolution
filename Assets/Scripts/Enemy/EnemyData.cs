using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

//Scene 전환시 적의 정보를 저장하는 용도
public class EnemyData : MonoBehaviour
{
    private static EnemyData instance = null;
    public static EnemyData Instance {
        get { return instance; }
    }

    [SerializeField]
    public Skill[] skillList = new Skill[8];
    public NNModel model;

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

    public void SetSkill(int i, int no) {
        skillList[i] = SkillDatabase.Instance.GetSkill(no);
    }

    public void SetSkill(int i, Skill skill) {
        skillList[i] = skill;
    }

    public Skill GetSkill(int i) {
        return skillList[i];
    }
}
