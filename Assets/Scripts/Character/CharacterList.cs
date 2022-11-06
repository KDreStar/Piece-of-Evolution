using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum CharacterListType {
    None,
    Characters,
    Monsters,
    PvP
}

public class CharacterList : MonoBehaviour
{
    public CharacterSlot[] slots = new CharacterSlot[4];
    private List<CharacterData> datas; //포인터용도
    
    public CharacterListType listType = CharacterListType.Characters;
    int page = 1;
    int maxPage = 1;
    int index = 0;

    private Sprite idle;
    private Sprite selected;

    public TextMeshProUGUI textPage;

    //이전 버튼 누르면
    public void Prev() {
        if (page <= 1)
            return;
        
        page--;
        UpdateSlots();
    }

    //다음 버튼 누르면
    public void Next() {
        if (page > maxPage)
            return;
        
        page++;
        UpdateSlots();
    }

    public void Select(int i) {
        index = i;
        UpdateSlots();
    }

    public int GetCurrentIndex() {
        return (page - 1) * 4 + index;
    }

    public GameObject GetCurrentCharacter() {
        return slots[index].character.gameObject;
    }

    public void SetDatas(List<CharacterData> datas) {
        this.datas = datas;
        //Debug.Log("데이터" + datas + " " +datas.Count);
        UpdateSlots();
    }

    //현재 페이지에 맞게 Slot 업데이트
    //타입에 맞게 알아서 불러옴
    public void UpdateSlots() {
        if (datas == null) {
            Debug.Log("Null");
            return;
        }

        maxPage = datas.Count / 4 + 1;

        if (page > maxPage)
            page = maxPage;

        for (int i=0; i<4; i++) {
            int k = (page - 1) * 4 + i;

            if (k >= datas.Count) {
                slots[i].SetActiveSlot(false);
            } else {
                slots[i].SetActiveSlot(true);
                slots[i].UpdateSlot(datas[k]);
            }

            if (index == i) {
                slots[i].image.sprite = selected;
            } else {
                slots[i].image.sprite = idle;
            }
        }

        textPage.text = "" + page + "/" + maxPage;
    }

    void Awake() {
        Sprite[] sprites = Resources.LoadAll<Sprite>("UI/Frame");
        idle = sprites[1];
        selected = sprites[7];
    }

    // Start is called before the first frame update
    void Start()
    {
        if (listType == CharacterListType.Characters) {
            index = Managers.Data.currentCharacterIndex; //Load시 자동으로 마지막으로 선택한 캐릭터 인덱스로 됨

            page = (int)(index / 4) + 1;
            index = index % 4; 
        }
    }



    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
    }
}
