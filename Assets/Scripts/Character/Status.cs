using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Policies;

public class Status : MonoBehaviour
{
    public string name;

    [Space (10f)]
    public float baseHP;
    public float baseATK;
    public float baseDEF;
    public float baseSPD;

    [Space (10f)]
    [SerializeField]
    float maxHP;
    public float MaxHP {
        get { return maxHP; }
    }

    [SerializeField]
    float currentHP;
    public float CurrentHP {
        get { return currentHP; }
    }

    [SerializeField]
    float currentATK;
    public float CurrentATK {
        get { return currentATK; }
    }
    
    [SerializeField]
    float currentDEF; //방어력 DEF
    public float CurrentDEF {
        get { return currentDEF; }
    }
    
    [SerializeField]
    float currentSPD; //스피드 SPD
    public float CurrentSPD {
        get { return currentSPD; }
    }

    //현재 패시브
    [Space (10f)]
    public Buffs passives;

    //현재 버프
    [Space (10f)]
    public Buffs buffs;


    private EquipSkills equipSkills;

    public void SetDefaultStat() {
        maxHP = baseHP;
        currentATK = baseATK;
        currentDEF = baseDEF;
        currentSPD = baseSPD;
    }

    void CalculateStat() {
        SetDefaultStat();
        
        SetPassives();
        
        ApplyPassives();
        ApplyBuffs();
    }

    enum Stat {HP, ATK, DEF, SPD}

    //패시브 세팅
    public void SetPassives() {
        passives.Reset();

        for (int i=0; i<EquipSkills.MaxSlot; i++) {
            PassiveSkill passiveSkill = equipSkills.GetPassiveSkill(i);

            if (passiveSkill == null)
                continue;

            foreach (var effect in passiveSkill.Effects) {
                passives.AddPassive(effect);
            }
        }
    }

    //패시브 적용
    public void ApplyPassives() {
        List<Buff> buffList = passives.buffs;

        if (buffList == null)
            return;

        for (int i=0; i<buffList.Count; i++) {
            Effect effect = buffList[i].effect;

            float num = Managers.Battle.CalculateFormula(effect.Formula, this);

            ApplyEffect(effect, num);
        }
    }

    //버프 적용
    public void ApplyBuffs() {
        List<Buff> buffList = buffs.buffs;

        if (buffList == null)
            return;

        for (int i=0; i<buffList.Count; i++) {
            Effect effect = buffList[i].effect;

            float num = Managers.Battle.CalculateFormula(effect.Formula, this);

            ApplyEffect(effect, num);
        }
    }

    //버프 추가
    public void AddBuff(Effect effect, float duration, bool infinity=false) {
        buffs.AddBuff(effect, duration, infinity);
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
