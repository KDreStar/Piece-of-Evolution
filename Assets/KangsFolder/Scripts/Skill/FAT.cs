using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAT : SkillEffect
{
    float speed = 1;
    float time = 1;
    // Update is called once per frame
    public override void Update()
    {
        Vector2 vector = new Vector2(1, 0);
        transform.Translate(vector * speed * Time.deltaTime);
    }
}
