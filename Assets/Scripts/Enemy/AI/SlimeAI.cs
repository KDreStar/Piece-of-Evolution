using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Scarecrow2AI : SlimeAI
{
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

        discreteActionsOut[0] = 0;
        discreteActionsOut[1] = 0;

        discreteActionsOut[2] = 0;
        discreteActionsOut[3] = 0;

        discreteActionsOut[4] = 0;

        EquipSkills equipSkills = agent.attacker.equipSkills;
        SkillSlot   acidBubbleSlot = equipSkills.GetSkillSlot(0);
        ActiveSkill acidBubble = acidBubbleSlot.GetActiveSkill();

        SkillSlot   acidZoneSlot = equipSkills.GetSkillSlot(1);
        ActiveSkill acidZone = acidBubbleSlot.GetActiveSkill();

        

        for (int i=0; i<EquipSkills.MaxSlot; i++) {
            SkillSlot skillSlot = 
            ActiveSkill activeSkill = skillSlot.GetActiveSkill();
            
            if (activeSkill == null || skillSlot.CurrentCooltime > 0)
                continue;

            //유니티 scale은 오브젝트 크기와 같음 그러므로 x2
            for (int k=0; k<4; k++) {
                //4방향 박스 안에 적이 존재하면 그 방향으로 스킬 발사
                Collider2D[] list = Physics2D.OverlapBoxAll(
                    new Vector2(x1, y1),
                    new Vector2(activeSkill.Range * 2, 2),
                    k * 45
                );

                for (int p=0; p<list.Length; p++) {
                    // 상대 ..... 나   x1 > x2
                    // 나 ....... 상대 x1 < x2 
                    if (list[p].CompareTag(agent.defender.gameObject.tag) == true) {
                        int inputXi = x1 > x2 ? 1 : 2;
                        int inputYi = y1 > y2 ? 1 : 2;

                        if (k == 0)
                            inputYi = 0;
                        if (k == 2)
                            inputXi = 0;

                        discreteActionsOut[2] = inputXi;
                        discreteActionsOut[3] = inputYi;
                        discreteActionsOut[4] = i + 1;

                        return;
                    }
                }
            }
        }
    }
}
