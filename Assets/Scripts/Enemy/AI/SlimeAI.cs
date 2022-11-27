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

        SkillSlot   acidBubbleSlot = null;
        ActiveSkill acidBubble = null;
        SkillEffect acidBubbleEffect = Managers.Pool.GetSkillEffectInfo(acidBubble.Prefab);
        int acidBubbleIndex = 0;

        SkillSlot   acidZoneSlot = null;
        ActiveSkill acidZone = null;
        SkillEffect acidZoneEffect = Managers.Pool.GetSkillEffectInfo(acidZone.Prefab);
        int acidZoneIndex = 1;

        for (int i=0; i<EquipSkills.MaxSlot; i++) {
            ActiveSkill activeSkill = equipSkills.GetActiveSkill(i);

            if (activeSkill == null)
                continue;

            if (activeSkill.No == 1001) {
                acidBubbleSlot = equipSkills.GetSkillSlot(i);
                acidBubble = acidBubbleSlot.GetActiveSkill();
                acidBubbleIndex = i;
            } else if (activeSkill.No == 1002) {
                acidZoneSlot = equipSkills.GetSkillSlot(i);
                acidZone = acidZoneSlot.GetActiveSkill();
                acidZoneIndex = i;
            }
        }

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

        if (-0.5f < relativeY && relativeY < 0.5f)
            moveY = 0;
        else if (relativeY <= -0.5f)
            moveY = -1;
        else
            moveY = 1;

        int skillX = moveX;
        int skillY = moveY;

        if (skillX == 0 && skillY == 0) {
            skillX = (int)(relativeX / Mathf.Abs(relativeX));
            skillY = (int)(relativeY / Mathf.Abs(relativeY));
        }

        int skillIndex = 0;

        if (acidBubbleSlot.CurrentCooltime <= 0) {
            skillIndex = acidBubbleIndex;
        } else if (acidZoneSlot.CurrentCooltime <= 0) {
            Vector2 size = acidZoneEffect.GetColliderRange() / 2 + agent.defender.GetSize() / 2;

            Debug.Log("[Size] " + size + " " + relativeX + " " + relativeY);

            if (Mathf.Abs(relativeX) <= size.x && Mathf.Abs(relativeY) <= size.y) {
                skillX = 1;
                skillY = 0;
                skillIndex = acidZoneIndex;
            }
        }

        discreteActionsOut[0] = GetDirection(moveX, moveY);
        discreteActionsOut[1] = GetDirection(skillX, skillY);
        discreteActionsOut[2] = skillIndex;
    }

    public int GetDirection(int inputX, int inputY) {
        //                         x   ↑  ↗   →  ↘   ↓  ↙   ←   ↖
        float[] dx = new float[] { 0,  0,  1,  1,  1,  0, -1, -1, -1};
        float[] dy = new float[] { 0,  1,  1,  0, -1, -1, -1,  0,  1};

        for (int i=0; i<9; i++) {
            if (inputX == dx[i] && inputY == dy[i])
                return i;
        }

        return 0;
    }
}
