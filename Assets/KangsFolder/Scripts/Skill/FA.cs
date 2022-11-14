using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FA : SkillEffect
{
    float speed = 1;
    // Update is called once per frame
    public override void FixedUpdate()
    {
        Vector2 vector = new Vector2(1, 0);
        transform.Translate(vector * speed * Time.deltaTime);
    }
}
