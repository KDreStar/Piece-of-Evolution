using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : SkillEffect
{
    //스킬 이펙트 관련 스크립터블 오브젝트

    //테스트로 일단 세팅
    float time = 1.0f;
    float speed;

    public override void Start() {
        base.Start();

        speed = 80.0f / time; 

        Debug.Log("Destroy");
        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    public override void Update()
    {
        Vector3 vector = new Vector3(0, 0, 1);
        transform.Rotate(vector * speed * Time.deltaTime);
    }

    //여기서 스킬 데미지 계산 처리
    public override void CalculateDamage() {
        Debug.Log("데미지 체크");

        float damage = 250;//attackerStatus.CurrentATK  - defenderStatus.CurrentDEF;
        defenderStatus.TakeDamage(damage);
    }
}
