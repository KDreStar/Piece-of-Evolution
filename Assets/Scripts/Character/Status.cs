using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        /*
        HP = (ATK + 50) * 2
        HP
        (ATK+50)*2
        */ 
        for (int i=0; i<skill.Effects.Length; i++) {
            Effect effect = skill.Effects[i];

            float result = Managers.Battle.CalculateFormula(effect.Formula, this);

            ApplyEffect(effect, result);
        }
    }

    public void ApplyEffect(Effect effect, float num) {
        switch (effect.EffectTag) {
            case EffectTag.ATTACKER_MHP:
                Operate(ref maxHP, effect.EffectOperator, num);
                break;

            case EffectTag.ATTACKER_CHP:
                Operate(ref currentHP, effect.EffectOperator, num);
                break;

            case EffectTag.ATTACKER_ATK:
                Operate(ref currentATK, effect.EffectOperator, num);
                break;

            case EffectTag.ATTACKER_DEF:
                Operate(ref currentDEF, effect.EffectOperator, num);
                break;

            case EffectTag.ATTACKER_SPD:
                Operate(ref currentSPD, effect.EffectOperator, num);
                break;
        }
    }

    private void Operate(ref float stat, EffectOperator op, float num) {
        switch (op) {
            case EffectOperator.SET:
                stat = num;
                break;

            case EffectOperator.ADD:
                stat += num;
                break;

            case EffectOperator.SUB:
                stat -= num;
                break;

            case EffectOperator.MUL:
                stat *= num;
                break;

            case EffectOperator.DIV:
                stat /= num;
                break;
        }
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
