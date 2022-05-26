using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLaser : SkillEffect
{
    //스킬 이펙트 관련 스크립터블 오브젝트

    //테스트로 일단 세팅
    float speed = 5.0f;

    public override void Start() {
        base.Start();

        Debug.Log("Destroy");
        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    public override void Update()
    {

    }

    //여기서 스킬 데미지 계산 처리
    public override void CalculateDamage() {
        Debug.Log("데미지 체크");

        float damage = 250;//attackerStatus.CurrentATK  - defenderStatus.CurrentDEF;
        defenderStatus.TakeDamage(damage);
    }
}
