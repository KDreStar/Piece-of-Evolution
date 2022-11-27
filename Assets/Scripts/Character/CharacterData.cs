using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;
using System.IO;
using Unity.MLAgents.Policies;
using Unity.Barracuda;
using Unity.Barracuda.ONNX;
using UnityEngine.UI;

public enum PathType {
    Resources,
    Local,
    URL
}

[Serializable]
public class CharacterData
{
    public int no;

    public string name;
    public float baseHP;
    public float baseATK;
    public float baseDEF;
    public float baseSPD;

    public int[] skillNos = new int[8];

    public BehaviorType behaviorType;

    public string aiName;
    public string modelPath;
    public PathType modelPathType;
    
    public string spritePath;
    public PathType spritePathType;

    [NonSerialized]
    public NNModel model;

    [NonSerialized]
    public Sprite sprite;

    public void SetData(Monster monster) {
        name = monster.Name;
        baseHP = monster.BaseHP;
        baseATK = monster.BaseATK;
        baseDEF = monster.BaseDEF;
        baseSPD = monster.BaseSPD;

        sprite = monster.Sprite;
        spritePath = "Monsters/Images/" + monster.FileName;
        spritePathType = PathType.Resources;

        aiName = monster.FileName;

        if (monster.IsOnnx == true) {
            modelPath = "Monsters/Models/" + monster.FileName;
            modelPathType = PathType.Resources;
            behaviorType = BehaviorType.InferenceOnly;
        } else {
            behaviorType = BehaviorType.HeuristicOnly;
        }

        for (int i=0; i<monster.Skills.Length; i++) {
            Skill skill = monster.Skills[i];
            skillNos[i] = skill == null ? 0 : skill.No;
        }
    }

    public void SetData(Character character) {
        Status status = character.status;
        Image image = character.image;
        EquipSkills equipSkills = character.equipSkills;
        SpriteRenderer sr = character.sr;

        name = status.name;
        baseHP = status.baseHP;
        baseATK = status.baseATK;
        baseDEF = status.baseDEF;
        baseSPD = status.baseSPD;

        if (sr != null)
            sprite = sr.sprite;

        if (image != null)
            sprite = image.sprite;

        for (int i=0; i<EquipSkills.MaxSlot; i++) {
            Skill skill = equipSkills.GetSkill(i);

            if (skill != null)
                skillNos[i] = skill.No;
        }
    }

    public void LoadSprite() {
        if (sprite != null)
            return;

        switch (spritePathType) {
            case PathType.Resources:
                sprite = Resources.Load<Sprite>(spritePath);
                break;
        }
    }

    public void LoadModel() {
        if (modelPath == "" || modelPath == null)
            return;
        
        string path;

        if (modelPathType == PathType.Local)
            path = Application.persistentDataPath + "/" + modelPath;
        else
            path = "";

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
        
        modelData.name = "Data" + aiName;
        modelData.hideFlags = HideFlags.HideInHierarchy;

        model.modelData = modelData;
        model.name = "Model" + aiName;

        Debug.Log("모델" + model.name);
    }
}
