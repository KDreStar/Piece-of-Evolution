using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 오브젝트 슬롯에 스킬이 붙어있음
public class SkillSlot : MonoBehaviour
{
    public const int maxSlot = 8;
    GameObject[] slot = new GameObject[maxSlot];

    //최대 스킬 코스트 (인스펙터에서 설정) 기본 20
    [SerializeField]
    private int maxSkillCost = 20;

    private int currentSkillCost = 0;

    //스킬 장착
    public bool EquipSkill(int i, Skill skill) {
        //maxCost 넘는지 확인
        if (currentSkillCost + skill.skillData.SkillCost > maxSkillCost)
            return false;

        //빈 슬롯이면 추가
        if (GetSkill(i) == null) {
            AddSkill(i, skill);
        } else {
            //슬롯이 이미 있으면 스킬인벤토리와 스왑 (Destroy)
            SkillInventory.Instance.TransferSkill(GetSkill(i));
            AddSkill(i, skill);
        }

        currentSkillCost += skill.skillData.SkillCost;
        
        return true;
    }

    //스킬슬롯 내부에서 위치 변경
    public void SwapSkill(int i, int j) {
        GameObject slot1 = slot[i];
        GameObject slot2 = slot[j];

        slot[i] = slot2;
        slot[j] = slot1;
    }

    //스킬 추가후 인벤토리 스킬 삭제
    public void AddSkill(int i, Skill skill) {
        if (skill is ActiveSkill) {
            ActiveSkill activeSkill = skill as ActiveSkill;
            ActiveSkill temp = slot[i].AddComponent<ActiveSkill>();

            temp.SetData(activeSkill.activeSkillData);
        } else {
            PassiveSkill passiveSkill = skill as PassiveSkill;
            PassiveSkill temp = slot[i].AddComponent<PassiveSkill>();

            temp.SetData(passiveSkill.passiveSkillData);
        }

        //Destroy(skill);
    }

    public void UseSkill(int i) {
        ActiveSkill skill = GetActiveSkill(i);

        if (skill == null)
            return;

        skill.Use();
    }

    public Skill GetSkill(int i) {
        return slot[i].GetComponent<Skill>();
    }

    public ActiveSkill GetActiveSkill(int i) {
        Skill skill = GetSkill(i);

        if (skill is not ActiveSkill)
            return null;

        return skill as ActiveSkill;
    }

    public PassiveSkill GetPassiveSkill(int i) {
        Skill skill = GetSkill(i);

        if (skill is not PassiveSkill)
            return null;

        return skill as PassiveSkill;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject skillSlot = new GameObject();
        skillSlot.name = "SkillSlot";

        for (int i=0; i<maxSlot; i++) {
            GameObject skill = new GameObject();
            skill.name = "Slot" + (i + 1);
            skill.transform.parent = skillSlot.transform;
            slot[i] = skill;
        }

        //Test
        AddSkill(0, SkillDatabase.Instance.GetSkill(12));

        UseSkill(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
