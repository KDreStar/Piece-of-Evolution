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
    public BattleEnvController battleEnvController;


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
    공격자 139
    - 위치 3
    - 스킬 정보 (스킬 번호 / 쿨타임) 17 * 8
    - 스탯 (HP 비율, MaxHP, ATK, DEF, SPD) 5
    
    방어자 139
    - 위치 3
    - 스킬 정보 (스킬 번호 / 쿨타임) 17 * 8
    - 스탯 (HP 비율, MaxHP, ATK, DEF, SPD) 5

    스킬 이펙트 [생성 시간 기준 8개를 가져옴] 256
    - 스킬 번호 / 시전자 / 위치 x, y / 크기 x, y, z / 진행방향 direction 8 * 8

    남은 배틀 시간 1
    벽 거리 4

    = 549

    추가로 추가할 거 배틀 시간
    */

    private float[] GetBinaryEncoding(int n) {
        float[] binary = new float[16];

        for (int i=15; i>=0; i--) {
            binary[i] = n % 2;
            n = n / 2;
        }

        return binary;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Debug.Log("Collecting...");
        
        //공격자의 위치 (필드 기준 좌표로 함)
        sensor.AddObservation(attacker.transform.localPosition);

        //공격자의 스킬 8개의 번호, 쿨타임
        /*
        스킬 번호를 열거형으로 하면 원핫 인코딩이 되어야함 그러나 그렇게 하면
        관측하는 벡터가 너무 많아짐

        바이너리 인코딩은 어떨까?
        스킬 번호를 바이너리로 바꿔서 넣어주면 구분은 될 것이다.

        넉넉하게 길이를 2^16으로
        */
        for (int i=0; i<EquipSkills.maxSlot; i++) {
            SkillSlot skillSlot = attackerEquipSkills.GetSkillSlot(i);

            int no = 0;
            float cooltime = 0;

            if (skillSlot.skill != null) {
                no = skillSlot.skill.No;
                cooltime = skillSlot.CurrentCooltime;
            }

            
            sensor.AddObservation(GetBinaryEncoding(no));
            sensor.AddObservation(cooltime);
        }

        sensor.AddObservation(attackerStatus.CurrentHP / attackerStatus.MaxHP);
        sensor.AddObservation(attackerStatus.MaxHP);
        sensor.AddObservation(attackerStatus.CurrentATK);
        sensor.AddObservation(attackerStatus.CurrentDEF);
        sensor.AddObservation(attackerStatus.CurrentSPD);


        //방어자의 위치
        sensor.AddObservation(defender.transform.localPosition);

        //방어자의 스킬 8개의 번호, 쿨타임
        for (int i=0; i<EquipSkills.maxSlot; i++) {
            SkillSlot skillSlot = defenderEquipSkills.GetSkillSlot(i);

            int no = 0;
            float cooltime = 0;

            if (skillSlot.skill != null) {
                no = skillSlot.skill.No;
                cooltime = skillSlot.CurrentCooltime;
            }

            sensor.AddObservation(GetBinaryEncoding(no));
            sensor.AddObservation(cooltime);
        }

        sensor.AddObservation(defenderStatus.CurrentHP / defenderStatus.MaxHP);
        sensor.AddObservation(defenderStatus.MaxHP);
        sensor.AddObservation(defenderStatus.CurrentATK);
        sensor.AddObservation(defenderStatus.CurrentDEF);
        sensor.AddObservation(defenderStatus.CurrentSPD);
        
        //스킬 이펙트 8개
        //스킬 번호 / 시전자 / 위치 x, y / 크기 x, y, z / 진행방향 direction
        List<SkillEffect> skillEffects = field.GetList();

        for (int i=0; i<8; i++) {
            if (i < skillEffects.Count) {
                SkillEffect effect = skillEffects[i];

                if (effect == null) {
                    sensor.AddObservation(new float[32]);
                    continue;
                }

                sensor.AddObservation(GetBinaryEncoding(effect.GetSkillNo()));
                //{1, 0} 내 스킬
                //{0, 1} 상대 스킬
                if (gameObject.CompareTag(effect.GetAttackerTag()) == true)
                    sensor.AddObservation(new float[] {1, 0});
                else
                    sensor.AddObservation(new float[] {0, 1});

                sensor.AddObservation(effect.transform.localPosition);
                sensor.AddObservation(effect.transform.localScale);
                sensor.AddOneHotObservation(effect.direction - 1, 8);
            } else {
                sensor.AddObservation(new float[32]);
            }
        }

        sensor.AddObservation(battleEnvController.timer / battleEnvController.MaxBattleTime);

        float x = attacker.transform.localPosition.x;
        float y = attacker.transform.localPosition.y;

        //상하좌우
        sensor.AddObservation(4 - y); //0~8
        sensor.AddObservation(y - (-4)); //0~8
        sensor.AddObservation(x + 7); //0~14
        sensor.AddObservation(7 - x); //0~14
    }

    //액션 처리
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var direction = actionBuffers.DiscreteActions[0];
        var skillIndex = actionBuffers.DiscreteActions[1];

        Debug.Log(attacker.tag + " Action " + direction + " " + skillIndex);

        //                         x   ↑  ↗   →  ↘   ↓  ↙   ←   ↖
        float[] dx = new float[] { 0,  0,  1,  1,  1,  0, -1, -1, -1};
        float[] dy = new float[] { 0,  1,  1,  0, -1, -1, -1,  0,  1};

        Vector2 vector = new Vector2(dx[direction], dy[direction]);

        //캐릭터 움직이기
        attacker.transform.Translate(vector * attackerStatus.CurrentSPD * 0.3f * Time.deltaTime);

        //스킬 사용하기
        if (skillIndex > 0) {
            Debug.Log(skillIndex + " Skill Used " + direction);
            bool result = attackerEquipSkills.UseSkill(skillIndex - 1, direction == 0 ? lastDirection : direction);

            //AddReward(result == true ? 0.02f : 0);
        }

        //AddReward(-0.003f);

        if (lastDirection != direction && direction > 0)
            lastDirection = direction;
    }

    //인스펙터에서 Heuristic Only를 하거나 모델이 없는 경우 직접 조작할 수 있음
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
