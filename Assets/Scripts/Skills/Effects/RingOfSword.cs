using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingOfSword : SkillEffect
{

    public override void Initialize() {
        base.Initialize();

        //캐릭터 중심이 계속 기준
        gameObject.transform.parent = attacker.transform;
    }

    public override IEnumerator Hitting() {
        float currentTime = 0;
        float destroyTime = 1.5f;

        while (currentTime < destroyTime) {
            currentTime += Time.deltaTime;
            currentDuration = currentTime / destroyTime;

            yield return null;
        }
        
        currentDuration = 1;
        DestroySkillEffect();
    }
    // Update is called once per frame
    public override void Update()
    {

    }
}