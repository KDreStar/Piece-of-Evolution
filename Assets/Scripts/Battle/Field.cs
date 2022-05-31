using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public List<SkillEffect> skillEffects;    

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
        for (int i=0; i<skillEffects.Count; i++) {
            skillEffects[i].DestroySkillEffect();
        }

        skillEffects.Clear();
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
