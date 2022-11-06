using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//액티브 스킬 기본 정보
//모든 스킬에 있을만한 정보만
[CreateAssetMenu(menuName = "ActiveSkill")]
public class ActiveSkill : Skill
{
    //기본 쿨타임
    [SerializeField]
    private float cooltime;
    public float Cooltime {
        get { return cooltime; }
    }

    //생성 위치 (기본 0, 0)
    [SerializeField]
    private Vector2 createOffset;
    public Vector2 CreateOffset {
        get { return createOffset; }
    }

    //사정거리 (투사체)
    //안날라가는 경우 = 0으로 설정
    [SerializeField]
    private float range;
    public float Range {
        get { return range; }
    }

    //스킬속도 (투사체 or 애니메이션 속도)
    //기본 1
    [SerializeField]
    private float speed = 1;
    public float Speed {
        get { return speed; }
    }

    //이펙트 프리팹
    [SerializeField]
    private GameObject prefab;
    public GameObject Prefab {
        get { return prefab; }
    }
}
