using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터 목록 관련
public class MockListController : MonoBehaviour
{
    public CharacterList characterList;

    public void MockBattle(bool playerControl) {
        bool result = characterList.GetCurrentIndex() < Managers.Data.characterDataList.Count;

        if (result == true) {
            Managers.Battle.BattleSetting(false, Managers.Data.characterDataList[characterList.GetCurrentIndex()], Managers.Data.currentCharacterData, playerControl);
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
