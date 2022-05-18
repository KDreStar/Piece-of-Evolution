using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class CharacterAgent : Agent
{
    public GameObject character;
    public Status characterStatus;
    public SkillSlot characterSkillSlot;
    public GameObject enemy;
    public SkillSlot enemySkillSlot;
    public Status enemyStatus;
    EnvironmentParameters m_ResetParams;

    public override void Initialize()
    {
        m_ResetParams = Academy.Instance.EnvironmentParameters;

        characterSkillSlot = character.GetComponent<SkillSlot>();
        characterStatus = character.GetComponent<Status>();

        enemySkillSlot = enemy.GetComponent<SkillSlot>();
        enemyStatus = enemy.GetComponent<Status>();
        SetResetParameters();
    }

    //수집하는 정보
    //내 위치 / 상대 위치 / 내 스킬 정보 (일단 쿨타임)
    //나중에 스킬 이펙트 정보 + 현재 필드에 있는 스킬들
    public override void CollectObservations(VectorSensor sensor)
    {
        Debug.Log("Collecting...");
        
        sensor.AddObservation(character.transform.position);
        sensor.AddObservation(enemy.transform.position);

        ActiveSkill activeSkill = characterSkillSlot.GetActiveSkill(0);

        if (activeSkill == null)
            sensor.AddObservation(0);
        else
            sensor.AddObservation(activeSkill.CurrentCooltime);
    }

    //취하는 액션
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var direction = actionBuffers.DiscreteActions[0];
        var skillIndex = actionBuffers.DiscreteActions[1];

        Debug.Log("Action " + direction + " " + skillIndex);

        //                         x   ↑  ↗   →  ↘   ↓  ↙   ←   ↖
        float[] dx = new float[] { 0,  0,  1,  1,  1,  0, -1, -1, -1};
        float[] dy = new float[] { 0,  1,  1,  0, -1, -1, -1,  0,  1};

        Vector2 vector = new Vector2(dx[direction], dy[direction]);

        character.transform.Translate(vector * characterStatus.CurrentSPD * 0.3f * Time.deltaTime);

        if (skillIndex > 0) {
            characterSkillSlot.UseSkill(skillIndex - 1);

            //if (result == false)
            //    AddReward(-0.5f)
        }

        if (enemyStatus.CurrentHP <= 0) {
            AddReward(3.0f);
            Debug.Log("Destroyed Enemy");
            EndEpisode();
        }
        AddReward(-0.01f);
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Begin Episode");
        SetResetParameters();
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
            Debug.Log("OK");
        }

        /*
        if (Input.GetKeyDown(KeyCode.W))
            discreteActionsOut[1] = 2;

        if (Input.GetKeyDown(KeyCode.E))
            discreteActionsOut[1] = 3;

        if (Input.GetKeyDown(KeyCode.A))
            discreteActionsOut[1] = 4;

        if (Input.GetKeyDown(KeyCode.S))
            discreteActionsOut[1] = 5;

        if (Input.GetKeyDown(KeyCode.D))
            discreteActionsOut[1] = 6;

        if (Input.GetKeyDown(KeyCode.X))
            discreteActionsOut[1] = 7;

        if (Input.GetKeyDown(KeyCode.Space))
            discreteActionsOut[1] = 8;
        */
    }

    public void SetResetParameters()
    {
       character.transform.position = new Vector2(-5, 0);
       enemy.transform.position = new Vector2(5, 0);
       enemyStatus.SetHP(500);
       //characterSkillSlot.GetActiveSkill(0).ResetCooltime();
    }
}
