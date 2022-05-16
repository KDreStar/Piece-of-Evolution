using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrow : SkillEffect
{
    //스킬 이펙트 관련 스크립터블 오브젝트

    //테스트로 일단 세팅
    float speed = 10.0f;

    // Update is called once per frame
    public override void Update()
    {
        Vector2 vector = new Vector2(1, 0);
        transform.Translate(vector * speed * Time.deltaTime);
    }

    //여기서 스킬 데미지 계산 처리
    public override void CalculateDamage() {
        Debug.Log("데미지 체크");
    }
}
