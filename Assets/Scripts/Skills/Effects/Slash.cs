using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : SkillEffect
{
    //스킬 이펙트 관련 스크립터블 오브젝트

    //테스트로 일단 세팅
    float time = 1.0f;
    float speed = 80.0f;

    public override void Initialize() {
        base.Initialize();

        //캐릭터 중심이 계속 기준
        gameObject.transform.parent = attacker.transform;
    }

    // Update is called once per frame
    public override void Update()
    {

    }
}
