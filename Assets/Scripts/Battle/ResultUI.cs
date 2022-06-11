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
        textMessage.text = BattleManager.Instance.result;

        if (BattleManager.Instance.getSkill == null) {
            skillSlot.gameObject.SetActive(false);
        } else {
            skillSlot.AddSkill(BattleManager.Instance.getSkill);
            textSkillMessage.text = BattleManager.Instance.judgeMessage;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
