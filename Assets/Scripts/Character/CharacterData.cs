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
public enum PathType {
    Resources,
    Local,
    URL
}

public class CharacterData
{
    public Skill[] skillList = new Skill[8];
    public NNModel model;
    public EnemyAI enemyAI;
    
    public string name;
    public float baseHP;
    public float baseATK;
    public float baseDEF;
    public float baseSPD;
    public BehaviorType behaviorType;

    public string aiName;
    public string modelPath;
    public PathType modelPathType;
    
    public string spritePath;
    public PathType spritePathType;

    public Sprite sprite;

    public void Save(Monster monster) {
        name = monster.Name;
        baseHP = monster.BaseHP;
        baseATK = monster.BaseATK;
        baseDEF = monster.BaseDEF;
        baseSPD = monster.BaseSPD;

        sprite = monster.Sprite;
        spritePath = "Monsters/Images/" + monster.FileName;
        spritePathType = PathType.Resources;

        aiName = monster.FileName + "AI";
        if (monster.IsOnnx == true) {
            modelPath = "Monsters/Models/" + monster.FileName;
            modelPathType = PathType.Resources;
        } else {
            behaviorType = BehaviorType.HeuristicOnly;
        }

        for (int i=0; i<monster.SkillList.Length; i++)
            skillList[i] = monster.SkillList[i];
    }

    //Deep Copy
    public void Save(CharacterData data) {
        name = data.name;
        baseHP = data.baseHP;
        baseATK = data.baseATK;
        baseDEF = data.baseDEF;
        baseSPD = data.baseSPD;

        sprite = data.sprite;
        spritePath = data.spritePath;
        spritePathType = data.spritePathType;

        aiName = data.aiName;
        modelPath = data.modelPath;
        modelPathType = data.modelPathType;

        behaviorType = data.behaviorType;

        for (int i=0; i<data.skillList.Length; i++)
            skillList[i] = data.skillList[i];
    }

    public void Save(GameObject character) {
        Status status = character.GetComponent<Status>();
        Image image = character.GetComponent<Image>();
        EquipSkills equipSkills = character.GetComponent<EquipSkills>();

        name = status.name;
        baseHP = status.baseHP;
        baseATK = status.baseATK;
        baseDEF = status.baseDEF;
        baseSPD = status.baseSPD;

        //sprite = image.sprite;

        /*
        for (int i=0; i<EquipSkills.maxSlot; i++) {
            Skill skill = equipSkills.GetSkill(i);
            skillList[i] = skill;
        }
        */
        skillList = equipSkills.GetSkills();
    }

    public void Load(GameObject character) {
        Status status = character.GetComponent<Status>();
        Image image = character.GetComponent<Character>().image;
        EquipSkills equipSkills = character.GetComponent<EquipSkills>();
        BehaviorParameters bp = character.GetComponent<BehaviorParameters>();
        BattleAgent agent = character.GetComponent<BattleAgent>();
        SpriteRenderer sr = character.GetComponent<SpriteRenderer>();

        status.name = name;
        status.baseHP = baseHP;
        status.baseATK = baseATK;
        status.baseDEF = baseDEF;
        status.baseSPD = baseSPD;

        LoadSprite();
        if (image != null && sprite != null)
            image.sprite = sprite;

        if (sr != null && sprite != null)
            sr.sprite = sprite;
        //image.sprite = sprite;

        /*
        for (int i=0; i<EquipSkills.maxSlot; i++) {
            Skill skill = equipSkills.GetSkill(i);
            skillList[i] = skill;
        }
        */
        equipSkills.skills = skillList;

        if (bp != null && model != null) {
            bp.Model = model;
            bp.BehaviorType = behaviorType;
        }

        if (bp != null && model == null) {
            agent.ai = Managers.Data.AIFactory.Create(aiName);
            bp.BehaviorType = behaviorType;
        }
    }

    public CharacterJSONData CreateJSONData() {
        CharacterJSONData data = new CharacterJSONData();

        data.name = name;
        data.baseHP = baseHP;
        data.baseATK = baseATK;
        data.baseDEF = baseDEF;
        data.baseSPD = baseSPD;
        data.skillNoList = new int[EquipSkills.MaxSlot];

        for (int i=0; i<EquipSkills.MaxSlot; i++)
            data.skillNoList[i] = skillList[i] != null ? skillList[i].No : 0;
        
        data.behaviorType = behaviorType;

        data.aiName = aiName;
        data.modelPath = modelPath;
        data.modelPathType = modelPathType;

        data.spritePath = spritePath;
        data.spritePathType = spritePathType;

        return data;
    }

    public void SetJSONData(CharacterJSONData data) {
        name = data.name;
        baseHP = data.baseHP;
        baseATK = data.baseATK;
        baseDEF = data.baseDEF;
        baseSPD = data.baseSPD;

        for (int i=0; i<EquipSkills.MaxSlot; i++) {
            int no = data.skillNoList[i];

            if (no != 0)
                skillList[i] = Managers.DB.SkillDB.GetSkill(no);
        }

        behaviorType = data.behaviorType;

        aiName = data.aiName;
        modelPath = data.modelPath;
        modelPathType = data.modelPathType;

        spritePath = data.spritePath;
        spritePathType = data.spritePathType;
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

    public void SetSkill(int i, int no) {
        skillList[i] = Managers.DB.SkillDB.GetSkill(no);
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
