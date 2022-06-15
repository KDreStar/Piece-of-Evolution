using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using Unity.Barracuda.ONNX;
using Unity.MLAgents.Policies;
using System.IO;
using UnityEngine.UI;

//Scene 전환시 캐릭터의 정보를 저장하는 용도
public class CharacterData : MonoBehaviour
{
    private static CharacterData instance = null;
    public static CharacterData Instance {
        get { return instance; }
    }

    [SerializeField]
    private Skill[] skillList = new Skill[8];

    public NNModel model;

    public string name;
    public float baseHP;
    public float baseATK;
    public float baseDEF;
    public float baseSPD;
    public BehaviorType type;

    public Sprite sprite;

    private string dataFilePath;
    private string modelFilePath;

    //싱글톤
    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        dataFilePath = Application.persistentDataPath + "/CharacterJSONData.json";
        modelFilePath = Application.persistentDataPath + "/CharacterModel.onnx";
    }

    void Start() {
        baseHP = 500;
        baseATK = 50;
        baseDEF = 40;
        baseSPD = 10;
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

        File.WriteAllText(dataFilePath, json);
    }

    public void Load() {
        string json = File.ReadAllText(dataFilePath);
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

        type = data.type;

        ONNXModelConverter converter = new ONNXModelConverter(true);
        Model temp = converter.Convert(modelFilePath);

        model = ScriptableObject.CreateInstance<NNModel>();
        NNModelData modelData = ScriptableObject.CreateInstance<NNModelData>();

        
        using (var memoryStream = new MemoryStream())
        using (var writer = new BinaryWriter(memoryStream))
        {
            ModelWriter.Save(writer, temp);
            modelData.Value = memoryStream.ToArray();
        }
        
        modelData.name = "Data";
        modelData.hideFlags = HideFlags.HideInHierarchy;

        model.modelData = modelData;
        model.name = "Model";
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
    
    public void UpdateSkills(EquipSkills equipSkills) {
        for (int i=0; i<EquipSkills.maxSlot; i++) {
            Debug.Log(i);

            SkillSlot skillSlot = equipSkills.GetSkillSlot(i);

            skillList[i] = skillSlot.GetSkill();
        }
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
