using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    public Skill skill = null;
    private float currentCooltime;
    public float CurrentCooltime {
        get { return currentCooltime; }
    }

    public bool IsEmpty() {
        return skill != null ? true : false;
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

            yield return null;
        }

        ResetCooltime();
        Debug.Log("재사용 가능");
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
