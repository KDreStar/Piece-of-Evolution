using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rush : SkillEffect
{
    float currentTime = 0;
    float destroyTime = 0;

    public override void Initialize(GameObject caster, int skillX, int skillY) {
        base.Initialize(caster, skillX, skillY);

        attacker.isStopping = true;
        DisableCollider();
        currentTime = 0;
        destroyTime = activeSkill.Range / activeSkill.Speed;
    }

    public override IEnumerator Active() {
        EnableCollider();

        while (currentTime < destroyTime) {
            currentTime += Time.deltaTime;
            currentDuration = currentTime / destroyTime;

            yield return null;
        }
        
        currentDuration = 1;
        attacker.isStopping = false;
        DestroySkillEffect();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {

        Vector3 vector = GetVelocity();

        if (currentTime > 0)
            attacker.rigid.MovePosition(attacker.transform.position + vector * activeSkill.Speed * Time.deltaTime);
        transform.position = attacker.transform.position;
    }
}
