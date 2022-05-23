using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터 조작 관련
public class CharacterController : MonoBehaviour
{
    EquipSkills equipSkills;
    Status status;

    // Start is called before the first frame update
    void Start()
    {
        equipSkills = GetComponent<EquipSkills>();
        status = GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMove();
        CheckUseSkill();
    }

    void CheckMove() {
        float dx = Input.GetAxisRaw("Horizontal");
        float dy = Input.GetAxisRaw("Vertical");

        Vector2 vector = new Vector2(dx, dy);
        transform.Translate(vector * status.CurrentSPD * 0.3f * Time.deltaTime);
    }

    void CheckUseSkill() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            equipSkills.UseSkill(0);
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            equipSkills.UseSkill(1);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            equipSkills.UseSkill(2);
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            equipSkills.UseSkill(3);
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            equipSkills.UseSkill(4);
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            equipSkills.UseSkill(5);
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            equipSkills.UseSkill(6);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            equipSkills.UseSkill(7);
        }
    }
}
