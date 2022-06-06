using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 오브젝트 슬롯에 스킬이 붙어있음
public class EquipSkills : MonoBehaviour
{
    public const int maxSlot = 8;
    public GameObject equipSkills;
    public SkillSlot[] skillSlot = new SkillSlot[maxSlot];

    //최대 스킬 코스트 (인스펙터에서 설정) 기본 20
    [SerializeField]
    private int maxSkillCost = 20;

    private int currentSkillCost = 0;
    public int CurrentSkillCost {
        get { return currentSkillCost; }
    }


    public bool EquipSkill(int i, SkillSlot newSlot) {
        if (checkSkillCost(i, newSlot.skill) == false)
            return false;

        SkillManager.Instance.SwapSkillSlot(skillSlot[i], newSlot);
        CalculateCost();

        return true;
    }

    private bool checkSkillCost(int i, Skill skill) {
        if (skillSlot[i].IsEmpty()) {
            //cost 확인
            if (currentSkillCost + skill.SkillCost > maxSkillCost)
                return false;

        } else {
            Skill oldSkill = GetSkill(i);

            //기존 스킬을 빼고 장착하는 것
            if (currentSkillCost + skill.SkillCost - oldSkill.SkillCost > maxSkillCost)
                return false;
        }

        return true;
    }

    public void RemoveSkill(int i) {
        skillSlot[i].RemoveSkill();
    }

    public SkillSlot GetSkillSlot(int i) {
        return skillSlot[i];
    }

    public void CalculateCost() {
        currentSkillCost = 0;

        for (int i=0; i<maxSlot; i++) {
            Skill skill = GetSkill(i);

            if (skill != null)
                currentSkillCost += skill.SkillCost;
        }
    }

    //스킬 추가만
    public void AddSkill(int i, Skill skill) {
        skillSlot[i].AddSkill(skill);
    }

    public bool UseSkill(int i){
        return skillSlot[i].UseSkill(gameObject);
    }

    public bool UseSkill(int i, int direction) {
        return skillSlot[i].UseSkill(gameObject, direction);
    }

    public Skill GetSkill(int i) {
        return skillSlot[i].skill;
    }

    public ActiveSkill GetActiveSkill(int i) {
        return skillSlot[i].GetActiveSkill();
    }

    public PassiveSkill GetPassiveSkill(int i) {
        return skillSlot[i].GetPassiveSkill();
    }

    void Start()
    {
        skillSlot = equipSkills.GetComponentsInChildren<SkillSlot>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateCost();
    }
}
