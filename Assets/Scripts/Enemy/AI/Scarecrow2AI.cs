using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Scarecrow2AI : EnemyAI
{
    public override void Judge(BattleAgent agent, in ActionBuffers actionsOut) {
        var discreteActionsOut = actionsOut.DiscreteActions;

        float x1 = agent.attacker.transform.localPosition.x;
        float y1 = agent.attacker.transform.localPosition.y;

        float x2 = agent.defender.transform.localPosition.x;
        float y2 = agent.defender.transform.localPosition.y;

        Debug.Log("수집");

        discreteActionsOut[0] = 0;
        discreteActionsOut[1] = 0;

        discreteActionsOut[2] = 0;
        discreteActionsOut[3] = 0;

        discreteActionsOut[4] = 0;

        for (int i=0; i<EquipSkills.maxSlot; i++) {
            SkillSlot skillSlot = agent.attacker.equipSkills.GetSkillSlot(i);
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
