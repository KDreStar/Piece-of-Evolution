using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 오브젝트 슬롯에 스킬이 붙어있음
public class EquipSkills : MonoBehaviour
{
    public const int maxSlot = 8;
    public GameObject equipSkills;
    private SkillSlot[] skillSlot = new SkillSlot[maxSlot];

    //인스펙터 적용
    public Skill[] skillList = new Skill[maxSlot];

    //최대 스킬 코스트 (인스펙터에서 설정) 기본 20
    [SerializeField]
    private int maxCost = 20;

    private int currentCost = 0;
    public int CurrentCost {
        get { return currentCost; }
    }

    //가능시 true 불가능시 false
    public bool CheckCost(int i, Skill skill) {
        if (skillSlot[i].IsEmpty()) {
            //cost 확인
            if (currentCost + skill.Cost > maxCost)
                return false;

        } else {
            Skill oldSkill = GetSkill(i);

            //기존 스킬을 빼고 장착하는 것
            if (currentCost + skill.Cost - oldSkill.Cost > maxCost)
                return false;
        }

        return true;
    }

    //스킬 제거
    public void RemoveSkill(int i) {
        skillSlot[i].RemoveSkill();
    }

    //i번째 스킬 슬롯 얻기
    public SkillSlot GetSkillSlot(int i) {
        return skillSlot[i];
    }

    //코스트 적용
    public void CalculateCost() {
        currentCost = 0;

        for (int i=0; i<maxSlot; i++) {
            Skill skill = GetSkill(i);

            if (skill != null)
                currentCost += skill.Cost;
        }
    }

    //스킬 추가만
    public void AddSkill(int i, Skill skill) {
        Debug.Log("스킬1" + skill);

        skillSlot[i].AddSkill(skill);
    }


    public bool UseSkill(int i, int skillX, int skillY) {
        return skillSlot[i].UseSkill(gameObject, skillX, skillY);
    }

    public Skill GetSkill(int i) {
        return skillSlot[i].skill;
    }

    public Skill[] GetSkillList() {
        return skillList;
    }

    public ActiveSkill GetActiveSkill(int i) {
        return skillSlot[i].GetActiveSkill();
    }

    public PassiveSkill GetPassiveSkill(int i) {
        return skillSlot[i].GetPassiveSkill();
    }

    public void UpdateSkillList() {
        for (int i=0; i<maxSlot; i++)
            skillList[i] = GetSkill(i);
    }

    public void Init() {
        for (int i=0; i<maxSlot; i++) {
            Skill skill = skillList[i];

            if (skill == null) {
                RemoveSkill(i);
                CalculateCost();
                continue;
            }


            Debug.Log("스킬 Init" + CheckCost(i, skill));

            if (CheckCost(i, skill))
                AddSkill(i, skill);
        }
    }

    //이 오브젝트 밑에 달린 스킬 슬롯들을 가져옴
    //그 후 마우스 오버시 나오는 툴팁의 시작위치를 적용
    void Awake() {
        skillSlot = equipSkills.GetComponentsInChildren<SkillSlot>();
    }

    //Character.cs 에서 다 불러옴
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CalculateCost();
    }
}
