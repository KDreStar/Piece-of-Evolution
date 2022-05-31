using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 오브젝트 슬롯에 스킬이 붙어있음
public class EquipSkills : MonoBehaviour
{
    public const int maxSlot = 8;
    private GameObject[] slot = new GameObject[maxSlot];
    public SkillSlot[] skillSlot = new SkillSlot[maxSlot];

    //최대 스킬 코스트 (인스펙터에서 설정) 기본 20
    [SerializeField]
    private int maxSkillCost = 20;

    private int currentSkillCost = 0;

    //스킬 장착
    public bool EquipSkill(int i, Skill skill) {
        //maxCost 넘는지 확인
        if (currentSkillCost + skill.SkillCost > maxSkillCost)
            return false;

        //빈 슬롯이면 추가
        if (skillSlot[i].IsEmpty()) {
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
        Skill skillI = skillSlot[i].skill;
        Skill skillJ = skillSlot[j].skill;
        
        skillSlot[i].skill = skillJ;
        skillSlot[j].skill = skillI;
    }

    public SkillSlot GetSkillSlot(int i) {
        return skillSlot[i];
    }

    //스킬 추가만
    public void AddSkill(int i, Skill skill) {
        skillSlot[i].skill = skill;
    }

    public bool UseSkill(int i){
        return skillSlot[i].UseSkill(gameObject);
    }

    public bool UseSkill(int i, int direction) {
        return skillSlot[i].UseSkill(gameObject, direction);
    }

    public Skill GetSkill(int i) {
        return skillSlot[i].skill;
    }

    public ActiveSkill GetActiveSkill(int i) {
        return skillSlot[i].GetActiveSkill();
    }

    public PassiveSkill GetPassiveSkill(int i) {
        return skillSlot[i].GetPassiveSkill();
    }


    // Start is called before the first frame update

    void Awake() {
        GameObject equipSkills = new GameObject();
        equipSkills.name = "EquipSkills";

        skillSlot = new SkillSlot[maxSlot];

        for (int i=0; i<maxSlot; i++) {
            slot[i] = new GameObject();

            slot[i].name = "Slot" + (i + 1);
            slot[i].transform.parent = equipSkills.transform;
            skillSlot[i] = slot[i].AddComponent<SkillSlot>();

            Debug.Log(skillSlot[i]);
        }
    }

    void Start()
    {
        //Temp
        //나중에 UI로 수정하세요
        /*
        GameObject equipSkills = new GameObject();
        equipSkills.name = "EquipSkills";

        skillSlot = new SkillSlot[maxSlot];

        for (int i=0; i<maxSlot; i++) {
            slot[i] = new GameObject();

            slot[i].name = "Slot" + (i + 1);
            slot[i].transform.parent = equipSkills.transform;
            skillSlot[i] = slot[i].AddComponent<SkillSlot>();

            Debug.Log(skillSlot[i]);
        }
        */
        if (gameObject.tag == "Character") {
            AddSkill(0, SkillDatabase.Instance.GetSkill(13));
            AddSkill(1, SkillDatabase.Instance.GetSkill(14));
            AddSkill(2, SkillDatabase.Instance.GetSkill(15));
        } else {
			//AddSkill(0, SkillDatabase.Instance.GetSkill(13));
			//AddSkill(1, SkillDatabase.Instance.GetSkill(14));
			//AddSkill(2, SkillDatabase.Instance.GetSkill(15));
			AddSkill(0, SkillDatabase.Instance.GetSkill(16));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
