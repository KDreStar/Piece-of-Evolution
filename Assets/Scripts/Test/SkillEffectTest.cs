using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectTest : MonoBehaviour
{
    public ActiveSkill activeSkill;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(activeSkill);

        SkillEffect se = Managers.Pool.GetSkillEffectInfo(activeSkill.Prefab);

        Debug.Log(se.GetCreatePos());
        Debug.Log(se.GetColliderRange());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
