using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.MLAgents;

public class BattleUI : MonoBehaviour
{
    public TextMeshProUGUI textTime;
    public BattleEnvController battleEnvController;

    public TextMeshProUGUI textMessage;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("배틀 시작");
        Managers.Battle.StartBattle();
    }

    // Update is called once per frame
    void Update()
    {
        if (textTime != null)
            textTime.text = Mathf.Ceil(battleEnvController.MaxBattleTime - battleEnvController.timer).ToString();

        float counter = Managers.Battle.startCounter;

        if (counter > 0) {
            if (counter >= 1) {
                textMessage.text = Mathf.Floor(counter).ToString();
            } else {
                textMessage.text = "Start!";
            }
        } else {
            textMessage.text = "";
        }

        if (Managers.Battle.isLearning) {
            if (Academy.Instance != null) {
                textMessage.text = Academy.Instance.StepCount.ToString();
            }
        }
    }
}
