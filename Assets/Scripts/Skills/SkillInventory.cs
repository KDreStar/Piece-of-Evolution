using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//수량 없이 ㄱ 
public class SkillInventory : MonoBehaviour
{
    public SkillSlot[] skillSlot = new SkillSlot[20];
    public List<Skill> skillList;

    public int limit = 20;
    public int page = 1;
    public int maxPage = 1;

    public TextMeshProUGUI textPage;

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
        int k = (page - 1) * limit;

        for (int i=0; i<limit; i++)
            skillList[i + k] = GetSkill(i);
    }

    //이전 버튼 누르면
    public void Prev() {
        if (page <= 1)
            return;
        
        page--;
        UpdateSlots();
    }

    //다음 버튼 누르면
    public void Next() {
        if (page > maxPage)
            return;
        
        page++;
        UpdateSlots();
    }
    /*
    public void RemoveSkill(Skill skill) {
        int index = skillSlot.IndexOf(skill);

        if (index >= 0)
            RemoveSkill(index);
    }
    */
    public void UpdateSlots() {
        if (skillList == null) {
            Debug.Log("Null");
            return;
        }

        maxPage = (skillList.Count - 1) / limit + 1;
        maxPage = maxPage < 1 ? 1 : maxPage;

        if (page > maxPage)
            page = maxPage;

        for (int i=0; i<limit; i++) {
            int k = (page - 1) * limit + i;


            if (k >= skillList.Count) {
                skillSlot[i].skill = null;
            } else {
                skillSlot[i].skill = skillList[k];
            }
        }

        textPage.text = "" + page + "/" + maxPage;
    }

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        skillSlot = GetComponentsInChildren<SkillSlot>();
        skillList = Managers.Data.skillInventoryData.GetSkillList();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
    }
}
