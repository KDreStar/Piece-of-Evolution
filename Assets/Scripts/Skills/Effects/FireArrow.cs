using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrow : MonoBehaviour
{
    //스킬 이펙트 관련 스크립터블 오브젝트

    //테스트로 일단 세팅
    float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Create Fire Arrow");
        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vector = new Vector2(1, 0);
        transform.Translate(vector * speed * Time.deltaTime);
    }
}
