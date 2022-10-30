using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    public Character character;
    
    [HideInInspector]
    public Image image;
    private Button button;

    public void UpdateSlot(CharacterData characterData) {
        character.LoadData(characterData);
    }

    public void SetActiveSlot(bool active) {
        character.gameObject.SetActive(active);
        button.interactable = active;
    }

    void Awake() {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
