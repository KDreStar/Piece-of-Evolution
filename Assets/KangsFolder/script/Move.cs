using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer a;
    //컴포넌트들이 다 클래스임
    //선언을 해주면 안에있는 속성들을 전부 사용 가능하다!
    // 
    public int atkDmg;
    public bool attacked = false;

    int avoid_moveSpeed = 200;
    private Rigidbody2D m_body2d;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); // 부착된 오브젝트의 Animator의 데이터를 받아온다.
        m_body2d = GetComponent<Rigidbody2D>();//

        atkDmg = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //transform.localScale = new Vector3(-1, 1, 1); // localScale은 현재 오브젝트의 크기를 조절함
            //animator.SetBool("moving", true);
            a.flipX = true;
            if (Input.GetKeyDown(KeyCode.W) && !animator.GetCurrentAnimatorStateInfo(0).IsName("avoid_new"))
            {
                animator.SetTrigger("avoid_new");
                transform.Translate(Vector3.right * Time.deltaTime * avoid_moveSpeed);
            }
            else
            {
                transform.Translate(Vector3.right * Time.deltaTime * 2); // Vector3.right, left: new Vector3(1, 0, 0), (-1, 0, 0)과 동일함.
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow)) //Input.GetKey : 키를 꾹 누르고 있을 때 true 반환
        {
            a.flipX = false;
            //transform.localScale = new Vector3(1, 1, 1); // localScale은 현재 오브젝트의 크기를 조절함
            //animator.SetBool("moving", true);
            if (Input.GetKeyDown(KeyCode.W) && !animator.GetCurrentAnimatorStateInfo(0).IsName("avoid_new"))
            {
                animator.SetTrigger("avoid_new");
                transform.Translate(Vector3.left * Time.deltaTime * avoid_moveSpeed);
            }
            else//
            {
                transform.Translate(Vector3.left * Time.deltaTime * 2); // Vector3.right, left: new Vector3(1, 0, 0), (-1, 0, 0)과 동일함.
            }
        }
        if (Input.GetKey(KeyCode.UpArrow)) //Input.GetKey : 키를 꾹 누르고 있을 때 true 반환
        {
            if (Input.GetKeyDown(KeyCode.W) && !animator.GetCurrentAnimatorStateInfo(0).IsName("avoid_new"))
            {
                animator.SetTrigger("avoid_new");
                transform.Translate(Vector3.up * Time.deltaTime * avoid_moveSpeed);
            }
            else
            {
                transform.Translate(Vector3.up * Time.deltaTime * 2); // Vector3.right, left: new Vector3(1, 0, 0), (-1, 0, 0)과 동일함.
            }
        }
        if (Input.GetKey(KeyCode.DownArrow)) //Input.GetKey : 키를 꾹 누르고 있을 때 true 반환
        {
            if (Input.GetKeyDown(KeyCode.W) && !animator.GetCurrentAnimatorStateInfo(0).IsName("avoid_new"))
            {
                animator.SetTrigger("avoid_new");
                transform.Translate(Vector3.down * Time.deltaTime * avoid_moveSpeed);
            }
            else
            {
                transform.Translate(Vector3.down * Time.deltaTime * 2); // Vector3.right, left: new Vector3(1, 0, 0), (-1, 0, 0)과 동일함.
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack_new"))
        {
            animator.SetTrigger("attack_new");
            //Debug.Log("어택true");
            attacked = true;

        }     
        
    }
}
