using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using Unity.Barracuda.ONNX;
using Unity.MLAgents.Policies;
using System.IO;
using UnityEngine.UI;
using System;

//캐릭터 정보 (적 포함)
//세이브 로드시 로드의 경우 씬이 바뀔때마다 로드를 하면 파일 오버헤드가 발생할 것이므로
//로드는 게임 시작시에만 함
//메모리에 로드
//메모리에 저장 로드는 Set Get이용
public class CharacterData
{
    public Skill[] skillList = new Skill[8];
    public NNModel model;

    public string tag;
    public string name;
    public float baseHP;
    public float baseATK;
    public float baseDEF;
    public float baseSPD;
    public BehaviorType type;

    public Sprite sprite;

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

    public void Get(GameObject character) {
        Status status = character.GetComponent<Status>();
        Image image = character.GetComponent<Image>();
        EquipSkills equipSkills = character.GetComponent<EquipSkills>();
        BehaviorParameters bp = character.GetComponent<BehaviorParameters>();

        status.name = name;
        status.baseHP = baseHP;
        status.baseATK = baseATK;
        status.baseDEF = baseDEF;
        status.baseSPD = baseSPD;

        //image.sprite = sprite;

        /*
        for (int i=0; i<EquipSkills.maxSlot; i++) {
            Skill skill = equipSkills.GetSkill(i);
            skillList[i] = skill;
        }
        */
        equipSkills.skillList = skillList;

        Debug.Log(skillList);

        if (bp != null && model != null) {
            bp.Model = model;
            bp.BehaviorType = type;
        }
    }

    public CharacterJSONData CreateJSONData() {
        CharacterJSONData data = new CharacterJSONData();

        data.name = name;
        data.baseHP = baseHP;
        data.baseATK = baseATK;
        data.baseDEF = baseDEF;
        data.baseSPD = baseSPD;
        data.skillNoList = new int[EquipSkills.maxSlot];

        for (int i=0; i<EquipSkills.maxSlot; i++)
            data.skillNoList[i] = skillList[i] != null ? skillList[i].No : 0;
        
        return data;
    }

    public void SetJSONData(CharacterJSONData data) {
        name = data.name;
        baseHP = data.baseHP;
        baseATK = data.baseATK;
        baseDEF = data.baseDEF;
        baseSPD = data.baseSPD;

        Debug.Log(data.name);

        for (int i=0; i<EquipSkills.maxSlot; i++) {
            int no = data.skillNoList[i];

            if (no != 0)
                skillList[i] = SkillDatabase.Instance.GetSkill(no);
        }
    }

    public void LoadModel(string modelName) {
        string path = Application.persistentDataPath + "/" + modelName;

        if (!File.Exists(path))
            return;

        ONNXModelConverter converter = new ONNXModelConverter(true);
        Model temp = converter.Convert(path);

        model = ScriptableObject.CreateInstance<NNModel>();
        NNModelData modelData = ScriptableObject.CreateInstance<NNModelData>();

        using (var memoryStream = new MemoryStream())
        using (var writer = new BinaryWriter(memoryStream))
        {
            ModelWriter.Save(writer, temp);
            modelData.Value = memoryStream.ToArray();
        }
        
        modelData.name = "Data" + modelName;
        modelData.hideFlags = HideFlags.HideInHierarchy;

        model.modelData = modelData;
        model.name = "Model" + modelName;

        Debug.Log("모델" + model.name);
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

    public Skill[] GetSkillList() {
        return skillList;
    }
}
