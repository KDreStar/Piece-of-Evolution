using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ActiveSkill")]
public class ActiveSkill : Skill
{
    //기본 쿨타임
    [SerializeField]
    private float baseCooltime;
    public float BaseCooltime {
        get { return baseCooltime; }
    }

    //데미지 공식
    [SerializeField]
    private string damageFormula;
    public string DamageFormula {
        get { return damageFormula; }
    }

    //이펙트
    [SerializeField]
    private GameObject effect;
    public GameObject Effect {
        get { return effect; }
    }

    //스킬을 사용하면 프리펩을 생성하면 됨
    //프리펩에는 이펙트랑 스크립트를 가지고 있음
    //각 스킬 마다 구현하면 될 듯? ㅁ?ㄹ
    public bool Use(GameObject attacker, int direction=3)
    {
        //currentCooltime = activeSkillData.BaseCooltime;
        //바라보는 방향으로 스킬 생성
        // 1 = ↑ //
        Vector3 angle = new Vector3(0, 0, 135 - direction * 45);

        GameObject clone = Instantiate(effect, attacker.transform.position, effect.transform.rotation * Quaternion.Euler(angle));
        clone.tag = attacker.tag + "Skill";
        clone.transform.parent = attacker.transform.parent.transform;

        //StartCoroutine(ApplyCooltime());

        Debug.Log(name + "사용");
        Debug.Log(damageFormula);

        return true;
    }

    /*
    public void ResetCooltime() {
        currentCooltime = 0;
    }

    IEnumerator ApplyCooltime() {
        while (currentCooltime > 0) {
            currentCooltime -= Time.deltaTime;

            yield return null;
        }

        ResetCooltime();
        Debug.Log("재사용 가능");
    }
    */
}
