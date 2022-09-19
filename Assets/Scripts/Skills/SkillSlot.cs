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
    public Image image;
    public Image cooltimeImage;

    //true = 드래그 가능 false = 드래그 불가능
    public bool dragable;

    //true 장착중인 스킬
    //false 인벤토리에 있는 스킬
    public bool isEquipSkill;

    //0~7
    public int equipIndex;

    //스킬 툴팁 시작점을 알려줌
    public Vector2 tooltipPivot;

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
    public bool UseSkill(GameObject attacker, int direction=3) {
        ActiveSkill activeSkill = GetActiveSkill();

        if (activeSkill == null)
            return false;
        
        //코루틴 종료 직전 -> UseSkill 실행 -> 쿨타임 다시 0 초과됨 -> 코루틴 2개 돌아감
        //이 현상 방지
        if (isUse == false)
            return false;

        //바라보는 방향으로 스킬 생성
        // 1 = ↑ //
        Vector3 angle = new Vector3(0, 0, 135 - direction * 45);
  
        //스킬 관리 인스턴스에서 이펙트 빌림
        //그후 위치 세팅
        SkillEffect skillEffect = Managers.Pool.GetSkillEffect(activeSkill.Effect);
        skillEffect.gameObject.tag = attacker.tag + "Skill";

        skillEffect.transform.position = attacker.transform.position;
        skillEffect.transform.rotation = activeSkill.Effect.transform.rotation * Quaternion.Euler(angle);
        skillEffect.transform.parent = attacker.transform.parent.transform;

        skillEffect.Initialize();

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

        if (dragSkillSlot == null)
            return;

        SkillController.Instance.MoveSkill(this);
    }

    //마우스 오버시
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skill != null) {
            SkillTooltip.Instance.Show(skill, transform.position, tooltipPivot);
        }
            
    }

    //마우스 오버 종료
    public void OnPointerExit(PointerEventData eventData)
    {
        SkillTooltip.Instance.Hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        cooltimeCoroutine = ApplyCooltime();
        image = GetComponent<Image>();

        if (skill != null)
            image.sprite = skill.Icon;
    }

    // Update is called once per frame
    void Update()
    {
        if (skill != null)
            image.sprite = skill.Icon;
        else
            image.sprite = null;

        ActiveSkill activeSkill = GetActiveSkill();

        if (activeSkill == null || activeSkill.Cooltime == 0)
            cooltimeImage.fillAmount = 0;
        else
            cooltimeImage.fillAmount = currentCooltime / activeSkill.Cooltime;
    }
}
