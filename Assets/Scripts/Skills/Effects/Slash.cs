using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : SkillEffect
{
    //스킬 이펙트 관련 스크립터블 오브젝트

    //테스트로 일단 세팅
    float time = 1.0f;
    float speed = 80.0f;

    // Update is called once per frame
    public override void Update()
    {
        Vector3 vector = new Vector3(0, 0, 1);
        transform.Rotate(vector * speed * Time.deltaTime);
    }
}
