using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

//Scene 전환시 캐릭터의 정보를 저장하는 용도
public class CharacterData : MonoBehaviour
{
    private static CharacterData instance = null;
    public static CharacterData Instance {
        get { return instance; }
    }

    [SerializeField]
    private Skill[] skillList = new Skill[8];

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
