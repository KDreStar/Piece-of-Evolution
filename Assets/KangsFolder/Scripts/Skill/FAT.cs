using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAT : SkillEffect
{

    // Update is called once per frame
    public override void Update()
    {
        Vector2 vector = new Vector2(1, 0);
        transform.Translate(vector * activeSkill.BaseSpeed * Time.deltaTime);
    }
}
