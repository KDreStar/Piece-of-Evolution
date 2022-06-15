using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Skill skill = null;
    private float currentCooltime;
    public float CurrentCooltime {
        get { return currentCooltime; }
    }

    public Image image;
    public Image cooltimeImage;

    public bool dragable;
    public bool isEquipSkill;
    public int equipIndex;
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

    /////
    public bool UseSkill(GameObject gameObject, int direction=3) {
        ActiveSkill activeSkill = GetActiveSkill();

        if (activeSkill == null)
            return false;
        
        //코루틴 종료 직전 -> UseSkill 실행 -> 쿨타임 다시 0 초과됨 -> 코루틴 2개 돌아감
        //이 현상 방지
        if (isUse == false)
            return false;

        activeSkill.Use(gameObject, direction);
        isUse = false;
        currentCooltime = activeSkill.BaseCooltime;
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

        SkillManager.Instance.MoveSkill(this);
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

        ActiveSkill temp = GetActiveSkill();

        if (temp == null)
            cooltimeImage.fillAmount = 0;
        else
            cooltimeImage.fillAmount = currentCooltime / temp.BaseCooltime;
    }
}
