using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//기타 정보, 정보 불러오기용도
public class Character : MonoBehaviour
{
    private Status status;
    private EquipSkills equipSkills;

    public bool customSetting = false;
    
    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<Status>();
        equipSkills = GetComponent<EquipSkills>();

        if (customSetting == false)
            LoadData();
        else
            Init();
    }

    //배틀시 불러오는 용도
    public void LoadData() {
        bool isCharacter = this.CompareTag("Character");
        bool isEnemy     = this.CompareTag("Enemy");

        if (isCharacter) {
            Managers.Data.LoadCharacterData(gameObject);
            equipSkills.skillList = Managers.Data.characterData.skillList;
        }

        if (isEnemy)
            Managers.Data.LoadEnemyData(gameObject);

        Init();
    }

    public void LoadData(CharacterData characterData) {
        characterData.Load(gameObject);
        Init();
    }

    void Init() {
        equipSkills.Init();
        status.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
