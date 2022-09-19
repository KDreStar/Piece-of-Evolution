using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;
using Unity.MLAgents.Policies;

public class Status : MonoBehaviour
{
    public string name;

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

    public void SetDefaultStat() {
        maxHP = baseHP;
        currentATK = baseATK;
        currentDEF = baseDEF;
        currentSPD = baseSPD;
    }

    void CalculateStat() {
        SetDefaultStat();
        //패시브 스킬, 버프등을 처리하고 currentStat을 설정
        for (int i=0; i<EquipSkills.maxSlot; i++) {
            PassiveSkill skill = equipSkills.GetPassiveSkill(i);

            if (skill == null)
                continue;
            
            ApplyPassiveSkill(skill);
        }
    }

    enum Stat {HP, ATK, DEF, SPD}

    private void ApplyPassiveSkill(PassiveSkill skill) {
        string[] formulaList = skill.StatFormula.Replace(" ", "").Split(',');
       
        /*
        HP = (ATK + 50) * 2
        HP
        (ATK+50)*2
        */ 
        for (int i=0; i<formulaList.Length; i++) {
            string[] formula = formulaList[i].Split('=');
            
            float result = Calculate(formula[1]);

            Stat stat = (Stat)Enum.Parse(typeof(Stat), formula[0]);

            switch (stat) {
                case Stat.HP:
                    maxHP = result;
                    break;

                case Stat.ATK:
                    currentATK = result;
                    break;

                case Stat.DEF:
                    currentDEF = result;
                    break;

                case Stat.SPD:
                    currentSPD = result;
                    break;
            }
        }
    }

    public float Calculate(string formula) {
        DataTable table = new DataTable();
        string[] statList = {"HP", "ATK", "DEF", "SPD"};
        float[] statValue = {maxHP, currentATK, currentDEF, currentSPD};

        //열 생성
        for (int i=0; i<statList.Length; i++) {
            if (formula.Contains(statList[i]))
                table.Columns.Add(statList[i], typeof(float));
        }

        table.Columns.Add("Result").Expression = formula;

        //값 생성
        DataRow row = table.Rows.Add();
        
        for (int i=0; i<statList.Length; i++) {
            if (formula.Contains(statList[i]))
                row[statList[i]] = statValue[i];
        }

        //계산
        table.BeginLoadData();
        table.EndLoadData();

        return float.Parse(row["Result"].ToString());
    }

    //태그가 있는 경우 Data에서 가져옴
    //태그가 없는 경우 인스펙터창의 것을 적용
    public void Init() {
        currentHP  = baseHP;
        currentATK = baseATK;
        currentDEF = baseDEF;
        currentSPD = baseSPD;

        SetDefaultHP();
    }

    // Start is called before the first frame update
    void Awake()
    {
        equipSkills = GetComponent<EquipSkills>();
    }

    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateStat();
    }

    //Temp
    public void SetDefaultHP() {
        CalculateStat();

        currentHP = maxHP;
    }

    public void TakeDamage(float damage) {
        currentHP -= damage;
    }
}
