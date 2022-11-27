using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//수량 없이 ㄱ 
public class SkillInventory : MonoBehaviour
{
    public SkillSlot[] skillSlots = new SkillSlot[20];
    public List<Skill> skills;

    public int limit = 20;
    public int page = 1;
    public int maxPage = 1;

    public TextMeshProUGUI textPage;

    public SkillSlot GetSkillSlot(int i) {
        return skillSlots[i];
    }

    public void AddSkill(int i, Skill skill) {
        skillSlots[i].AddSkill(skill);
    }

    public Skill GetSkill(int i) {
        if (i >= skills.Count)
            return null;

        return skillSlots[i].skill;
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
        skillSlots[i].skill = null;
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
        if (skills == null) {
            Debug.Log("Null");
            return;
        }

        maxPage = (skills.Count - 1) / limit + 1;
        maxPage = maxPage < 1 ? 1 : maxPage;

        if (page > maxPage)
            page = maxPage;

        for (int i=0; i<limit; i++) {
            int k = (page - 1) * limit + i;


            if (k >= skills.Count) {
                skillSlots[i].skill = null;
            } else {
                skillSlots[i].skill = skills[k];
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
        skillSlots = GetComponentsInChildren<SkillSlot>();
        LoadData();
    }

    public void LoadData() {
        SkillInventoryData data = Managers.Data.gameData.skillInventoryData;

        skills.Clear();
        for (int i=0; i<data.skillNos.Count; i++) {
            int no = data.skillNos[i];
            skills.Add(Managers.DB.SkillDB.GetSkill(no));
        }
    }

    //GameData에 있는 skillNo들을 업데이트
    //
    public void UpdateGameData() {
        SkillInventoryData data = Managers.Data.gameData.skillInventoryData;

        int left = (page - 1) * limit;
        int right = page * limit;

        for (int i=0; i<limit; i++) {
            Skill skill = GetSkill(i);
            int k = (page - 1) * limit + i;

            skills[k] = skill;
        }

        data.skillNos.Clear();
        for (int i=0; i<skills.Count; i++) {
            Skill skill = skills[i];

            int no = skill == null ? 0 : skill.No;

            data.skillNos.Add(no);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
    }
}
