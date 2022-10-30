using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public List<SkillEffect> skillEffects;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject upWall;
    public GameObject downWall;

    public void Add(SkillEffect skillEffect) {
        skillEffects.Add(skillEffect);
    }

    public void Remove(SkillEffect skillEffect) {
        skillEffects.Remove(skillEffect);
    }

    public List<SkillEffect> GetList() {
        return skillEffects;
    }

    public void ClearEffects() {
        Debug.Log("전체 사라짐" + skillEffects.Count);
        while (skillEffects.Count > 0) { 
            skillEffects[0].DestroySkillEffect();
        }
    }

    void Awake() {
        skillEffects = new List<SkillEffect>();
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
