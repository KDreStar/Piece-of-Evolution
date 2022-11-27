using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingOfSword : SkillEffect
{
    public override IEnumerator Active() {
        float currentTime = 0;
        float destroyTime = 1.5f;

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