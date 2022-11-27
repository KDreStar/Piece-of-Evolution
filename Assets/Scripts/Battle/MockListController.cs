using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Policies;

//캐릭터 목록 관련
public class MockListController : MonoBehaviour
{
    public CharacterList characterList;

    public void MockBattle(bool playerControl) {
        CharacterData character = Managers.Data.GetCurrentCharacterData();
        CharacterData enemy = characterList.GetCurrentCharacterData();

        if (enemy == null)
            return;

        character.behaviorType = playerControl == true ? BehaviorType.HeuristicOnly : BehaviorType.Default;
        enemy.behaviorType     = BehaviorType.InferenceOnly;

        Managers.Battle.BattleSetting(false, character, enemy);
    }

    public void Learning() {
        CharacterData character = Managers.Data.GetCurrentCharacterData();
        CharacterData enemy = characterList.GetCurrentCharacterData();

        if (enemy == null)
            return;

        character.behaviorType = BehaviorType.Default;
        enemy.behaviorType     = BehaviorType.InferenceOnly;

        Managers.Battle.BattleSetting(true, character, enemy);
    }

    public void SelfPlay() {
        CharacterData character = Managers.Data.GetCurrentCharacterData();
        character.behaviorType = BehaviorType.Default;

        Managers.Battle.BattleSetting(true, character, character);
    }

    void Start() {
        characterList.SetDatas(Managers.Data.gameData.characterDatas);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
