using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 오브젝트 슬롯에 스킬이 붙어있음
public class EquipSkills : MonoBehaviour
{
    public const int MaxSlot = 8;
    public GameObject equipSkillsObject;
    private SkillSlot[] skillSlots = new SkillSlot[MaxSlot];

    //인스펙터 적용
    public Skill[] skills = new Skill[MaxSlot];

    //최대 스킬 코스트 (인스펙터에서 설정) 기본 20
    [SerializeField]
    private int maxCost = 20;

    private int currentCost = 0;
    public int CurrentCost {
        get { return currentCost; }
    }

    //가능시 true 불가능시 false
    public bool CheckCost(int i, Skill skill) {
        if (skillSlots[i].IsEmpty()) {
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
        skillSlots[i].RemoveSkill();
    }

    //i번째 스킬 슬롯 얻기
    public SkillSlot GetSkillSlot(int i) {
        return skillSlots[i];
    }

    //코스트 적용
    public void CalculateCost() {
        currentCost = 0;

        for (int i=0; i<MaxSlot; i++) {
            Skill skill = GetSkill(i);

            if (skill != null)
                currentCost += skill.Cost;
        }
    }

    //스킬 추가만
    public void AddSkill(int i, Skill skill) {
        skillSlots[i].AddSkill(skill);
    }
    
    public bool UseSkill(int i, int direction) {
        return skillSlots[i].UseSkill(gameObject, direction);
    }

    public Skill GetSkill(int i) {
        return skillSlots[i].skill;
    }

    public Skill[] GetSkills() {
        return skills;
    }

    public ActiveSkill GetActiveSkill(int i) {
        return skillSlots[i].GetActiveSkill();
    }

    public PassiveSkill GetPassiveSkill(int i) {
        return skillSlots[i].GetPassiveSkill();
    }

    public void Randomize() {
        List<int> indexs = new List<int>();
        int[] randomIndexs = new int[MaxSlot];

        for (int i=0; i<MaxSlot; i++)
            indexs.Add(i);

        for (int i=0; i<MaxSlot; i++) {
            int k = Random.Range(0, indexs.Count);

            k = indexs[k];
            randomIndexs[k] = i;
            indexs.Remove(k);
        }

        

        for (int i=0; i<MaxSlot; i++) {
            
            int k = randomIndexs[i];
            Debug.Log("[Random] " + k);
            Skill skill = skills[k];

            if (skill == null)
                RemoveSkill(i);
            else
                AddSkill(i, skill);
        }
    }

    public void LoadSkills(int[] skillNos) {
        for (int i=0; i<MaxSlot; i++) {
            Skill skill = Managers.DB.SkillDB.GetSkill(skillNos[i]);

            if (skill == null)
                skills[i] = null;
            else
                skills[i] = skill;
        }
    }

    //GameData에 있는 skillNo들을 업데이트
    public void UpdateGameData() {
        CharacterData characterData = Managers.Data.GetCurrentCharacterData();

        for (int i=0; i<MaxSlot; i++) {
            Skill skill = GetSkill(i);
            skills[i] = skill;

            int no = skill == null ? 0 : skill.No;

            characterData.skillNos[i] = no;
        }
    }

    public void Init() {
        for (int i=0; i<MaxSlot; i++) {
            Skill skill = skills[i];

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
        skillSlots = equipSkillsObject.GetComponentsInChildren<SkillSlot>();
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
