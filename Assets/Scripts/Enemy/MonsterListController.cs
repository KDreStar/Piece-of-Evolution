using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//캐릭터 목록 관련
public class MonsterListController : MonoBehaviour
{
    public List<CharacterData> monsterDataList = new List<CharacterData>();
    public CharacterList characterList;

    public void BattleButton() {
        bool result = characterList.GetCurrentIndex() < monsterDataList.Count;

        if (result == true)
            Managers.Battle.BattleSetting(false, Managers.Data.currentCharacterData, monsterDataList[characterList.GetCurrentIndex()]);
    }

    public void LearningButton() {
        bool result = characterList.GetCurrentIndex() < monsterDataList.Count;

        if (result == true)
            Managers.Battle.BattleSetting(true, Managers.Data.currentCharacterData, monsterDataList[characterList.GetCurrentIndex()]);
    }

    public void ExecuteButton() {
        bool result = characterList.GetCurrentIndex() < monsterDataList.Count;

        if (result == true)
            Managers.Battle.BattleSetting(true, Managers.Data.currentCharacterData, monsterDataList[characterList.GetCurrentIndex()]);
    }

    void Start() {
        Managers.DB.MonsterDB.LoadFieldMonsterList(monsterDataList, GameManager.Instance.monsterFieldName);

        characterList.SetDatas(monsterDataList);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
