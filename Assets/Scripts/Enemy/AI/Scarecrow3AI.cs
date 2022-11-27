using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Scarecrow3AI : EnemyAI
{
    int dy;

    public float bound = 2.0f;

    public Scarecrow3AI() {
        dy = Random.Range(0, 2) == 0 ? -1 : 1;
    }

    public override void Judge(BattleAgent agent, in ActionBuffers actionsOut) {
        var discreteActionsOut = actionsOut.DiscreteActions;

        discreteActionsOut[0] = 0;
        discreteActionsOut[1] = 0;
        discreteActionsOut[2] = 0;

        float attackerX = agent.attacker.transform.localPosition.x;
        float attackerY = agent.attacker.transform.localPosition.y;

        float halfHeight = agent.fieldHeight / 2;

        if (halfHeight - Mathf.Abs(attackerY) < bound)
            dy = -dy;

        discreteActionsOut[0] = GetDirection(0, dy);
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
