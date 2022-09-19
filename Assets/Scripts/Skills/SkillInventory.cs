using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//수량 없이 ㄱ 
public class SkillInventory : MonoBehaviour
{
    public List<SkillSlot> skillSlot;
    public GameObject skillSlotPrefab;

    public List<Skill> skillList;

    public int pageMaxSlot = 20;
    public int page = 1;
    public int maxPage = 5;

    public SkillSlot GetSkillSlot(int i) {
        return skillSlot[i];
    }

    public void AddSkill(int i, Skill skill) {
        skillSlot[i].AddSkill(skill);
    }

    public Skill GetSkill(int i) {
        if (i >= skillList.Count)
            return null;

        return skillSlot[i].skill;
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

    public void RemoveSkill(int i) {
        skillSlot[i].skill = null;
    }

    public void UpdateSkillList() {
        int k = (page - 1) * pageMaxSlot;

        for (int i=0; i<pageMaxSlot; i++)
            skillList[i + k] = GetSkill(i);
    }
    /*
    public void RemoveSkill(Skill skill) {
        int index = skillSlot.IndexOf(skill);

        if (index >= 0)
            RemoveSkill(index);
    }
    */

    public void CreateSkillSlot(int i) {
        GameObject skill = Instantiate(skillSlotPrefab);

        skill.transform.parent = transform;
        skill.name = "Slot" + (i + 1);

        SkillSlot sl = skill.GetComponent<SkillSlot>();
        sl.dragable = true;
        sl.tooltipPivot = new Vector2(1, 1);
        sl.transform.localScale = new Vector3(1, 1, 1);

        skillSlot.Add(sl);
    }
    
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        skillSlot = new List<SkillSlot>();
        skillList = Managers.Data.skillInventoryData.GetSkillList();

        int k = (page - 1) * pageMaxSlot; 

        for (int i=0; i<pageMaxSlot; i++) {
            CreateSkillSlot(i);
            AddSkill(i, skillList[i + k]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
