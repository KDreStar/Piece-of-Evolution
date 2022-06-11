using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using UnityEngine.UI;
using System.IO;

//Scene 전환시 적의 정보를 저장하는 용도
public class EnemyData : MonoBehaviour
{
    private static EnemyData instance = null;
    public static EnemyData Instance {
        get { return instance; }
    }

    public Skill[] skillList = new Skill[8];
    public NNModel model;

    public string name;
    public float baseHP;
    public float baseATK;
    public float baseDEF;
    public float baseSPD;

    public Sprite sprite;

    public string filePath;

    //싱글톤
    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        if (BattleManager.Instance.isLearning)
            LoadWithLearning();
    }

    public void Save() {
        CharacterJSONData data = new CharacterJSONData();

        data.name = name;
        data.baseHP = baseHP;
        data.baseATK = baseATK;
        data.baseDEF = baseDEF;
        data.baseSPD = baseSPD;
        data.skillNoList = new int[EquipSkills.maxSlot];

        for (int i=0; i<EquipSkills.maxSlot; i++)
            data.skillNoList[i] = skillList[i] != null ? skillList[i].No : 0;
        
        string json = JsonUtility.ToJson(data);

        string fileName = "EnemyJSONData";
        filePath = Application.persistentDataPath + "/" + fileName + ".json";

        File.WriteAllText(filePath, json);
    }

    public void Load() {
        string json = File.ReadAllText(filePath);
        CharacterJSONData data = JsonUtility.FromJson<CharacterJSONData>(json);

        name = data.name;
        baseHP = data.baseHP;
        baseATK = data.baseATK;
        baseDEF = data.baseDEF;
        baseSPD = data.baseSPD;

        for (int i=0; i<EquipSkills.maxSlot; i++) {
            int no = data.skillNoList[i];

            if (no != 0)
                skillList[i] = SkillDatabase.Instance.GetSkill(no);
        }
    }

    public void LoadWithLearning() {
        string json = File.ReadAllText("EnemyJSONData.json");
        CharacterJSONData data = JsonUtility.FromJson<CharacterJSONData>(json);

        name = data.name;
        baseHP = data.baseHP;
        baseATK = data.baseATK;
        baseDEF = data.baseDEF;
        baseSPD = data.baseSPD;

        for (int i=0; i<EquipSkills.maxSlot; i++) {
            int no = data.skillNoList[i];

            if (no != 0)
                skillList[i] = SkillDatabase.Instance.GetSkill(no);
        }
    }

    public void Set(GameObject character) {
        Status status = character.GetComponent<Status>();
        Image image = character.GetComponent<Image>();
        EquipSkills equipSkills = character.GetComponent<EquipSkills>();

        name = status.name;
        baseHP = status.baseHP;
        baseATK = status.baseATK;
        baseDEF = status.baseDEF;
        baseSPD = status.baseSPD;

        sprite = image.sprite;

        /*
        for (int i=0; i<EquipSkills.maxSlot; i++) {
            Skill skill = equipSkills.GetSkill(i);
            skillList[i] = skill;
        }
        */
        skillList = equipSkills.GetSkillList();
    }

    public void SetSkill(int i, int no) {
        skillList[i] = SkillDatabase.Instance.GetSkill(no);
    }

    public void SetSkill(int i, Skill skill) {
        skillList[i] = skill;
    }

    public Skill GetSkill(int i) {
        return skillList[i];
    }
}
