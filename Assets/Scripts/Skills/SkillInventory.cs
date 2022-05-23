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

    public List<GameObject> slot;

    private int page = 1;
    public int Page {
        get { return page; }
    }
 
    /*
    public SkillSlot GetSkillSlot(int i) {
        return slot[i].GetComponent<SkillSlot>();
    }

    public Skill GetSkill(int i) {
        if (i >= slot.Count)
            return null;

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

    public void RemoveSkill(int i) {
        Skill skill = GetSkill(i);

        Destroy(skill);
    }


    public void RemoveSkill(Skill skill) {
        for (int i=0; i<slot.Count; i++) {
            if (GetSkill(i).skillData == skill.skillData) {
                RemoveSkill(i);
                return;
            }    
        }
    }

    public void AddSkill(Skill skill) {
        for (int i=0; i<slot.Count; i++) {
            Skill temp = GetSkill(i);

            if (temp == null) {
                AddSkill(i, skill);
                return;
            }
        }

        slot.Add(CreateSkillObject(slot.Count));
        AddSkill(slot.Count - 1, skill);
    }

    public GameObject CreateSkillObject(int i) {
        GameObject skill = new GameObject();

        skill.transform.parent = gameObject.transform;
        skill.name = "Slot" + (i + 1);

        return skill;
    }

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
    }

    //스킬 이동시
    public void MoveSkill(Skill skill) {
        AddSkill(skill);
        Destroy(skill);
    }

    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
