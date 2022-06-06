using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//수량 없이 ㄱ 
public class SkillInventory : MonoBehaviour
{
    private static SkillInventory instance = null;
    public static SkillInventory Instance {
        get { return instance; }
    }

    public List<SkillSlot> skillSlot;

    private int page = 1;
    public int Page {
        get { return page; }
    }

    public SkillSlot GetSkillSlot(int i) {
        return skillSlot[i];
    }

    public void AddSkill(int i, Skill skill) {
        skillSlot[i].AddSkill(skill);
    }

    public void AddSkill(Skill skill) {
        for (int i=0; i<skillSlot.Count; i++) {
            Skill temp = GetSkill(i);

            if (temp == null) {
                AddSkill(i, skill);
                return;
            }
        }

        skillSlot.Add(CreateSkillSlot(skillSlot.Count));
        AddSkill(skillSlot.Count - 1, skill);
    }

    public Skill GetSkill(int i) {
        if (i >= skillSlot.Count)
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

    /*
    public void RemoveSkill(Skill skill) {
        int index = skillSlot.IndexOf(skill);

        if (index >= 0)
            RemoveSkill(index);
    }
    */

    public SkillSlot CreateSkillSlot(int i) {
        GameObject skill = new GameObject();

        skill.transform.parent = gameObject.transform;
        skill.name = "Slot" + (i + 1);
        return skill.AddComponent<SkillSlot>();
    }
    
    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        skillSlot = new List<SkillSlot>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
