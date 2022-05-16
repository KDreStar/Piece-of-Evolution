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
    // Enter ������, Stay�ȿ�������, Exit�������
    void OnTriggerStay2D(Collider2D other)
    //void OnTriggerEnter2D(Collider2D other)
    //���⼭ OnTriggerStay2D�� �����ϸ� �ִ� ���� ����.
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

