using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnvController : MonoBehaviour
{
    [Tooltip("Max Battle Time (sec)")] public int MaxBattleTime = 120;

    public GameObject character;
    public GameObject enemy;

    private Status characterStatus;
    private Status enemyStatus;

    private BattleAgent characterAgent;
    private BattleAgent enemyAgent;

    private Field field;

    public float timer = 0;

    public void StopTrainer() {
        Managers.Battle.StopTrainer();
    }

    void Start()
    {
        characterAgent = character.GetComponent<BattleAgent>();
        enemyAgent = enemy.GetComponent<BattleAgent>();

        characterStatus = character.GetComponent<Status>();
        enemyStatus = enemy.GetComponent<Status>();

        field = GetComponent<Field>();

        ResetScene();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= MaxBattleTime && MaxBattleTime > 0) {
            EndEpisode();
        }

        if (characterStatus.CurrentHP <= 0) {
			EndEpisode();
        }

        if (enemyStatus.CurrentHP <= 0) {
			EndEpisode();
        }
    }

    //상대 HP를 깐 만큼 보상을 줌
    //+는 승리 -는 패배여야함 (셀프플레이시)
    public void EndEpisode() {
        float characterHPRate = characterStatus.CurrentHP / characterStatus.MaxHP;
        float enemyHPRate = enemyStatus.CurrentHP / enemyStatus.MaxHP;
        float timeBonus = 2f - 1.5f * timer / MaxBattleTime; //2~0.5

        float differentHPRate;
        float rewardBonus = 2.5f;
        float finalReward;

        characterHPRate = Mathf.Clamp01(characterHPRate);
        enemyHPRate = Mathf.Clamp01(enemyHPRate);

        differentHPRate = characterHPRate - enemyHPRate;
        finalReward = rewardBonus * differentHPRate * timeBonus;

        if (characterAgent != null) {
            characterAgent.AddReward(finalReward);
            characterAgent.EndEpisode();
        }

        //적이 완성된 모델을 가지고 있는 경우
        if (enemyAgent != null) {
            enemyAgent.AddReward(-finalReward);
            enemyAgent.EndEpisode();
        }

        //전투면 바로 종료
        if (Managers.Battle.isLearning == false) {
            string result = "무승부";

            if (differentHPRate > 0)
                result = "승리";
            if (differentHPRate < 0)
                result = "패배";

            Managers.Battle.result = result;
            Managers.Battle.EndBattle();
        }

        ResetScene();
    }
	
    public void ResetScene()
    {
		timer = 0;

        field.ClearEffects();

        int random = Random.Range(0, 2) * 2 - 1;

        character.transform.position = field.transform.position + new Vector3(-5 * random, Random.Range(-2.0f, 2.0f), 0);
        enemy.transform.position = field.transform.position + new Vector3(5 * random, Random.Range(-2.0f, 2.0f), 0);

        Debug.Log("Reset Position " + character.transform.position);

        character.GetComponent<Status>().SetDefaultHP();
        enemy.GetComponent<Status>().SetDefaultHP();

        for (int i=0; i<EquipSkills.maxSlot; i++) {
			character.GetComponent<EquipSkills>().GetSkillSlot(i).ResetCooltime();
            enemy.GetComponent<EquipSkills>().GetSkillSlot(i).ResetCooltime();
        }

        
        //characterEquipSkills.GetSkillSlot(0).ResetCooltime();
    }
}