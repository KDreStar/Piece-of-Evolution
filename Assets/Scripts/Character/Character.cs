using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    /*캐릭터 정보들*/
    string name; //이름

    int chp; //현재 체력 currentHP
    int mhp; //최대 체력 maxHP
    int bhp; //기본 체력 baseHP

    int atk; //공격력 ATK
    int batk; //기본 공격력 baseATK

    int def; //방어력 DEF
    int bdef; //기본 방어력 baseDEF

    int spd; //스피드 SPD
    int bspd; //기본 스피드 baseSPD

    /*
    8개로 한 다음 1~6은 커스텀 스킬 7은 기본 공격 8은 회피
    기본 공격과 회피도 쿨타임을 적용시키기 위함
    */
    //Skill skill; //보유 스킬 

    int maxSkillCost;



    //현재 눌린 키를 이용하여 이동하는 방식
    void move() {

    }

    //
    void useSkill(int n) {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
