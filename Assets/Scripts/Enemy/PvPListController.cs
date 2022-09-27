using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvPListController : MonoBehaviour
{
    public CharacterList characterList;
    public List<CharacterData> pvpDataList = new List<CharacterData>();

    public void ChangeList() {
        Managers.Data.ChangePvPList(pvpDataList);
        //characterList.UpdateSlots();
    }

    public void StartPvP() {
        Managers.Battle.BattleSetting(false, characterList.GetCurrentCharacter());
    }

    void Start() {
        ChangeList();

        characterList.SetDatas(pvpDataList);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
