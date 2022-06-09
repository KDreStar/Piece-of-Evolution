using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

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

    public Sprite sprite;

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
        baseHP = 500;
        baseATK = 50;
        baseDEF = 40;
        baseSPD = 10;
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
