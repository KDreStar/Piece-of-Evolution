using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDatabase : MonoBehaviour
{
    /*
    스킬을 그냥 스크립터블 오브젝트로 하고
    변하는 정보는 슬롯에 저장함
    */
    public Skill[] skillList;

    private static SkillDatabase instance = null;
    public static SkillDatabase Instance {
        get { return instance; }
    }

    public Skill GetSkill(int no) {
        for (int i=0; i<skillList.Length; i++) {
            if (skillList[i] != null && skillList[i].No == no)
                return skillList[i];
        }

        return null;
    }

    void Awake()
    {
        //Resources/Skills/* 모든 스킬 가져옴
        skillList = Resources.LoadAll<Skill>("Skills");

        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
