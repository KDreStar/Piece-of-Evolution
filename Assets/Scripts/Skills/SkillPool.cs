using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPool : MonoBehaviour
{
    private static SkillPool instance = null;
    public static SkillPool Instance {
        get { return instance; }
    }

    public int count = 0;

    private Dictionary<int, Queue<SkillEffect>> skillEffectTable = new Dictionary<int, Queue<SkillEffect>>();

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private SkillEffect CreateSkillEffect(GameObject skillPrefab) {
        SkillEffect skillEffect = Instantiate(skillPrefab).GetComponent<SkillEffect>();

        skillEffect.gameObject.SetActive(false);
        skillEffect.transform.parent = this.transform;

        Debug.Log("Count: " + count++);

        return skillEffect;
    }

    public SkillEffect GetSkillEffect(GameObject skillPrefab) {
        int no = skillPrefab.GetComponent<SkillEffect>().GetSkillNo();

        //Queue에 생성된게 없으면
        if (skillEffectTable.ContainsKey(no) == false)
            skillEffectTable.Add(no, new Queue<SkillEffect>());

        SkillEffect skillEffect;

        if (skillEffectTable[no].Count == 0) {
            skillEffect = CreateSkillEffect(skillPrefab);
        } else {
            skillEffect = skillEffectTable[no].Dequeue();
        }

        skillEffect.gameObject.SetActive(true);
        Debug.Log("Active:true");
        return skillEffect;
    }

    public void ReturnSkillEffect(SkillEffect skillEffect) {
        int no = skillEffect.GetSkillNo();

        skillEffect.CancelInvoke();
        skillEffect.gameObject.SetActive(false);
        skillEffect.transform.parent = this.transform;
        skillEffectTable[no].Enqueue(skillEffect);
    }
}
