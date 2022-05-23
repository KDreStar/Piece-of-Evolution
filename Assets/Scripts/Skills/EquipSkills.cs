using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 오브젝트 슬롯에 스킬이 붙어있음
public class EquipSkills : MonoBehaviour
{
    public const int maxSlot = 8;
    public GameObject[] slot = new GameObject[maxSlot];

    //최대 스킬 코스트 (인스펙터에서 설정) 기본 20
    [SerializeField]
    private int maxSkillCost = 20;

    private int currentSkillCost = 0;

    //스킬 장착
    public bool EquipSkill(int i, Skill skill) {
        //maxCost 넘는지 확인
        if (currentSkillCost + skill.SkillCost > maxSkillCost)
            return false;

        SkillSlot skillSlot = GetSkillSlot(i);

        //빈 슬롯이면 추가
        if (skillSlot.IsEmpty()) {
            AddSkill(i, skill);
        } else {
            //슬롯이 이미 있으면 스킬인벤토리와 스왑 (Destroy)
            //SkillInventory.Instance.AddSkill(skillSlot.skill);
            AddSkill(i, skill);
        }

        currentSkillCost += skill.SkillCost;
        
        return true;
    }

    //스킬슬롯 내부에서 위치 변경
    public void SwapSkill(int i, int j) {
        SkillSlot skillSlotI = slot[i].GetComponent<SkillSlot>();
        SkillSlot skillSlotJ = slot[j].GetComponent<SkillSlot>();

        Skill skillI = skillSlotI.skill;
        Skill skillJ = skillSlotJ.skill;
        
        skillSlotI.skill = skillJ;
        skillSlotJ.skill = skillI;
    }

    public SkillSlot GetSkillSlot(int i) {
        return slot[i].GetComponent<SkillSlot>();
    }

    //스킬 추가만
    public void AddSkill(int i, Skill skill) {
        GetSkillSlot(i).skill = skill;
    }

    public bool UseSkill(int i) {
        return GetSkillSlot(i).UseSkill(gameObject);
    }

    public Skill GetSkill(int i) {
        return GetSkillSlot(i).skill;
    }

    public ActiveSkill GetActiveSkill(int i) {
        return GetSkillSlot(i).GetActiveSkill();
    }

    public PassiveSkill GetPassiveSkill(int i) {
        return GetSkillSlot(i).GetPassiveSkill();
    }


    // Start is called before the first frame update
    void Start()
    {
        //Temp
        //나중에 UI로 수정하세요
        GameObject equipSkills = new GameObject();
        equipSkills.name = "EquipSkills";

        for (int i=0; i<maxSlot; i++) {
            slot[i] = new GameObject();

            slot[i].name = "Slot" + (i + 1);
            slot[i].transform.parent = equipSkills.transform;
            slot[i].AddComponent<SkillSlot>();
        }

        //Test
        AddSkill(0, SkillDatabase.Instance.GetSkill(13));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
