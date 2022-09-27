using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//캐릭터 목록 관련
public class CharacterListController : MonoBehaviour
{
    public TMP_InputField characterName;
    public CharacterList characterList;

    public void CreateCharacter() {
        Managers.Data.CreateCharacter(characterName.text);
        characterList.UpdateSlots();
    }

    public void DeleteCharacter() {
        Managers.Data.DeleteCharacter();
        characterList.UpdateSlots();
    }

    public void SelectCharacter() {
        Managers.Data.SelectCharacter(characterList.GetCurrentIndex());
    }

    void Start() {
        characterList.SetDatas(Managers.Data.characterDataList);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
