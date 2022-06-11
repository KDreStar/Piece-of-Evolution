using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//수량 없이 ㄱ 
public class SkillInventoryData : MonoBehaviour
{
    private static SkillInventoryData instance = null;
    public static SkillInventoryData Instance {
        get { return instance; }
    }

    public int Count = 20;
    public List<Skill> skillList;
    
    //싱글톤
    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        skillList = new List<Skill>();

        for (int i=0; i<Count; i++) {
            skillList.Add(null);
        }
    }

    public Skill GetSkill(int i) {
        return skillList[i];
    }

    public void AddSkill(Skill skill) {
        for (int i=0; i<Count; i++) {
            if (skillList[i] == null) {
                skillList[i] = skill;
                return;
            }
        }

        Count++;
        skillList.Add(skill);
    }

    public void UpdateSkills(SkillInventory inv) {
        for (int i=0; i<Count; i++) {
            Debug.Log(i);

            SkillSlot skillSlot = inv.GetSkillSlot(i);

            skillList[i] = skillSlot.GetSkill();
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
}
