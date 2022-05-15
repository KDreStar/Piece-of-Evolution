using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    string name; //이름

    float currentHP;
    float maxHP;
    float baseHP;

    float currentATK;
    float baseATK;
    
    float currentDEF; //방어력 DEF
    float baseDEF; //기본 방어력 baseDEF

    float currentSPD; //스피드 SPD
    float baseSPD; //기본 스피드 baseSPD

    public float CurrentSPD {
        get { return currentSPD; }
    }

    //버프
    //스킬
    private SkillSlot skillSlot;

    void CalculateStat() {
        //패시브 스킬, 버프등을 처리하고 currentStat을 설정
        for (int i=0; i<SkillSlot.maxSlot; i++) {
            PassiveSkill skill = skillSlot.GetPassiveSkill(i);

            if (skill == null)
                continue;
            
            skill.Calculate();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        baseATK = 20;
        baseDEF = 10;
        baseHP = 500;
        baseSPD = 10;

        currentATK = baseATK;
        currentSPD = baseSPD;

        //현재 오브젝트에 부착되어 있는 것중 SkillSlot을 가져옴
        skillSlot = GetComponent<SkillSlot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
