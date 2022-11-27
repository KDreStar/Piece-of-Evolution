using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//스킬 슬롯
public class SkillSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Skill skill = null;
    private float currentCooltime;
    public float CurrentCooltime {
        get { return currentCooltime; }
    }

    //스킬 이미지, 쿨타임 확인 이미지
    public GameObject skillObject;
    public GameObject cooltimeObject;

    private Image skillImage;
    private Image cooltimeImage;

    private RectTransform skillImageRT;
    private RectTransform cooltimeImageRT;

    //true = 드래그 가능 false = 드래그 불가능
    public bool dragable;

    //true 장착중인 스킬
    //false 인벤토리에 있는 스킬
    public bool isEquipSkill;

    //0~7
    public int equipIndex;

    //스킬 툴팁 시작점을 알려줌
    [HideInInspector]
    public Vector2 tooltipPivot;

    public GridLayoutGroup layout;

    private IEnumerator cooltimeCoroutine;
    private bool isUse = true;

    public bool IsEmpty() {
        return skill == null ? true : false;
    }

    public void AddSkill(Skill skill) {
        Debug.Log("스킬" + skill);

        this.skill = skill;
    }

    public void RemoveSkill() {
        skill = null;
    }

    public Skill GetSkill() {
        return skill;
    }

    public ActiveSkill GetActiveSkill() {
        if (skill is not ActiveSkill)
            return null;
        return skill as ActiveSkill;
    }

    public PassiveSkill GetPassiveSkill() {
        if (skill is not PassiveSkill)
            return null;
        return skill as PassiveSkill;
    }

    //스킬을 사용하면 프리펩을 생성하면 됨
    //프리펩에는 이펙트랑 스크립트를 가지고 있음
    //각 스킬 마다 구현되어 있음
    public bool UseSkill(GameObject attacker, int direction) {
        ActiveSkill activeSkill = GetActiveSkill();

        if (activeSkill == null)
            return false;
        
        //코루틴 종료 직전 -> UseSkill 실행 -> 쿨타임 다시 0 초과됨 -> 코루틴 2개 돌아감
        //이 현상 방지
        if (isUse == false)
            return false;

        SkillEffect skillEffect = Managers.Pool.GetSkillEffect(activeSkill.Prefab);

        skillEffect.Initialize(attacker, direction);

        Debug.Log(name + "사용");

        isUse = false;
        currentCooltime = activeSkill.Cooltime;
        cooltimeCoroutine = ApplyCooltime();
        StartCoroutine(cooltimeCoroutine);

        return true;
    }

    public void ResetCooltime() {
        isUse = true;
        currentCooltime = 0;
        StopCoroutine(cooltimeCoroutine);
    }

    IEnumerator ApplyCooltime() {
        while (currentCooltime > 0) {
            currentCooltime -= Time.deltaTime;
            yield return null;
        }

        ResetCooltime();
        
        Debug.Log("재사용 가능");
    }

    public void OnBeginDrag(PointerEventData eventData){
        if (IsEmpty())
            return;

        if (dragable == false)
            return;
        
        DragSkillSlot.Instance.SetSkillSlot(this);
        DragSkillSlot.Instance.transform.position = eventData.position;
    }

    // 마우스 드래그 중일 때 계속 발생하는 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        if (IsEmpty())
            return;

        if (dragable == false)
            return;
        
        DragSkillSlot.Instance.transform.position = eventData.position;
    }

    // 마우스 드래그가 끝났을 때 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragable == false)
            return;

        DragSkillSlot.Instance.Clear();
    }

    // 드롭 했을때
    public void OnDrop(PointerEventData eventData) {
        if (dragable == false)
            return;

        SkillSlot dragSkillSlot = DragSkillSlot.Instance.skillSlot;

        Debug.Log("드랍");
        if (dragSkillSlot == null)
            return;

        SkillController.Instance.MoveSkill(this);
    }

    //마우스 오버시
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skill != null) {
            SkillTooltip.Instance.Show(skill, transform.position, CalculatePivot(eventData));
        }
    }

    //마우스 오버 종료
    public void OnPointerExit(PointerEventData eventData)
    {
        SkillTooltip.Instance.Hide();
    }

    //크기 조절
    void Awake() {
        //하드코딩된 size 나중에 수정할 것
        skillImage = skillObject.GetComponent<Image>();
        skillImageRT = skillObject.GetComponent<RectTransform>();

        cooltimeImage = cooltimeObject.GetComponent<Image>();
        cooltimeImageRT = cooltimeObject.GetComponent<RectTransform>();

        skillImageRT.sizeDelta = layout.cellSize - new Vector2(1.5f, 1.5f);
        cooltimeImageRT.sizeDelta = layout.cellSize - new Vector2(1.5f, 1.5f);

        cooltimeCoroutine = ApplyCooltime();

        
    }

    Vector2 CalculatePivot(PointerEventData eventData) {
        int[] dx = new int[] {0, 1, 0, 1};
        int[] dy = new int[] {0, 0, 1, 1};

        Vector2 canvasSize = GameManager.Instance.GetCanvasSize();

        int k1 = eventData.position.x < canvasSize.x / 2 ? 0 : 1;
        int k2 = eventData.position.y < canvasSize.y / 2 ? 0 : 2;
        int k = k1 + k2;

        Debug.Log("위치: " + eventData.position);
        return new Vector2(dx[k], dy[k]);
    }

    // Start is called before the first frame update
    void Start()
    {
        

        
    }

    // Update is called once per frame
    void Update()
    {
        if (skill != null) {
            skillObject.SetActive(true);
            skillImage.sprite = skill.Icon;
        } else {
            skillObject.SetActive(false);
            skillImage.sprite = null;
        }

        ActiveSkill activeSkill = GetActiveSkill();

        if (activeSkill == null || activeSkill.Cooltime == 0)
            cooltimeImage.fillAmount = 0;
        else
            cooltimeImage.fillAmount = currentCooltime / activeSkill.Cooltime;
    }
}
