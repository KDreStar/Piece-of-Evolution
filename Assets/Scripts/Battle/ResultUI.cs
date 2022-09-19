using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.MLAgents;

public class ResultUI : MonoBehaviour
{
    public TextMeshProUGUI textMessage;
    public TextMeshProUGUI textSkillMessage;
    public SkillSlot skillSlot;

    // Start is called before the first frame update
    void Start()
    {
        textMessage.text = Managers.Battle.result;

        if (Managers.Battle.getSkill == null) {
            skillSlot.gameObject.SetActive(false);
        } else {
            skillSlot.AddSkill(Managers.Battle.getSkill);
            textSkillMessage.text = Managers.Battle.judgeMessage;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
