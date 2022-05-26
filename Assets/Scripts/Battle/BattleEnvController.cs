using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnvController : MonoBehaviour
{
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 5000;

    public GameObject character;
    public GameObject enemy;

    private Status characterStatus;
    private Status enemyStatus;

    private BattleAgent characterAgent;
    private BattleAgent enemyAgent;

    private Field field;

    private int m_ResetTimer;

    void Start()
    {
        characterAgent = character.GetComponent<BattleAgent>();
        enemyAgent = enemy.GetComponent<BattleAgent>();

        characterStatus = character.GetComponent<Status>();
        enemyStatus = enemy.GetComponent<Status>();

        field = GetComponent<Field>();

        //ResetScene();
    }

    void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0) {
            End();
        }

        if (characterStatus.CurrentHP <= 0) {
            characterAgent.AddReward(5.0f);
            enemyAgent.AddReward(-5.0f);

            End();
        }

        if (enemyStatus.CurrentHP <= 0) {
            enemyAgent.AddReward(5.0f);
            characterAgent.AddReward(-5.0f);

            End();
        }
    }

    public void End() {
        characterAgent.EndEpisode();
        enemyAgent.EndEpisode();
        ResetScene();
    }

    public void ResetScene()
    {
        m_ResetTimer = 0;

        character.transform.position = field.transform.position + new Vector3(-5, 0, 0);
        enemy.transform.position = field.transform.position + new Vector3(5, 0, 0);

        Debug.Log("Reset Position " + character.transform.position);

        character.GetComponent<Status>().SetHP(500);
        enemy.GetComponent<Status>().SetHP(500);

        for (int i=0; i<EquipSkills.maxSlot; i++) {
            character.GetComponent<EquipSkills>().GetSkillSlot(i).ResetCooltime();
            enemy.GetComponent<EquipSkills>().GetSkillSlot(i).ResetCooltime();
        }

        //field.ClearEffects();
        //characterEquipSkills.GetSkillSlot(0).ResetCooltime();
    }
}