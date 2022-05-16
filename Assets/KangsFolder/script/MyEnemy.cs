using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemy : MonoBehaviour
{
    public int hp;

    void Start()
    {
        hp = 20;
    }
    // Enter 들어갔을때, Stay안에있을때, Exit벗어났을때
    void OnTriggerStay2D(Collider2D other)
    //void OnTriggerEnter2D(Collider2D other)
    //여기서 OnTriggerStay2D로 변경하면 있는 내내 맞음.
    {
        Move q = other.GetComponent<Move>();

        if (q != null && q.animator.GetCurrentAnimatorStateInfo(0).IsName("attack_new") && q.attacked)
        {
            hp = hp - q.atkDmg;
            Debug.Log(hp);
            q.attacked = false;
            return;
        } //

    }
}

