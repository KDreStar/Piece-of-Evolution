using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class SkillTooltip : MonoBehaviour
{
    private static SkillTooltip instance = null;
    public static SkillTooltip Instance {
        get { return instance; }
    }

    private RectTransform rectTransform;

    public TextMeshProUGUI textNo;
    public TextMeshProUGUI textCost;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDescription;
    public TextMeshProUGUI textBaseCooltime;

    public void Show(Skill skill, Vector2 pos, Vector2 pivot) {
        transform.position = pos;
        rectTransform.pivot = pivot;

        gameObject.SetActive(true);

        textNo.text = "No." + skill.No.ToString();
        textCost.text = skill.Cost.ToString();
        textName.text = skill.Name;
        textDescription.text = skill.Description;

        if (skill is ActiveSkill) {
            ActiveSkill activeSkill = skill as ActiveSkill;

            //textBaseCooltime.SetActive(true);
            textBaseCooltime.text = string.Format("쿨타임 {0}s", activeSkill.Cooltime);
        } else {
            textBaseCooltime.text = "패시브 스킬";
        }
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        rectTransform = GetComponent<RectTransform>();

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
