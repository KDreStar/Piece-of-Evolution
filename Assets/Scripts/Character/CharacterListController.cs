using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterListController : MonoBehaviour
{
    //페이지 나중에 구현
    public GameObject[] characterObjects = new GameObject[4];
    private Character[] characters = new Character[4];
    public Image[] images = new Image[4];

    public TMP_InputField characterName;

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i<4; i++) {
            characters[i] = characterObjects[i].GetComponent<Character>();
        }
    }

    public void CreateCharacter() {
        Managers.Data.CreateCharacter(characterName.text);
    }

    public void DeleteCharacter() {
        Managers.Data.DeleteCharacter();
    }

    public void SelectCharacter() {
        Managers.Data.SelectCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0; i<4; i++) {
            characters[i].LoadData();

            if (Managers.Data.IsCharacter(i) == true)
                characterObjects[i].SetActive(true);
            else
                characterObjects[i].SetActive(false);

            if (i == Managers.Data.currentCharacterIndex) {
                images[i].color = new Color32(64, 64, 64, 255);
            } else {
                images[i].color = new Color32(128, 128, 128, 255);
            }
        }
    }
}
