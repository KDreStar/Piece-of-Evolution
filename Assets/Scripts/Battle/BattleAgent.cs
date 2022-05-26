using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;

public class BattleAgent : Agent
{
    public GameObject attacker;
    private Status attackerStatus;
    private EquipSkills attackerEquipSkills;
    public GameObject defender;
    private EquipSkills defenderEquipSkills;
    private Status defenderStatus;
    private int lastDirection = 3;
    EnvironmentParameters m_ResetParams;
    public Field field;

    ///////

    public override void Initialize()
    {
        m_ResetParams = Academy.Instance.EnvironmentParameters;

        attackerEquipSkills = attacker.GetComponent<EquipSkills>();
        attackerStatus = attacker.GetComponent<Status>();

        defenderEquipSkills = defender.GetComponent<EquipSkills>();
        defenderStatus = defender.GetComponent<Status>();
    }

    /*
    수집 정보
    공격자 18
    - 위치 2
    - 스킬 정보 (스킬 번호 / 쿨타임) 2 * 8
    
    방어자 18
    - 위치 2
    - 스킬 정보 (스킬 번호 / 쿨타임) 2 * 8

    스킬 이펙트 [생성 시간 기준 8개를 가져옴] 64 
    - 스킬 번호 / 시전자 / 위치 x, y / 크기 x, y, z / 진행방향 direction 8 * 8

    = 100

    */
    public override void CollectObservations(VectorSensor sensor)
    {
        Debug.Log("Collecting...");
        if (attacker == null) {
            Debug.Log("Null");
        }

        if (defender == null)
            Debug.Log("Null");
        
        //공격자의 x, y
        sensor.AddObservation(attacker.transform.position.x);
        sensor.AddObservation(attacker.transform.position.y);

        //공격자의 스킬 8개의 번호, 쿨타임
        for (int i=0; i<EquipSkills.maxSlot; i++) {
            SkillSlot skillSlot = attackerEquipSkills.GetSkillSlot(i);

            int no = 0;
            float cooltime = 0;

            if (skillSlot.skill != null) {
                no = skillSlot.skill.No;
                cooltime = skillSlot.CurrentCooltime;
            }

            sensor.AddObservation(no);
            sensor.AddObservation(cooltime);
        }

        //방어자의 x, y
        sensor.AddObservation(defender.transform.position.x);
        sensor.AddObservation(defender.transform.position.y);

        //방어자의 스킬 8개의 번호, 쿨타임
        for (int i=0; i<EquipSkills.maxSlot; i++) {
            SkillSlot skillSlot = defenderEquipSkills.GetSkillSlot(i);

            int no = 0;
            float cooltime = 0;

            if (skillSlot.skill != null) {
                no = skillSlot.skill.No;
                cooltime = skillSlot.CurrentCooltime;
            }

            sensor.AddObservation(no);
            sensor.AddObservation(cooltime);
        }
        
        //스킬 이펙트 8개
        //스킬 번호 / 시전자 / 위치 x, y / 크기 x, y, z / 진행방향 direction
        List<SkillEffect> skillEffects = field.GetList();

        for (int i=0; i<8; i++) {
            if (i < skillEffects.Count) {
                SkillEffect effect = skillEffects[i];

                if (effect == null) {
                    sensor.AddObservation(new float[] {0, 0, 0, 0, 0, 0, 0, 0});
                    continue;
                }

                sensor.AddObservation(effect.GetSkillNo());
                sensor.AddObservation(effect.GetAttackerTag() == "Character" ? 2 : 3);
                sensor.AddObservation(effect.transform.position.x);
                sensor.AddObservation(effect.transform.position.y);
                sensor.AddObservation(effect.transform.localScale);
                sensor.AddObservation(effect.direction);
            } else {
                sensor.AddObservation(new float[] {0, 0, 0, 0, 0, 0, 0, 0});
            }
        }
    }

    //액션 처리
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var direction = actionBuffers.DiscreteActions[0];
        var skillIndex = actionBuffers.DiscreteActions[1];

        Debug.Log("Action " + direction + " " + skillIndex);

        //                         x   ↑  ↗   →  ↘   ↓  ↙   ←   ↖
        float[] dx = new float[] { 0,  0,  1,  1,  1,  0, -1, -1, -1};
        float[] dy = new float[] { 0,  1,  1,  0, -1, -1, -1,  0,  1};

        Vector2 vector = new Vector2(dx[direction], dy[direction]);

        //캐릭터 움직이기
        attacker.transform.Translate(vector * attackerStatus.CurrentSPD * 0.3f * Time.deltaTime);

        //스킬 사용하기
        if (skillIndex > 0) {
            Debug.Log(skillIndex + " Skill Used " + direction);
            attackerEquipSkills.UseSkill(skillIndex - 1, direction == 0 ? lastDirection : direction);

            //if (result == false)
            //    AddReward(-0.5f)
        }

        AddReward(-0.01f);

        if (lastDirection != direction && direction > 0)
            lastDirection = direction;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;

        //                         x   ↑  ↗   →  ↘   ↓  ↙   ←   ↖
        float[] dx = new float[] { 0,  0,  1,  1,  1,  0, -1, -1, -1};
        float[] dy = new float[] { 0,  1,  1,  0, -1, -1, -1,  0,  1};

        float inputX = 0;
        float inputY = 0;

        if (Input.GetKey(KeyCode.UpArrow)) {
            inputY += 1;
            Debug.Log("Pressed UpKey");
        }
        
        if (Input.GetKey(KeyCode.DownArrow))
            inputY -= 1;

        if (Input.GetKey(KeyCode.LeftArrow))
            inputX -= 1;
        
        if (Input.GetKey(KeyCode.RightArrow))
            inputX += 1;

        for (int i=0; i<9; i++) {
            if (dx[i] == inputX && dy[i] == inputY)
                discreteActionsOut[0] = i;
        }

        discreteActionsOut[1] = 0;

        if (Input.GetKey(KeyCode.Q)) {
            discreteActionsOut[1] = 1;
            Debug.Log("Pressed Q Key");
        }

        if (Input.GetKey(KeyCode.W)) {
            discreteActionsOut[1] = 2;
            Debug.Log("Pressed W Key");
        }

        if (Input.GetKey(KeyCode.E))
            discreteActionsOut[1] = 3;

        if (Input.GetKey(KeyCode.A))
            discreteActionsOut[1] = 4;

        if (Input.GetKey(KeyCode.S))
            discreteActionsOut[1] = 5;

        if (Input.GetKey(KeyCode.D))
            discreteActionsOut[1] = 6;

        if (Input.GetKey(KeyCode.X))
            discreteActionsOut[1] = 7;

        if (Input.GetKey(KeyCode.Space))
            discreteActionsOut[1] = 8;
    }
}
