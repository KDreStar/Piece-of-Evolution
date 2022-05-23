using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    string name; //이름

    float currentHP;
    public float CurrentHP {
        get { return currentHP; }
    }

    float maxHP;
    public float MaxHP {
        get { return maxHP; }
    }
    public float baseHP;

    float currentATK;
    public float CurrentATK {
        get { return currentATK; }
    }
    public float baseATK;
    
    float currentDEF; //방어력 DEF
    public float CurrentDEF {
        get { return currentDEF; }
    }
    public float baseDEF; //기본 방어력 baseDEF

    float currentSPD; //스피드 SPD
    public float baseSPD; //기본 스피드 baseSPD

    public float CurrentSPD {
        get { return currentSPD; }
    }

    //버프
    //스킬
    private EquipSkills equipSkills;

    void CalculateStat() {
        //패시브 스킬, 버프등을 처리하고 currentStat을 설정
        for (int i=0; i<EquipSkills.maxSlot; i++) {
            PassiveSkill skill = equipSkills.GetPassiveSkill(i);

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

        maxHP = baseHP;

        currentHP  = baseHP;
        currentATK = baseATK;
        currentDEF = baseDEF;
        currentSPD = baseSPD;

        equipSkills = GetComponent<EquipSkills>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Temp
    public void SetHP(float hp) {
        currentHP = hp;
    }

    public void TakeDamage(float damage) {
        currentHP -= damage;
    }
}
