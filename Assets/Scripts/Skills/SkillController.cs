using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillController : MonoBehaviour
{
    private static SkillController instance = null;
    public static SkillController Instance {
        get { return instance; }
    }

    public SkillInventory skillInventory;
    public EquipSkills equipSkills;
    private SkillSlot dragSkillSlot;
    private SkillSlot dropSkillSlot;

    /*
    장착스킬 -> 장착스킬
    장착스킬 -> 인벤토리
    인벤토리 -> 장착스킬
    인벤토리 -> 인벤토리 체크
    */
    public void MoveSkill(SkillSlot dropSkillSlot) {
        dragSkillSlot = DragSkillSlot.Instance.skillSlot;
        this.dropSkillSlot = dropSkillSlot;
        //skillInventory = SkillInventory.Instance;

        Debug.Log("이동");
        //? -> 장착스킬
        //장착스킬 -> ?
        if (dragSkillSlot.isEquipSkill || dropSkillSlot.isEquipSkill) {
            //장착스킬 -> 장착스킬
            if (dragSkillSlot.isEquipSkill && dropSkillSlot.isEquipSkill) {
                SwapSkillSlot();
                return;
            }

            //장착스킬 -> 인벤토리
            //dropSkill을 장착해야함
            //인벤토리가 비어있으면 장착 안함
            if (dragSkillSlot.isEquipSkill) {
                if (dropSkillSlot.skill == null)
                    SwapSkillSlot();
                else if (equipSkills.CheckCost(dragSkillSlot.equipIndex, dropSkillSlot.skill))
                    SwapSkillSlot();
                return;
            }

            //인벤토리 -> 장착스킬
            //dragSkill을 장착해야함
            if (dropSkillSlot.isEquipSkill) {
                if (equipSkills.CheckCost(dragSkillSlot.equipIndex, dragSkillSlot.skill))
                    SwapSkillSlot();
                return;
            }
        }

        //인벤토리 -> 인벤토리
        SwapSkillSlot();
    }

    //스킬 스왑
    //스킬 스왑후 데이터매니저에 저장
    public void SwapSkillSlot(SkillSlot oldSlot = null, SkillSlot newSlot = null) {
        if (oldSlot == null)
            oldSlot = dragSkillSlot;
        
        if (newSlot == null)
            newSlot = dropSkillSlot;

        Skill temp = oldSlot.skill;

        oldSlot.AddSkill(newSlot.skill);
        newSlot.AddSkill(temp);

        dropSkillSlot = null;

        //캐릭터 스킬, 스킬 인벤토리 세이브
        //각각 Data의 SkillList를 참조하고 있으므로 우선 업데이트

        skillInventory.UpdateSkillList();
        equipSkills.UpdateSkillList();

        Managers.Data.SaveGameData();
        //SkillInventoryData.Instance.UpdateSkills(SkillInventory.Instance);
    }

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //currentSkillCost.text = equipSkills.CurrentSkillCost.ToString();
    }
}
