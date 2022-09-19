using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//기타 정보, 정보 불러오기용도
public class Character : MonoBehaviour
{
    private Status status;
    private EquipSkills equipSkills;
    
    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<Status>();
        equipSkills = GetComponent<EquipSkills>();

        LoadData();
    }

    public void LoadData() {
        bool isCharacter = this.CompareTag("Character");
        bool isEnemy     = this.CompareTag("Enemy");
        bool isCharacterList = this.CompareTag("CharacterList");

        if (isCharacter) {
            Managers.Data.GetCharacterData(gameObject);
            equipSkills.skillList = Managers.Data.characterData.skillList;
        }

        if (isEnemy)
            Managers.Data.GetEnemyData(gameObject);

        if (isCharacterList) {
            CharacterSlot slot = transform.GetComponentInParent<CharacterSlot>();
    
            int slotIndex = slot.slotIndex;

            Managers.Data.GetCharacterData(slotIndex, gameObject);
        }

        equipSkills.Init();
        status.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
