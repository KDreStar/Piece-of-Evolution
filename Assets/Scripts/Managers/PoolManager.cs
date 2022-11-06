using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스킬 이펙트를 미리 생성하여 빌려주는 방식
//메모리 효율을 높임 
public class PoolManager
{
    public int count = 0;
    //<스킬번호, 스킬이펙트 큐>로 저장
    private Dictionary<int, Queue<SkillEffect>> skillEffectTable = new Dictionary<int, Queue<SkillEffect>>();

    //큐에 스킬 이펙트가 없으면 생성
    private SkillEffect CreateSkillEffect(GameObject prefab) {
        SkillEffect skillEffect = Managers.Instantiate(prefab).GetComponent<SkillEffect>();

        skillEffect.gameObject.SetActive(false);
        skillEffect.transform.parent = Managers.Instance.transform;

        Debug.Log("Count: " + count++);

        return skillEffect;
    }

    //큐에 있는 스킬 이펙트를 빌려줌
    public SkillEffect GetSkillEffect(GameObject prefab) {
        int no = prefab.GetComponent<SkillEffect>().GetSkillNo();

        //Queue에 생성된게 없으면
        if (skillEffectTable.ContainsKey(no) == false)
            skillEffectTable.Add(no, new Queue<SkillEffect>());

        SkillEffect skillEffect;

        if (skillEffectTable[no].Count == 0) {
            skillEffect = CreateSkillEffect(prefab);
        } else {
            skillEffect = skillEffectTable[no].Dequeue();
        }

        skillEffect.gameObject.SetActive(true);
        Debug.Log("Active:true");
        return skillEffect;
    }

    //스킬 이펙트를 반환함
    public void ReturnSkillEffect(SkillEffect skillEffect) {
        int no = skillEffect.GetSkillNo();

        skillEffect.CancelInvoke();
        skillEffect.gameObject.SetActive(false);
        Debug.Log("사라짐");
        skillEffect.transform.parent = Managers.Instance.transform;
        skillEffectTable[no].Enqueue(skillEffect);
    }
}
