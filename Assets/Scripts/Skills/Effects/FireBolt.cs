using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : SkillEffect
{
    public override IEnumerator Active() {
        float currentTime = 0;
        float destroyTime = activeSkill.Range / activeSkill.Speed;

        while (currentTime < destroyTime) {
            currentTime += Time.deltaTime;
            currentDuration = currentTime / destroyTime;

            yield return null;
        }
        
        currentDuration = 1;
        DestroySkillEffect();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        Vector2 vector = new Vector2(1, 0);

        transform.Translate(vector * activeSkill.Speed * Time.deltaTime);
    }
}
