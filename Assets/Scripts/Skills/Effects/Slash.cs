using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : SkillEffect
{
    public override void Initialize(GameObject caster, int skillX, int skillY) {
        base.Initialize(caster, skillX, skillY);

        DisableCollider();
    }


    public override IEnumerator Active() {
        EnableCollider();

        while (true) {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Active"))
                break;

            yield return null;
        }

        EnableCollider();
        if (anim.GetCurrentAnimatorStateInfo(0).length > 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("Active")) {
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
    public override void FixedUpdate()
    {
        gameObject.transform.position = attacker.transform.position;
    }
}
