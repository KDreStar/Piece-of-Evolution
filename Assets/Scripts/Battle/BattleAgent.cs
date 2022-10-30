using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;

public class BattleAgent : Agent
{
    public Character attacker;
    public Character defender;

    EnvironmentParameters m_ResetParams;
    public Field field;
    public BattleEnvController battleEnvController;

    
    private float startX;
    private float startY;
    private float endX;
    private float endY;
    private float fieldWidth;
    private float fieldHeight;
    ///////

    public EnemyAI ai;

    public override void Initialize()
    {
        m_ResetParams = Academy.Instance.EnvironmentParameters;

        float scale = field.leftWall.transform.localScale.x / 2;
        startX = field.leftWall.transform.localPosition.x + scale;
        startY = field.downWall.transform.localPosition.y - scale;

        endX = field.rightWall.transform.localPosition.x - scale;
        endY = field.upWall.transform.localPosition.y + scale;

        fieldWidth = endX - startX;
        fieldHeight = endY - startY;
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

    /*
    정규화함

    공격자
    - 위치 x, y (벽 시작 좌표, 끝 좌표 기준으로 0~1 정규화)
    - 크기 x, y
    - HP (HP 비율로 0~1값)
    - SPD 이동속도를 정규화하는 경우 필드를 얼마나 빠르게 이동하는 경우를 0~1로 나타내는 것이므로
      1초에 왼쪽에서 오른쪽으로 얼마나 이동하는지를 나타내면 될 듯
    - 피격 범위
      위치로 하는 경우 점으로 판단되므로 직사각형의 범위가 필요함

    ATK DEF는 스킬에 따라 다르므로 (받는 데미지, 주는 데미지) 의미가 없으므로 제거

    - 스킬 정보
      스킬 번호는 아무 의미 없음
      효과를 벡터로 표현해야 함
      효과 구분은 다음과 같이 함

      태그 + 연산자

      HP를 50 소모하고 10초동안 ATK 50증가 버프를 주는 액티브다
      태그 자신.HP / 연산자 - / 수치 50
      태그 자신.ATK / 연산자 + / 수치 50 

      이렇게 하면 정확하지만 복잡하므로 간단하게 구분을 해보자
      태그 / 코스트만 보는 것이다.

      코스트가 높으면 좋은 스킬임을 감지할 수 있을 것이다.

      액티브 패시브 구분 (0 패시브 1 액티브)
      우선 쿨타임이 필요 (0~1 정규화)

      줄 수 있는 데미지 (스킬 데미지 - 상대방 방어력) / 상대방 HP (0~1)

      스킬 범위 표현
      직사각형 표현
      (x1, y1) (x2, y2), 회전값

      나갈 수 있는 범위 7 * 4 (왼오) (상하) (대각) (대각)

    최종
    스킬 구분
    00 스킬 없음
    10 패시브
    01 액티브

    코스트 0 1 2 3 4 5 / 5

    패시브인 경우 밑의 벡터는 전부 0
    액티브인 경우

    스킬의 가치는 모델이 판단해야 하므로 제외
    수집하는 벡터만 필요함

    효과는 일단 생략
    주는 데미지 

    범위
    (x1, y1, x2, y2, rotation(x, y, z))

    방어자도 똑같이

    필드 스킬
    (스킬에는 캐스팅 모션후 즉발 스킬)
    (즉발)
    (즉발 + 투사체)
    - 시전자 0 자신 / 1 상대 (0이면 이미 나간 스킬을 고려하는것) (1이면 상대 스킬을 피해야하는 의미)
    - 캐스팅 상태 0~1 (완료시 (1) 그 스킬 범위에 데미지가 들어간다는 의미)
    - 범위 (x1, y1) (x2, y2) 회전상태
    - 투사체 스킬은 현재 어디 위치에 있는지, 어느 방향으로 가는지를 알아야함
    - 투사체 범위도 하면 될듯

    - 즉 스킬의 사정거리, 스킬의 현 상태의 범위

    (6 + 7 * 8)
    (6 + 7 * 8)
    (14 * 8)
    1
    */

    private float[] GetBinaryEncoding(int n) {
        float[] binary = new float[16];

        for (int i=15; i>=0; i--) {
            binary[i] = n % 2;
            n = n / 2;
        }

        return binary;
    }

    //-1~1로 정규화
    public float NormalizationX(float x) {
        float result = x / (fieldWidth / 2);

        return Mathf.Clamp(result, -1, 1);
    }

    public float NormalizationScaleX(float size) {
        float result = size / fieldWidth;

        return Mathf.Clamp01(result);
    }

    public float NormalizationY(float y) {
        float result = y / (fieldHeight / 2);

        return Mathf.Clamp(result, -1, 1);
    }

    public float NormalizationScaleY(float size) {
        float result = size / fieldWidth;

        return Mathf.Clamp01(result);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Debug.Log("Collecting...");
        
        //공격자의 위치 (x, y) (벽과의 거리를 나타냄)
        sensor.AddObservation(NormalizationX(attacker.transform.localPosition.x));
        sensor.AddObservation(NormalizationY(attacker.transform.localPosition.y));
        Debug.Log("[Collecting] [" + attacker.tag +"] [x = " + NormalizationX(attacker.transform.localPosition.x) + " y=]" + NormalizationY(attacker.transform.localPosition.y));
        //Debug.Log("[Collecting] [" + attacker.tag +"-" + defender.tag + "] [x = " + NormalizationX(attacker.transform.localPosition.x) + "]");

        //공격자의 크기 scale 값 (x, y)
        sensor.AddObservation(NormalizationScaleX(attacker.GetSizeX()));
        sensor.AddObservation(NormalizationScaleY(attacker.GetSizeY()));

        Debug.Log("[Collecting] [" + attacker.tag +"] [size=" + attacker.GetSizeX() + " " +NormalizationScaleX(attacker.GetSizeX()));


        //공격자의 HP, SPD
        sensor.AddObservation(attacker.status.CurrentHP / attacker.status.MaxHP);
        sensor.AddObservation(attacker.status.CurrentSPD);

        /*
        공격자의 스킬
        스킬 구분 00 10 01 (없음 / 패시브 / 액티브)
        코스트
        쿨타임
        x, y scale z 회전값 
        range, effect object scale을 가져오면

        주는 데미지

        */
        for (int i=0; i<EquipSkills.maxSlot; i++) {
            SkillSlot skillSlot = attacker.equipSkills.GetSkillSlot(i);
            Skill skill = skillSlot.GetSkill();

            if (skill is PassiveSkill) {
                //스킬 구분
                sensor.AddObservation(new float[2] {1, 0});

                //코스트
                sensor.AddObservation(skill.Cost / 5);

                //쿨타임
                sensor.AddObservation(0);

                //스킬 범위
                sensor.AddObservation(new float[2]);

                //데미지
                sensor.AddObservation(0);
            } else if (skill is ActiveSkill) {
                ActiveSkill activeSkill = skillSlot.GetActiveSkill();
                //스킬 구분
                sensor.AddObservation(new float[2] {0, 1});

                //코스트
                sensor.AddObservation(skill.Cost / 5);

                //쿨타임
                sensor.AddObservation(skillSlot.CurrentCooltime / activeSkill.Cooltime);

                //스킬 범위, 모든 스킬은 캐릭터 기준으로 나가므로 중심좌표를 중앙값으로 해도 되긴함
                GameObject effect = activeSkill.Effect;

                sensor.AddObservation(NormalizationScaleX(activeSkill.Range * 2));
                sensor.AddObservation(NormalizationScaleY(effect.transform.localScale.y));

                //데미지
                if (defender.status.CurrentHP < 1)
                    sensor.AddObservation(1);
                else {
                    float skillDamage = attacker.status.Calculate(activeSkill.Damage);
                    float def = defender.status.CurrentDEF;
                    float damage = skillDamage - def;
                    damage = Mathf.Clamp(damage, 1, defender.status.CurrentHP);
                    sensor.AddObservation(damage / defender.status.CurrentHP);
                }
            } else {
                sensor.AddObservation(new float[2 + 1 + 1 + 2 + 1]);
            }

        }

        //방어자의 상대위치 (x, y)
        sensor.AddObservation(NormalizationX(defender.transform.localPosition.x));
        sensor.AddObservation(NormalizationY(defender.transform.localPosition.y));

        //방어자의 크기 scale 값 (x, y)
        sensor.AddObservation(NormalizationX(defender.GetSizeX()));
        sensor.AddObservation(NormalizationY(defender.GetSizeY()));

        //방어자의 HP, SPD
        sensor.AddObservation(defender.status.CurrentHP / defender.status.MaxHP);
        sensor.AddObservation(defender.status.CurrentSPD);

        /*
        방어자의 스킬
        스킬 구분 00 10 01 (없음 / 패시브 / 액티브)
        코스트
        쿨타임
        공격 범위
        주는 데미지
        */
        for (int i=0; i<EquipSkills.maxSlot; i++) {
            SkillSlot skillSlot = defender.equipSkills.GetSkillSlot(i);
            Skill skill = skillSlot.GetSkill();

            if (skill is PassiveSkill) {
                //스킬 구분
                sensor.AddObservation(new float[2] {1, 0});

                //코스트
                sensor.AddObservation(skill.Cost / 5);

                //쿨타임
                sensor.AddObservation(0);

                //스킬 범위
                sensor.AddObservation(new float[2]);

                //데미지
                sensor.AddObservation(0);
            } else if (skill is ActiveSkill) {
                ActiveSkill activeSkill = skillSlot.GetActiveSkill();
                //스킬 구분
                sensor.AddObservation(new float[2] {0, 1});

                //코스트
                sensor.AddObservation(skill.Cost / 5);

                //쿨타임
                sensor.AddObservation(skillSlot.CurrentCooltime / activeSkill.Cooltime);

                //스킬 범위
                GameObject effect = activeSkill.Effect;

                sensor.AddObservation(NormalizationScaleX(activeSkill.Range * 2));
                sensor.AddObservation(NormalizationScaleY(effect.transform.localScale.y));

                //데미지
                if (attacker.status.CurrentHP < 1)
                    sensor.AddObservation(1);
                else {
                    float skillDamage = defender.status.Calculate(activeSkill.Damage);
                    float def = attacker.status.CurrentDEF;
                    float damage = skillDamage - def;
                    //Debug.Log("[Damage]" + activeSkill.Damage  + " " + skillDamage + " " + damage);
                    damage = Mathf.Clamp(damage, 1, attacker.status.CurrentHP);
                    sensor.AddObservation(damage / attacker.status.CurrentHP);
                }
            } else {
                sensor.AddObservation(new float[2 + 1 + 1 + 2 + 1]);
            }

        }
        
        //스킬 이펙트 8개
        //시전자 / (최대 범위 위치 x, y / 크기 x, y) / 회전 / 현재 범위 위치 x, y / 크기 / 진행도
        List<SkillEffect> skillEffects = field.GetList();

        for (int i=0; i<8; i++) {
            if (i < skillEffects.Count) {
                SkillEffect effect = skillEffects[i];

                if (effect == null) {
                    sensor.AddObservation(new float[8]);
                    continue;
                }

                //{1, 0} 내 스킬
                //{0, 1} 상대 스킬
                if (gameObject.CompareTag(effect.GetAttackerTag()) == true)
                    sensor.AddObservation(new float[] {1, 0});
                else
                    sensor.AddObservation(new float[] {0, 1});

                //최대 범위의 중간 위치 <- 이건 나중에

                //스킬의 상대위치
                sensor.AddObservation(NormalizationX(effect.transform.localPosition.x));
                sensor.AddObservation(NormalizationY(effect.transform.localPosition.y));

                sensor.AddObservation(NormalizationScaleX(effect.transform.localScale.x));
                sensor.AddObservation(NormalizationScaleY(effect.transform.localScale.y));

                //데미지
                sensor.AddObservation(0.2f);
 
                sensor.AddObservation(effect.direction / 7);
            } else {
                sensor.AddObservation(new float[8]);
            }
        }

        sensor.AddObservation(battleEnvController.timer / battleEnvController.MaxBattleTime);
    }

    //액션 처리
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int moveXi = actionBuffers.DiscreteActions[0];
        int moveYi = actionBuffers.DiscreteActions[1];

        int skillXi = actionBuffers.DiscreteActions[2];
        int skillYi = actionBuffers.DiscreteActions[3];

        var skillIndex = actionBuffers.DiscreteActions[4];
        int[] delta = new int[] {0, -1, 1};


        Debug.Log("[Action] + " + attacker.tag + "Move " + delta[moveXi] + ", " + delta[moveYi] + " Skill " + delta[skillXi] + ", " + delta[skillYi] + " index "+ skillIndex);

        

        Vector2 vector = new Vector2(delta[moveXi], delta[moveYi]);

        //캐릭터 움직이기
        attacker.transform.Translate(vector * attacker.status.CurrentSPD * 0.3f * Time.deltaTime);

        //스킬 사용하기
        if (skillIndex > 0) {
            if (!(skillXi == 0 && skillYi == 0)) {
                bool result = attacker.equipSkills.UseSkill(skillIndex - 1, delta[skillXi], delta[skillYi]);
            }
            //AddReward(result == true ? 0.02f : 0);
        }
    }

    //인스펙터에서 Heuristic Only를 하거나 모델이 없는 경우 작동
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaa" + (ai == null));

        if (ai == null)
            PlayerAction(actionsOut);
        else {
            
            ai.Judge(this, actionsOut);
        }
    }

    //0x 1<- 2->
    //0x 1v 2^
    public void PlayerAction(in ActionBuffers actionsOut) {
        var discreteActionsOut = actionsOut.DiscreteActions;

        //                         x   ↑  ↗   →  ↘   ↓  ↙   ←   ↖
        float[] dx = new float[] { 0,  0,  1,  1,  1,  0, -1, -1, -1};
        float[] dy = new float[] { 0,  1,  1,  0, -1, -1, -1,  0,  1};

        int inputX = 0;
        int inputY = 0;

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

        inputX = inputX > 0 ? 2 : (inputX < 0 ? 1 : 0);
        inputY = inputY > 0 ? 2 : (inputY < 0 ? 1 : 0);

        discreteActionsOut[0] = inputX;
        discreteActionsOut[2] = inputX;

        discreteActionsOut[1] = inputY;
        discreteActionsOut[3] = inputY;
        
        discreteActionsOut[4] = 0;

        if (Input.GetKey(KeyCode.Q)) {
            discreteActionsOut[4] = 1;
            Debug.Log("Pressed Q Key");
        }

        if (Input.GetKey(KeyCode.W)) {
            discreteActionsOut[4] = 2;
            Debug.Log("Pressed W Key");
        }

        if (Input.GetKey(KeyCode.E))
            discreteActionsOut[4] = 3;

        if (Input.GetKey(KeyCode.A))
            discreteActionsOut[4] = 4;

        if (Input.GetKey(KeyCode.S))
            discreteActionsOut[4] = 5;

        if (Input.GetKey(KeyCode.D))
            discreteActionsOut[4] = 6;

        if (Input.GetKey(KeyCode.X))
            discreteActionsOut[4] = 7;

        if (Input.GetKey(KeyCode.Space))
            discreteActionsOut[4] = 8;
    }
}
