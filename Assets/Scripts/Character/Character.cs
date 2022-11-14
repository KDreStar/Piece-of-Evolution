using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//기타 정보, 정보 불러오기용도
public class Character : MonoBehaviour
{
    [Tooltip("세이브 데이터로부터 로드를 하지 않음")]
    public bool dontLoad = false;

    //UI용도
    public Image image;

    [HideInInspector]
    public Status status;

    [HideInInspector]
    public EquipSkills equipSkills;

    [HideInInspector]
    public Rigidbody2D rigid;

    private SpriteRenderer sr;

    [HideInInspector]
    public bool isStopping = false;
    
    
    // Start is called before the first frame update
    void Awake() {
        status = GetComponent<Status>();
        equipSkills = GetComponent<EquipSkills>();
        sr = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (dontLoad == false)
            LoadData();
        else
            Init();
    }

    public Vector2 GetSize() {
        if (sr == null)
            return Vector2.zero;

        return sr.bounds.size;
    }   

    public float GetSizeX() {
        if (sr == null)
            return 0;
        
        return sr.bounds.size.x * transform.localScale.x;
    }

    public float GetSizeY() {
        if (sr == null)
            return 0;
        
        return sr.bounds.size.y * transform.localScale.x;
    }

    //배틀시 불러오는 용도
    public void LoadData() {
        bool isCharacter = this.CompareTag("Character");
        bool isEnemy     = this.CompareTag("Enemy");
        bool isCurrentCharacter = this.CompareTag("CurrentCharacter");

        Debug.Log("[Load CharacterData] " + Managers.Data.characterData.baseHP);

        if (isCharacter) {
            Managers.Data.LoadCharacterData(gameObject);
            //equipSkills.skillList = Managers.Data.characterData.skillList;
        }

        if (isEnemy)
            Managers.Data.LoadEnemyData(gameObject);

        if (isCurrentCharacter) {
            Managers.Data.LoadCurrentCharacterData(gameObject);
        }

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
