using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class SlimeAI : EnemyAI
{

    
    public SlimeAI() {

    }

    /** 행동 방식
    이동 = 적에게 무조건 다가감
                a  (-x, -y)
    d 차이가 0.5일때까지


    공격1 = 산성 방울
    - 적에게 가장 가까운 각도로 발사함
    
    공격2 = 산성 지대
    - 범위안에 들어온 경우 시전

    */
    public override void Judge(BattleAgent agent, in ActionBuffers actionsOut) {
        var discreteActionsOut = actionsOut.DiscreteActions;

        float attackerX = agent.attacker.transform.localPosition.x;
        float attackerY = agent.attacker.transform.localPosition.y;

        float defenderX = agent.defender.transform.localPosition.x;
        float defenderY = agent.defender.transform.localPosition.y;

        EquipSkills equipSkills = agent.attacker.equipSkills;
        SkillSlot   acidBubbleSlot = equipSkills.GetSkillSlot(0);
        ActiveSkill acidBubble = acidBubbleSlot.GetActiveSkill();
        SkillEffect acidBubbleEffect = acidBubble.Prefab.GetComponent<SkillEffect>();

        SkillSlot   acidZoneSlot = equipSkills.GetSkillSlot(1);
        ActiveSkill acidZone = acidBubbleSlot.GetActiveSkill();
        SkillEffect acidZoneEffect = acidZone.Prefab.GetComponent<SkillEffect>();

        float relativeX = defenderX - attackerX;
        float relativeY = defenderY - attackerY;

        int moveX = 0;
        int moveY = 0;

        if (-0.5f < relativeX && relativeX < 0.5f)
            moveX = 0;
        else if (relativeX <= -0.5f)
            moveX = -1;
        else
            moveX = 1;

        if (-0.5f < relativeX && relativeX < 0.5f)
            moveX = 0;
        else if (relativeX <= -0.5f)
            moveX = -1;
        else
            moveX = 1;

        int skillX = moveX;
        int skillY = moveY;

        if (skillX == 0 && skillY == 0) {
            skillX = (int)(relativeX / Mathf.Abs(relativeX));
            skillY = (int)(relativeY / Mathf.Abs(relativeY));
        }

        int skillIndex = 0;

        if (acidBubbleSlot.CurrentCooltime <= 0) {
            skillIndex = 1;
        } else if (acidZoneSlot.CurrentCooltime <= 0) {
            Vector2 size = acidZoneEffect.GetColliderRange();

            if (Mathf.Abs(relativeX) <= size.x || Mathf.Abs(relativeY) <= size.y) {
                skillX = 1;
                skillY = 0;
                skillIndex = 2;
            }
        }

        discreteActionsOut[0] = PosIndex(moveX);
        discreteActionsOut[1] = PosIndex(moveY);

        discreteActionsOut[2] = PosIndex(skillX);
        discreteActionsOut[3] = PosIndex(skillY);

        discreteActionsOut[4] = skillIndex;
    }

    int PosIndex(int k) {
        switch (k) {
            case 1:
                return 2;

            case -1:
                return 1;
        }

        return 0;
    }
}
