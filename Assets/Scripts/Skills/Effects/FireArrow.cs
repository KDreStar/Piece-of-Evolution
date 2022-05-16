using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrow : SkillEffect
{
    //스킬 이펙트 관련 스크립터블 오브젝트

    //테스트로 일단 세팅
    float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Create Fire Arrow");
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vector = new Vector2(1, 0);
        transform.Translate(vector * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col) {
        //즉 스킬이 시전자가 아닌 캐릭터에게 부딛칠때;
        if (col.CompareTag(caster.tag) == false) {
            Debug.Log("데미지 입음");
        }
    }
}
