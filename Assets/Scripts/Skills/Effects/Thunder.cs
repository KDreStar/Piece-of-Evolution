using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : SkillEffect
{

    public override void Initialize() {
        base.Initialize();
    }

    public override IEnumerator Hitting() {
        while (true) {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                break;

            yield return null;
        }

        EnableCollider();
        if (anim.GetCurrentAnimatorStateInfo(0).length > 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) {
                currentDuration = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                Debug.Log("지속시간" + currentDuration);

                yield return null;
            }
        }

        Debug.Log("지속시간" + currentDuration);
        currentDuration = 1;
        DestroySkillEffect();
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }
}