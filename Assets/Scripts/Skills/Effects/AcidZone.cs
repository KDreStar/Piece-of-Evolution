using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidZone : SkillEffect
{
    public override void Initialize(GameObject caster, int direction) {
        base.Initialize(caster, direction);

        DisableCollider();
    }

    public override IEnumerator Active() {
        float currentTime = 0;
        float destroyTime = 2.5f;

        EnableCollider();

        while (currentTime < destroyTime) {
            currentTime += Time.deltaTime;
            currentDuration = currentTime / destroyTime;

            yield return wait;
        }
        
        currentDuration = 1;
        DestroySkillEffect();
    }
    
    public override void FixedUpdate()
    {
        gameObject.transform.position = attacker.transform.position;
    }
}