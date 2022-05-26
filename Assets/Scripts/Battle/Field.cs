using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public List<SkillEffect> skillEffects;    

    public void Add(SkillEffect skillEffect) {
        skillEffects.Add(skillEffect);
    }

    public List<SkillEffect> GetList() {
        for (int i=0; i<skillEffects.Count; i++) {
            if (skillEffects[i] == null)
                skillEffects.RemoveAt(i);
        }

        skillEffects.TrimExcess();

        return skillEffects;
    }

    public void ClearEffects() {
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
