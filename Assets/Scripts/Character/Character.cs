using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents.Policies;

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

    [HideInInspector]
    public SpriteRenderer sr;

    [HideInInspector]
    public BehaviorParameters bp;

    [HideInInspector]
    public bool isStopping = false;
    
    [HideInInspector]
    public BattleAgent agent;
    
    // Start is called before the first frame update
    void Awake() {
        status = GetComponent<Status>();
        equipSkills = GetComponent<EquipSkills>();
        sr = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        agent = GetComponent<BattleAgent>();
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

        CharacterData data = null;

        if (isCharacter)
            data = Managers.Data.GetBattleCharacterData();

        if (isEnemy)
            data = Managers.Data.GetBattleEnemyData();

        if (isCurrentCharacter)
            data = Managers.Data.GetCurrentCharacterData();

        if (data != null)
            LoadData(data);

        Init();
    }

    public void LoadData(CharacterData data) {
        status.name = data.name;
        
        status.baseHP  = data.baseHP;
        status.baseATK = data.baseATK;
        status.baseDEF = data.baseDEF;
        status.baseSPD = data.baseSPD;

        if (image != null && data.sprite != null)
            image.sprite = data.sprite;

        if (sr != null && data.sprite != null)
            sr.sprite = data.sprite;


        equipSkills.LoadSkills(data.skillNos);

        if (bp != null) {
            if (data.model != null) {
                bp.Model = data.model;
                bp.BehaviorType = data.behaviorType;
            } else {
                agent.ai = Managers.DB.AIFactory.Create(data.aiName);
                bp.BehaviorType = data.behaviorType;
            }
        }
        
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
