using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Policies;

//캐릭터 목록 관련
public class MockListController : MonoBehaviour
{
    public CharacterList characterList;

    public void MockBattle(bool playerControl) {
        bool result = characterList.GetCurrentIndex() < Managers.Data.characterDataList.Count;

        BehaviorType type = playerControl == true ? BehaviorType.HeuristicOnly : BehaviorType.InferenceOnly;

        if (result == true) {
            Managers.Battle.BattleSetting(false, Managers.Data.currentCharacterData, Managers.Data.characterDataList[characterList.GetCurrentIndex()], type);
        }
    }

    public void Learning() {
        bool result = characterList.GetCurrentIndex() < Managers.Data.characterDataList.Count;

        if (result == true) {
            Managers.Battle.BattleSetting(true, Managers.Data.currentCharacterData, Managers.Data.characterDataList[characterList.GetCurrentIndex()], BehaviorType.InferenceOnly);
        }
    }

    public void SelfPlay() {
        bool result = characterList.GetCurrentIndex() < Managers.Data.characterDataList.Count;

        if (result == true) {
            Managers.Battle.BattleSetting(true, Managers.Data.currentCharacterData, Managers.Data.characterDataList[characterList.GetCurrentIndex()]);
        }
    }

    void Start() {
        characterList.SetDatas(Managers.Data.characterDataList);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
