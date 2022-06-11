using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillManager : MonoBehaviour
{
    private static SkillManager instance = null;
    public static SkillManager Instance {
        get { return instance; }
    }

    private SkillInventory skillInventory;
    private EquipSkills equipSkills;
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

        if (equipSkills == null)
            equipSkills = GameObject.FindWithTag("Character").GetComponent<EquipSkills>();
        //skillInventory = SkillInventory.Instance;

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
            if (dragSkillSlot.isEquipSkill) {
                //장착하려는 스킬(인벤토리)이 없는 경우
                if (dropSkillSlot.skill == null)
                    SwapSkillSlot();
                else
                    equipSkills.EquipSkill(dragSkillSlot.equipIndex, dropSkillSlot);
                return;
            }

            //인벤토리 -> 장착스킬
            //dragSkill을 장착해야함
            if (dropSkillSlot.isEquipSkill) {
                equipSkills.EquipSkill(dropSkillSlot.equipIndex, dragSkillSlot);
                return;
            }
        }

        //인벤토리 -> 인벤토리
        SwapSkillSlot();
    }

    public void SwapSkillSlot(SkillSlot oldSlot = null, SkillSlot newSlot = null) {
        if (oldSlot == null)
            oldSlot = dragSkillSlot;
        
        if (newSlot == null)
            newSlot = dropSkillSlot;

        Skill temp = oldSlot.skill;

        oldSlot.AddSkill(newSlot.skill);
        newSlot.AddSkill(temp);

        dropSkillSlot = null;

        CharacterData.Instance.UpdateSkills(equipSkills);
        SkillInventoryData.Instance.UpdateSkills(SkillInventory.Instance);
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
