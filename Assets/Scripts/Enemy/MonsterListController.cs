using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.MLAgents.Policies;

//캐릭터 목록 관련
public class MonsterListController : MonoBehaviour
{
    public List<CharacterData> monsterDatas = new List<CharacterData>();
    public CharacterList characterList;

    public void BattleButton() {
        CharacterData character = Managers.Data.GetCurrentCharacterData();
        CharacterData enemy     = characterList.GetCurrentCharacterData();

        if (enemy == null)
            return;

        character.behaviorType = BehaviorType.Default;

        Managers.Battle.BattleSetting(false, character, enemy);
    }

    public void LearningButton() {
        CharacterData character = Managers.Data.GetCurrentCharacterData();
        CharacterData enemy     = characterList.GetCurrentCharacterData();

        if (enemy == null)
            return;

        character.behaviorType = BehaviorType.Default;

        Managers.Battle.BattleSetting(true, character, enemy);
    }

    public void ControlButton() {
        CharacterData character = Managers.Data.GetCurrentCharacterData();
        CharacterData enemy     = characterList.GetCurrentCharacterData();

        if (enemy == null)
            return;

        character.behaviorType = BehaviorType.HeuristicOnly;

        Managers.Battle.BattleSetting(false, character, enemy);
    }

    public void ExecuteButton() {
        //bool result = characterList.GetCurrentIndex() < monsterDataList.Count;

        Managers.Battle.StartLearning();

        //if (result == true)
        //    Managers.Battle.BattleSetting(true, Managers.Data.currentCharacterData, monsterDataList[characterList.GetCurrentIndex()]);
    }

    void Start() {
        Debug.Log("?");
        monsterDatas = Managers.DB.MonsterDB.GetFieldMonsterList(GameManager.Instance.monsterFieldName);

        characterList.SetDatas(monsterDatas);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
