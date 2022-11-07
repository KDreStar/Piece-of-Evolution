using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using System;

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

    public string aiName;

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

        if (!aiName.Equals("")) {
            AIFactory temp = new AIFactory();
            Debug.Log(aiName);
            ai = temp.Create(aiName);
        }
    }

    

    /*
    정규화시 학습 속도가 빨라짐

    공격자
    필드 중심 기준 좌표 (x, y)
    자신 캐릭터의 크기 (size.x, size.y)
    
    현재 HP (현재 HP / 최대 HP로 정규화)
    현재 이동속도 (필드에서 1초간 이동할 수 있는 거리를 정규화) (x, y)

    스킬 정보
    - 패시브 10 / 액티브 01 / 없음 00
    - 코스트 (0~5 / 5)
    - 쿨타임 (현재 쿨타임 / 최대 쿨타임)
    - 스킬 사정거리 (x, y)
    - 스킬 생성 좌표 (캐릭터 기준 x, y) (양수 기준)
    - 스킬 범위 (x, y)
    - 효과 (2개만)
        - 태그 (2^8)
        - 연산자 (5)
        - 수치 (1) (현재 수치에서 어느정도 비율인지를 확인)

    방어자
    필드 중심 기준 좌표 (x, y)
    자신 캐릭터의 크기 (size.x, size.y)
    
    현재 HP (현재 HP / 최대 HP로 정규화)
    현재 이동속도 (필드에서 1초간 이동할 수 있는 거리를 정규화) (x, y)

    스킬 정보
    - 패시브 10 / 액티브 01 / 없음 00
    - 코스트 (0~5 / 5)
    - 쿨타임 (현재 쿨타임 / 최대 쿨타임)
    - 스킬 사정거리 (x, y)
    - 스킬 생성 좌표 (캐릭터 기준 x, y) (양수 기준)
    - 스킬 범위 (x, y)
    - 효과 (2개만)
        - 태그 (2^8)
        - 연산자 (5)
        - 수치 (1) (현재 수치에서 어느정도 비율인지를 확인) (패시브는 이미 적용되서 할 이유 없음)

    필드 스킬 8개
    - 시전자
    - 코스트 (0~5 / 5)
    - 캐스팅 시간 (1이면 완료)
    - 현재 좌표 (x, y)
    - 사정거리 비율 (투사체 기준) (range가 0이면 1)
    - 속도 (x, y)
    - 스킬 범위 (x, y)
    - 효과 (2개만)
        - 태그 (2^8)
        - 연산자 (5)
        - 수치 (1) (현재 수치에서 어느정도 비율인지를 확인)

    배틀 시간 (1)
    935?
    */

    /*
    new

    공격자
    필드 중심 기준 좌표 (x, y)

    현재 HP (현재 HP / 최대 HP로 정규화)
    최대 HP
    현재 공격력
    현재 방어력
    현재 이동속도 (필드에서 1초간 이동할 수 있는 거리를 정규화) (x, y)

    스킬 정보
    - 최대 쿨타임
    - 사용 가능 여부 (0--->1)
    - 스킬 사정거리 (x, y)
    - 스킬 생성 좌표 (캐릭터 기준 x, y) (양수 기준)
    - 스킬 범위 (x, y)
    - 효과 (2개만)
        - 태그 (2^8)
        - 연산자 (5)
        - 수치 (1) (현재 수치에서 어느정도 비율인지를 확인)

    */

    private float[] GetBinaryEncoding(int n, int len) {
        float[] binary = new float[len];

        for (int i=len-1; i>=0; i--) {
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

    public void CollectObservationsCharacter(Character a, Character d, VectorSensor sensor) {
        //캐릭터 위치 (x, y) (벽과의 거리를 나타냄)
        sensor.AddObservation(NormalizationX(a.transform.localPosition.x));
        sensor.AddObservation(NormalizationY(a.transform.localPosition.y));
        Debug.Log(string.Format("[Collecting] [{0}] [x={1}, y={2}",
            a.tag, NormalizationX(a.transform.localPosition.x), NormalizationY(a.transform.localPosition.y))
        );

        //캐릭터 크기 scale 값 (x, y)
        sensor.AddObservation(NormalizationScaleX(a.GetSizeX()));
        sensor.AddObservation(NormalizationScaleY(a.GetSizeY()));

        //캐릭터의 HP, SPD
        float distance1s = a.status.CurrentSPD * Managers.Battle.baseSpeed * 1.0f;

        sensor.AddObservation(a.status.CurrentHP / a.status.MaxHP);
        sensor.AddObservation(NormalizationScaleX(distance1s));
        sensor.AddObservation(NormalizationScaleY(distance1s));

        /* 스킬 정보
        - 패시브 10 / 액티브 01 / 없음 00
        - 코스트 (0~5 / 5)
        - 최대 쿨타임 (스킬 쿨타임 / 120초, 쿨타임이 긴 스킬이면 적절한 때에 쓰라는 것)
        - 쿨타임 (현재 쿨타임 / 최대 쿨타임)
        - 스킬 사정거리 (x, y)
        - 스킬 생성 좌표 (캐릭터 기준 x, y) (양수 기준)
        - 스킬 범위 (x, y)
        - 효과 (2개만)
            - 태그 (2^8)
            - 연산자 (5)
            - 수치 (1) (현재 수치에서 어느정도 비율인지를 확인) (패시브는 이미 적용되서 할 이유 없음)
        */

        int skillVectorSize = 2 + 1 + 1 + 1 + 2 + 2 + 2 + (8 + 5 + 1) * 2;

        for (int i=0; i<EquipSkills.MaxSlot; i++) {
            SkillSlot skillSlot = a.equipSkills.GetSkillSlot(i);
            Skill skill = skillSlot.GetSkill();

            if (skill is ActiveSkill) {
                ActiveSkill activeSkill = skillSlot.GetActiveSkill();
                SkillEffect skillEffect = activeSkill.Prefab.GetComponent<SkillEffect>();

                //스킬 구분
                sensor.AddObservation(new float[2] {0, 1});

                //코스트
                sensor.AddObservation(skill.Cost / 5);

                //최대 쿨타임 (120초 기준)
                sensor.AddObservation(Mathf.Clamp01(activeSkill.Cooltime / 120.0f));

                //현재 쿨타임 (0~1 사용불가능 1 사용가능)
                sensor.AddObservation(Mathf.Clamp01(1.0f - skillSlot.CurrentCooltime / activeSkill.Cooltime));

                //사정거리 (x, y)
                sensor.AddObservation(NormalizationScaleX(activeSkill.Range));
                sensor.AddObservation(NormalizationScaleY(activeSkill.Range));



                //생성 좌표 (x, y) (필드 좌표가 아닌 캐릭터 좌표이므로 size로 함)
                Vector2 createPos = skillEffect.GetCreatePos();
                sensor.AddObservation(NormalizationScaleX(createPos.x));
                sensor.AddObservation(NormalizationScaleY(createPos.y));

                //스킬 범위 (x, y)
                Vector2 skillRange = skillEffect.GetColliderRange();
                sensor.AddObservation(NormalizationScaleX(skillRange.x));
                sensor.AddObservation(NormalizationScaleY(skillRange.y));

                CollectObservationsEffects(activeSkill.Effects, a, d, sensor);
            } else {
                sensor.AddObservation(new float[skillVectorSize]);
            }
        }
    }

    public void CollectObservationsEffects(Effect[] effects, Character a, Character d, VectorSensor sensor) {
        for (int i=0; i<2; i++) {
            if (i >= effects.Length) {
                sensor.AddObservation(new float[8 + 5 + 1]);
                continue;
            }

            Effect effect = effects[i];

            sensor.AddObservation(GetBinaryEncoding((int)effect.EffectTag, 8));
            sensor.AddOneHotObservation((int)effect.EffectOperator, 5);

            //일단 데미지로만
            float damage = Managers.Battle.CalculateDamage(effect.Formula, a.status, d.status);
            sensor.AddObservation(Mathf.Clamp01(damage / d.status.CurrentHP));
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Debug.Log("Collecting...");
        
        CollectObservationsCharacter(attacker, defender, sensor);
        CollectObservationsCharacter(defender, attacker, sensor);

        //스킬 이펙트 8개
        /*
        필드 스킬 8개
        - 시전자
        - 코스트 (0~5 / 5)
        - 캐스팅 시간 (1이면 완료)
        - 현재 좌표 (x, y)
        - 사정거리 비율 (투사체 기준) (range가 0이면 1)
        - 회전 여부
        - 속도 (x, y) (투사체)
        - 스킬 범위 (x, y)
        - 효과 (2개만)
            - 태그 (2^8)
            - 연산자 (5)
            - 수치 (1) (현재 수치에서 어느정도 비율인지를 확인)
        */
        List<SkillEffect> skillEffects = field.GetList();
        int skillVectorSize = 2 + 1 + 1 + 1 + 2 + 1 + 2 + 2 + (8 + 5 + 1) * 2;

        for (int i=0; i<8; i++) {
            if (i < skillEffects.Count) {
                SkillEffect effect = skillEffects[i];
                ActiveSkill activeSkill = effect.activeSkill;

                if (effect == null) {
                    sensor.AddObservation(new float[skillVectorSize]);
                    continue;
                }

                //시전자
                //{1, 0} 내 스킬
                //{0, 1} 상대 스킬
                if (gameObject.CompareTag(effect.GetAttackerTag()) == true)
                    sensor.AddObservation(new float[] {1, 0});
                else
                    sensor.AddObservation(new float[] {0, 1});

                //코스트
                sensor.AddObservation(activeSkill.Cost / 5);

                //캐스팅 시간
                sensor.AddObservation(effect.currentCastingTime);

                //지속 시간
                sensor.AddObservation(effect.currentDuration);

                //현재 좌표
                sensor.AddObservation(NormalizationX(effect.transform.localPosition.x));
                sensor.AddObservation(NormalizationY(effect.transform.localPosition.y));

                //회전 여부
                sensor.AddObservation(effect.direction / 8);

                //속도 (-1 0 1)
                sensor.AddObservation(effect.GetVelocity());

                //스킬 범위
                Vector2 skillRange = effect.GetColliderRange();
                sensor.AddObservation(NormalizationScaleX(skillRange.x));
                sensor.AddObservation(NormalizationScaleY(skillRange.y));

                CollectObservationsEffects(activeSkill.Effects, attacker, defender, sensor);
            } else {
                sensor.AddObservation(new float[skillVectorSize]);
            }
        }

        //배틀 시간
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
        float baseSpeed = Managers.Battle.baseSpeed;
        attacker.rigid.MovePosition(attacker.rigid.position + vector * attacker.status.CurrentSPD * baseSpeed * Time.deltaTime);

        //스킬 사용하기
        if (skillIndex > 0) {
            if (!(skillXi == 0 && skillYi == 0)) {
                bool result = attacker.equipSkills.UseSkill(skillIndex - 1, delta[skillXi], delta[skillYi]);
            }
            //AddReward(result == true ? 0.02f : 0);
        }
    }
    
    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask) {
        for (int i=0; i<EquipSkills.MaxSlot; i++) {
            SkillSlot skillSlot = attacker.equipSkills.GetSkillSlot(i);
            ActiveSkill activeSkill = skillSlot.GetActiveSkill();

            if (activeSkill == null)
                actionMask.SetActionEnabled(4, i + 1, false);
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
