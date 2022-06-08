using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Skill skill = null;
    private float currentCooltime;
    public float CurrentCooltime {
        get { return currentCooltime; }
    }

    public Image image;

    public bool isEquipSkill;
    public int equipIndex;

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
        
        if (currentCooltime > 0)
            return false;

        activeSkill.Use(gameObject, direction);
        currentCooltime = activeSkill.BaseCooltime;
        StartCoroutine(ApplyCooltime());

        return true;
    }

    public void ResetCooltime() {
        currentCooltime = 0;
    }

    IEnumerator ApplyCooltime() {
        while (currentCooltime > 0) {
            currentCooltime -= Time.deltaTime;
            if (skill.No == 16)
                Debug.Log("슬래시쿨" + currentCooltime);

            yield return null;
        }

        ResetCooltime();
        Debug.Log("재사용 가능");
    }

    public void OnBeginDrag(PointerEventData eventData){
        if (IsEmpty())
            return;
        
        DragSkillSlot.Instance.SetSkillSlot(this);
        DragSkillSlot.Instance.transform.position = eventData.position;
    }

    // 마우스 드래그 중일 때 계속 발생하는 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        if (IsEmpty())
            return;
        
        DragSkillSlot.Instance.transform.position = eventData.position;
    }

    // 마우스 드래그가 끝났을 때 발생하는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSkillSlot.Instance.Clear();
    }

    // 드롭 했을때
    public void OnDrop(PointerEventData eventData) {
        SkillSlot dragSkillSlot = DragSkillSlot.Instance.skillSlot;

        if (dragSkillSlot == null)
            return;

        SkillManager.Instance.MoveSkill(this);
    }

    // Start is called before the first frame update
    void Start()
    {
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
    }
}
