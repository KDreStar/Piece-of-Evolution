using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

//스크립트에 추가되는 데이터이므로 스크립터블 오브젝트로
public class EnemyAI
{
    //BattleAgent 에서 model이 없는 경우 휴리스틱으로 동작
    //-> 이때 몬스터 AI 스크립트가 있으면 휴리스틱이 그걸로 동작
    //public BattleAgent agent;
    public EnemyAI() {
        
    }

    public virtual void Judge(BattleAgent agent, in ActionBuffers actionsOut) {

    }
}
