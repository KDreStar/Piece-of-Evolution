using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//액티브 스킬 기본 정보
[CreateAssetMenu(menuName = "ActiveSkill")]
public class ActiveSkill : Skill
{
    //기본 쿨타임
    [SerializeField]
    private float cooltime;
    public float Cooltime {
        get { return cooltime; }
    }

    //데미지 공식
    [SerializeField]
    private string damage;
    public string Damage {
        get { return damage; }
    }

    //스킬 속도
    [SerializeField]
    private float speed;
    public float Speed {
        get { return speed; }
    }

    //사정거리
    [SerializeField]
    private float range;
    public float Range {
        get { return range; }
    }

    //이펙트 프리팹
    [SerializeField]
    private GameObject effect;
    public GameObject Effect {
        get { return effect; }
    }
}
