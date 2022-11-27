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
    private BufferSensorComponent attackerSkillsBufferSensor;
    private BufferSensorComponent defenderSkillsBufferSensor;
    private BufferSensorComponent fieldBufferSensor;

    private float startX;
    private float startY;
    private float endX;
    private float endY;

    [HideInInspector]
    public float fieldWidth;
    [HideInInspector]
    public float fieldHeight;
    ///////

    public EnemyAI ai;

    public string aiName;

    public override void Initialize()
    {
        m_ResetParams = Academy.Instance.EnvironmentParameters;

        BufferSensorComponent[] bufferSensors = GetComponentsInChildren<BufferSensorComponent>();

        foreach (var sensor in bufferSensors) {
            if (sensor.SensorName.Equals("AttackerSkillsBufferSensor"))
                attackerSkillsBufferSensor = sensor;
            if (sensor.SensorName.Equals("DefenderSkillsBufferSensor"))
                defenderSkillsBufferSensor = sensor;
            if (sensor.SensorName.Equals("FieldBufferSensor"))
                fieldBufferSensor = sensor;
        }

        float scale = field.leftWall.transform.localScale.x / 2;
        startX = field.leftWall.transform.localPosition.x + scale;
        startY = field.downWall.transform.localPosition.y + scale;

        endX = field.rightWall.transform.localPosition.x - scale;
        endY = field.upWall.transform.localPosition.y - scale;

        fieldWidth = endX - startX;
        fieldHeight = endY - startY;

        Debug.Log(string.Format("[Collecting] {0} {1}", fieldWidth, fieldHeight));

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

    방어자
    공격자 기준 좌표

    */

    private float[] GetBinaryEncoding(int n, int len) {
        float[] binary = new float[len];

        for (int i=len-1; i>=0; i--) {
            binary[i] = n % 2;
            n = n / 2;
        }

        return binary;
    }

    //좌표 정규화 0~1로 함
    //|----------------->|
    public Vector2 NormalizePos(Vector2 pos) {
        float width = fieldWidth / 2;
        float height = fieldHeight / 2;

        float x = Mathf.Clamp(pos.x / width, -1, 1);
        float y = Mathf.Clamp(pos.y / height, -1, 1);

        return new Vector2(x, y);
    }

    //크기 정규화 (0~1)
    //|----------------->|
    public Vector2 NormalizeSize(Vector2 pos) {
        float width = fieldWidth;
        float height = fieldHeight;

        float x = Mathf.Clamp(pos.x / width, 0, 1);
        float y = Mathf.Clamp(pos.y / height, 0, 1);

        return new Vector2(x, y);
    }

    //캐릭터 기준 거리 정규화 (-1~1) 안씀
    //|----------------->| +1
    //|<-----------------| -1
    public Vector2 NormalizeRelativePos(Vector2 pos) {
        float width = fieldWidth;
        float height = fieldHeight;

        float baseX = attacker.transform.localPosition.x;
        float baseY = attacker.transform.localPosition.y;

        float x = Mathf.Clamp((pos.x - baseX) / width, -1, 1);
        float y = Mathf.Clamp((pos.y - baseY) / height, -1, 1);

        return new Vector2(x, y);
    }

    public float NormalizeValue(Effect effect, Character a, Character d) {

        return 0;
    }

    public void CollectObservationsCharacter(Character a, Character d, VectorSensor sensor) {
        //캐릭터 위치 (x, y) (벽과의 거리를 나타냄)
        if (a == attacker) {
            sensor.AddObservation(NormalizePos(a.transform.localPosition));
            Debug.Log(string.Format("[Collecting] [{0}] [pos={1}",
                a.tag, NormalizePos(a.transform.localPosition))
            );
        } else {
            sensor.AddObservation(NormalizeRelativePos(a.transform.localPosition));
            Debug.Log(string.Format("[Collecting] [{0}] [pos={1}",
                a.tag, NormalizeRelativePos(a.transform.localPosition))
            );
        }
        
        //캐릭터 크기 scale 값 (x, y)
        sensor.AddObservation(NormalizeSize(a.GetSize()));
        Debug.Log(string.Format("[Collecting] [{0}] [size={1} {2}", a.tag, NormalizeSize(a.GetSize()), a.GetSize()));

        //캐릭터의 HP, SPD
        float distance1s = a.status.CurrentSPD * Managers.Battle.baseSpeed * 1.0f;

        sensor.AddObservation(Mathf.Clamp01(a.status.CurrentHP / a.status.MaxHP));
        sensor.AddObservation(NormalizeSize(new Vector2(distance1s, distance1s)));
        

        /* 액티브 스킬 정보
        - 액티브 1 / 패시브 or 없음 0
        - 최대 쿨타임 (스킬 쿨타임 / 120초, 쿨타임이 긴 스킬이면 적절한 때에 쓰라는 것)
        - 사용 가능 여부 (0~ 사용불가능 1 사용가능)
        - 스킬 사정거리 (x, y)
        - 스킬 생성 좌표 (캐릭터 기준 x, y) (양수 기준)
        - 스킬 범위 (x, y)
        - 효과 (2개만)
            - 태그 (2^8)
            - 연산자 (5)
            - 수치 (1) (현재 수치에서 어느정도 비율인지를 확인) (패시브는 이미 적용되서 할 이유 없음)
        */
        CollectObservationsEquipSkills(a, d);
        
    }

    public EffectTag InverseEffectTag(EffectTag tag) {
        switch (tag) {
            case EffectTag.ATTACKER_MHP:
                return EffectTag.DEFENDER_MHP;

            case EffectTag.ATTACKER_ATK:
                return EffectTag.DEFENDER_ATK;

            case EffectTag.ATTACKER_DEF:
                return EffectTag.DEFENDER_DEF;

            case EffectTag.ATTACKER_SPD:
                return EffectTag.DEFENDER_SPD;
            
            case EffectTag.ATTACKER_CHP:
                return EffectTag.DEFENDER_CHP;

            case EffectTag.ATTACKER_DMG:
                return EffectTag.DEFENDER_DMG;

            case EffectTag.DEFENDER_MHP:
                return EffectTag.ATTACKER_MHP;

            case EffectTag.DEFENDER_ATK:
                return EffectTag.ATTACKER_ATK;

            case EffectTag.DEFENDER_DEF:
                return EffectTag.ATTACKER_DEF;

            case EffectTag.DEFENDER_SPD:
                return EffectTag.ATTACKER_SPD;
            
            case EffectTag.DEFENDER_CHP:
                return EffectTag.ATTACKER_CHP;

            case EffectTag.DEFENDER_DMG:
                return EffectTag.ATTACKER_DMG;
        }

        return tag;
    }

    public void CollectObservationsEquipSkills(Character a, Character d) {
        int skillVectorSize = 1 + 1 + 1 + 2 + 2 + 2 + (8 + 5 + 1) * 2;

        for (int i=0; i<EquipSkills.MaxSlot; i++) {
            SkillSlot skillSlot = a.equipSkills.GetSkillSlot(i);
            Skill skill = skillSlot.GetSkill();

            if (skill is not ActiveSkill)
                continue;

            float[] listObservation = new float[skillVectorSize];
            int k = 0;

            ActiveSkill activeSkill = skillSlot.GetActiveSkill();
            SkillEffect skillEffect = Managers.Pool.GetSkillEffectInfo(activeSkill.Prefab);

            //스킬 구분
            listObservation[k++] = 1;

            //최대 쿨타임 (120초 기준)
            listObservation[k++] = Mathf.Clamp01(activeSkill.Cooltime / 120.0f);

            //현재 쿨타임 (0~1 사용불가능 1 사용가능)
            listObservation[k++] = Mathf.Clamp01(1.0f - skillSlot.CurrentCooltime / activeSkill.Cooltime);

            //사정거리 (x, y)
            Vector2 range = new Vector2(activeSkill.Range, activeSkill.Range);
            Vector2 normRange = NormalizeSize(range);
            listObservation[k++] = normRange.x;
            listObservation[k++] = normRange.y;

            //생성 좌표 (x, y) (필드 좌표가 아닌 캐릭터 좌표이므로 size로 함)
            Vector2 createPos = skillEffect.GetCreatePos();
            Vector2 normCreatePos = NormalizeSize(createPos);
            listObservation[k++] = normCreatePos.x;
            listObservation[k++] = normCreatePos.y;

            //스킬 범위 (x, y)
            Vector2 colliderRange = skillEffect.GetColliderRange();
            Vector2 normColliderRange = NormalizeSize(colliderRange);
            listObservation[k++] = normColliderRange.x;
            listObservation[k++] = normColliderRange.y;

            Debug.Log("[Collecting] Skill " + createPos + " " + colliderRange);

            bool inverseTag = false;
            if (a == defender)
                inverseTag = true;

            float[] effectObs = GetObservationsEffects(activeSkill.Effects, attacker, defender, inverseTag);

            //복사
            for (int ei=0; ei<effectObs.Length; ei++)
                listObservation[k++] = effectObs[ei];

            if (a == attacker)
                attackerSkillsBufferSensor.AppendObservation(listObservation);
            else
                defenderSkillsBufferSensor.AppendObservation(listObservation);
        }
    }

    //방어자의 스킬을 확인 할때는 반대로 적용되어야 함
    //예를 들어 데미지를 주는 스킬은 DEFENDER_DAMAGE 이지만
    //방어자의 스킬을 볼 때도 DEFENDER_DAMAGE = 적에게 데미지가 들어가는 것으로 판단을 할 수 있음
    public float[] GetObservationsEffects(Effect[] effects, Character a, Character d, bool inverseTag=false) {
        int effectVectorSize = 8 + 5 + 1;

        float[] result = new float[effectVectorSize * 2];

        for (int i=0; i<2; i++) {
            if (i >= effects.Length)
                break;

            int k = effectVectorSize * i;

            Effect effect = effects[i];

            EffectTag tag = effect.EffectTag;

            if (inverseTag == true) {
                tag = InverseEffectTag(tag);
            }

            float[] binary = GetBinaryEncoding((int)tag, 8);

            for (int bi=0; bi<binary.Length; bi++)
                result[k++] = binary[bi];

            //k~k+5-1 까지 한 곳을 1로 만듦
            result[k + (int)effect.EffectOperator] = 1;
            k += 5;

            float damage = Managers.Battle.CalculateDamage(effect.Formula, a.status, d.status);
            result[k++] = Mathf.Clamp01(damage / d.status.CurrentHP);

            if (damage > 0)
                Debug.Log("[Damage] " + result[k-1]);
        }

        return result;
    }

    /*
    필드 스킬 8개
    - 시전자
    - 캐스팅 시간 (1이면 완료)
    - 지속 시간
    - 캐릭터 기준 거리 (x, y)
    - 회전 여부
    - 속도 (x, y) (투사체)
    - 스킬 범위 (x, y)
    - 효과 (2개만)
        - 태그 (2^8)
        - 연산자 (5)
        - 수치 (1) (현재 수치에서 어느정도 비율인지를 확인)
    */
    public void CollectObservationsFieldSkills() {
        List<SkillEffect> skillEffects = field.GetList();
        int skillVectorSize = 2 + 1 + 1 + 2 + 1 + 2 + 2 + 1 + (8 + 5 + 1) * 2;

        for (int i=0; i<8; i++) {
            if (i >= skillEffects.Count)
                break;

            SkillEffect effect = skillEffects[i];
            ActiveSkill activeSkill = effect.activeSkill;
            float[] listObservation = new float[skillVectorSize];
            int k = 0;

            //시전자
            //{1, 0} 내 스킬
            //{0, 1} 상대 스킬
            bool inverseTag;
            if (gameObject.CompareTag(effect.GetAttackerTag()) == true) {
                listObservation[k] = 1;
                inverseTag = false;
            } else {
                listObservation[k+1] = 1;
                inverseTag = true;
            }

            k += 2;

            //캐스팅 시간
            listObservation[k++] = Mathf.Clamp01(effect.currentCastingTime);

            //지속 시간
            listObservation[k++] = Mathf.Clamp01(effect.currentDuration);

            //현재 좌표
            Vector2 pos = NormalizeRelativePos(effect.GetCurrentPos());
            listObservation[k++] = pos.x;
            listObservation[k++] = pos.y;

            //회전 여부
            listObservation[k++] = effect.transform.eulerAngles.z / 360;

            //속도 (-1 0 1)
            Vector2 velocity = effect.GetVelocity();
            listObservation[k++] = velocity.x;
            listObservation[k++] = velocity.y;

            //스킬 범위
            Vector2 skillRange = NormalizeSize(effect.GetColliderRange());
            listObservation[k++] = skillRange.x;
            listObservation[k++] = skillRange.y;

            //공격 여부 (닿았으면 무시해도 됨)
            bool isUsed = effect.isAttacked;
            listObservation[k++] = isUsed ? 0 : 1; 

            Debug.Log("[Collecting] " + field.gameObject.name  + " Skill pos:" + effect.GetCurrentPos() + " " + pos + " range: " + skillRange + " time: " + effect.currentCastingTime + ", " + effect.currentDuration + " rotation: " + effect.transform.eulerAngles.z + " used: " + isUsed);



            float[] effectObs = GetObservationsEffects(activeSkill.Effects, attacker, defender, inverseTag);

            //복사
            for (int ei=0; ei<effectObs.Length; ei++)
                listObservation[k++] = effectObs[ei];

            fieldBufferSensor.AppendObservation(listObservation);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Debug.Log("Collecting...");
        
        CollectObservationsCharacter(attacker, defender, sensor);
        CollectObservationsCharacter(defender, attacker, sensor);

        CollectObservationsFieldSkills();
        

        //배틀 시간
        sensor.AddObservation(battleEnvController.timer / battleEnvController.MaxBattleTime);
    }


    //상대를 스킬로 맞추는 경우 깎은 HP 비율을 보상으로 줌 
    //액션 처리
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int moveDirection = actionBuffers.DiscreteActions[0];
        int skillDirection = actionBuffers.DiscreteActions[1];

        var skillIndex = actionBuffers.DiscreteActions[2];

        Debug.Log("[Action] + " + attacker.tag + "Move " + moveDirection + " Skill " + skillDirection + " index "+ skillIndex);

        //                         x   ↑  ↗   →  ↘   ↓  ↙   ←   ↖
        float[] dx = new float[] { 0,  0,  1,  1,  1,  0, -1, -1, -1};
        float[] dy = new float[] { 0,  1,  1,  0, -1, -1, -1,  0,  1};

        Vector2 vector = new Vector2(dx[moveDirection], dy[moveDirection]);

        //캐릭터 움직이기
        float baseSpeed = Managers.Battle.baseSpeed;

        if (attacker.isStopping == false)
            attacker.rigid.MovePosition(attacker.rigid.position + vector * attacker.status.CurrentSPD * baseSpeed * Time.deltaTime);

        //스킬 사용하기
        if (skillIndex > 0 && skillDirection > 0) {
            
            bool result = attacker.equipSkills.UseSkill(skillIndex - 1, skillDirection);
            //AddReward(result == true ? 0.02f : 0);
        }
    }
    
    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask) {
        Debug.Log("Write");

        for (int i=0; i<EquipSkills.MaxSlot; i++) {
            SkillSlot skillSlot = attacker.equipSkills.GetSkillSlot(i);
            ActiveSkill activeSkill = skillSlot.GetActiveSkill();

            bool available = true;
            
            if (activeSkill == null)
                available = false;

            if (skillSlot.CurrentCooltime > 0)
                available = false;
            
            actionMask.SetActionEnabled(2, i + 1, available);
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

        int moveDirection = 0;
        int skillDirection = 0;

        int inputX = 0;
        int inputY = 0;

        if (Input.GetKey(KeyCode.UpArrow))
            inputY += 1;
        
        if (Input.GetKey(KeyCode.DownArrow))
            inputY -= 1;

        if (Input.GetKey(KeyCode.LeftArrow))
            inputX -= 1;
        
        if (Input.GetKey(KeyCode.RightArrow))
            inputX += 1;

        for (int i=0; i<9; i++) {
            if (dx[i] == inputX && dy[i] == inputY) {
                moveDirection = i;
                skillDirection = i;
                break;
            }
        }
        
        discreteActionsOut[0] = moveDirection;
        discreteActionsOut[1] = skillDirection;
        discreteActionsOut[2] = 0;

        if (Input.GetKey(KeyCode.Q)) {
            discreteActionsOut[2] = 1;
            Debug.Log("Pressed Q Key");
        }

        if (Input.GetKey(KeyCode.W)) {
            discreteActionsOut[2] = 2;
            Debug.Log("Pressed W Key");
        }

        if (Input.GetKey(KeyCode.E))
            discreteActionsOut[2] = 3;

        if (Input.GetKey(KeyCode.A))
            discreteActionsOut[2] = 4;

        if (Input.GetKey(KeyCode.S))
            discreteActionsOut[2] = 5;

        if (Input.GetKey(KeyCode.D))
            discreteActionsOut[2] = 6;

        if (Input.GetKey(KeyCode.X))
            discreteActionsOut[2] = 7;

        if (Input.GetKey(KeyCode.Space))
            discreteActionsOut[2] = 8;
    }
}
