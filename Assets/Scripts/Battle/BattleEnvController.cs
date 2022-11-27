using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnvController : MonoBehaviour
{
    [Tooltip("Max Battle Time (sec)")]
    public int MaxBattleTime = 120;

    public Character character;
    public Character enemy;

    private Field field;

    public float timer = 0;

    public float finishReward = 2.0f;
    public float timePenalty  = -1.0f;
    public float hitReward    = 1.0f; 

    void Start()
    {
        field = GetComponent<Field>();

        ResetScene();
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= MaxBattleTime && MaxBattleTime > 0) {
            EndEpisode();
        }

        if (character.status.CurrentHP <= 0) {
			EndEpisode();
        }

        if (enemy.status.CurrentHP <= 0) {
			EndEpisode();
        }
    }

    public void AddHitReward(Character attacker, Character defender, float damage) {
        float maxHP = defender.status.MaxHP;
        float currentHP = defender.status.CurrentHP;

        float normalizedDamage = Mathf.Min((damage + currentHP) / maxHP, damage / maxHP);
        normalizedDamage = Mathf.Clamp01(normalizedDamage);

        float reward = hitReward * normalizedDamage;

        Debug.Log(string.Format("[Hit Reward] {0} {1}", damage, normalizedDamage));

        attacker.agent.AddReward(reward);
        defender.agent.AddReward(-reward);
    }

    //상대 HP를 깐 만큼 보상을 줌
    //+는 승리 -는 패배여야함 (셀프플레이시)
    //기본 점수 3
    //자신 HP 비율 패널티 -0.5~0
    //시간 패널티 1
    //무승부 = 0
    public void EndEpisode() {
        float characterHPRate = character.status.CurrentHP / character.status.MaxHP;
        float enemyHPRate = enemy.status.CurrentHP / enemy.status.MaxHP;
        float timeReward = timePenalty * timer / MaxBattleTime; //0~-1

        float differentHPRate;

        characterHPRate = Mathf.Clamp01(characterHPRate);
        enemyHPRate = Mathf.Clamp01(enemyHPRate);
        differentHPRate = characterHPRate - enemyHPRate;

        float coef = 0;

        if (differentHPRate > 0)
            coef = 1;
        if (differentHPRate < 0)
            coef = -1;

        Debug.Log(string.Format("[Finish Reward] {0} {1}", finishReward, timeReward));
        float reward = coef * (finishReward + timeReward);

        if (character.agent != null) {
            character.agent.AddReward(reward);
            character.agent.EndEpisode();
        }

        //적이 완성된 모델을 가지고 있는 경우
        if (enemy.agent != null) {
            enemy.agent.AddReward(-reward);
            enemy.agent.EndEpisode();
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

        character.status.SetDefaultHP();
        enemy.status.SetDefaultHP();

        for (int i=0; i<EquipSkills.MaxSlot; i++) {
			character.equipSkills.GetSkillSlot(i).ResetCooltime();
            enemy.equipSkills.GetSkillSlot(i).ResetCooltime();
        }

        
        //characterEquipSkills.GetSkillSlot(0).ResetCooltime();
    }
}