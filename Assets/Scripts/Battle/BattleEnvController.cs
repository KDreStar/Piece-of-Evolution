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
            EndEpisode();
        }

        if (characterStatus.CurrentHP <= 0) {
			EndEpisode();
        }

        if (enemyStatus.CurrentHP <= 0) {
			EndEpisode();
        }
    }

    public void EndEpisode() {
        float characterHPRate = characterStatus.CurrentHP / characterStatus.MaxHP;
        float enemyHPRate = enemyStatus.CurrentHP / enemyStatus.MaxHP;
        float timeBonus = 2 - m_ResetTimer / MaxEnvironmentSteps; //2~1

        float differentHPRate;
        float rewardBonus = 2.5f;
        float finalReward;

        characterHPRate = characterHPRate < 0 ? 0 : characterHPRate;
        enemyHPRate = enemyHPRate < 0 ? 0 : enemyHPRate;
        timeBonus = timeBonus < 0 ? 0 : timeBonus;

        differentHPRate = characterHPRate - enemyHPRate;
        finalReward = rewardBonus * timeBonus * differentHPRate;

        characterAgent.AddReward(finalReward);
        enemyAgent.AddReward(-finalReward);

        characterAgent.EndEpisode();
        enemyAgent.EndEpisode();
        ResetScene();
    }
	
    public void ResetScene()
    {
		m_ResetTimer = 0;

        int random = Random.Range(0, 2) * 2 - 1;

        character.transform.position = field.transform.position + new Vector3(-5 * random, Random.Range(-2.0f, 2.0f), 0);
        enemy.transform.position = field.transform.position + new Vector3(5 * random, Random.Range(-2.0f, 2.0f), 0);

        Debug.Log("Reset Position " + character.transform.position);

        character.GetComponent<Status>().SetHP(500);
        enemy.GetComponent<Status>().SetHP(500);

        for (int i=0; i<EquipSkills.maxSlot; i++) {
			character.GetComponent<EquipSkills>().GetSkillSlot(i).ResetCooltime();
            enemy.GetComponent<EquipSkills>().GetSkillSlot(i).ResetCooltime();
        }

        field.ClearEffects();
        //characterEquipSkills.GetSkillSlot(0).ResetCooltime();
    }
}