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

    public void Toggle(GameObject status) {
        bool active = status.activeSelf;

        status.SetActive(!active);
    }

    // Update is called once per frame
    void Update()
    {
        if (textTime != null) {
            int t = (int)Mathf.Ceil(battleEnvController.MaxBattleTime - battleEnvController.timer);
            textTime.text = string.Format("<mspace=0.75em>{0}</mspace>", t);
        }

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
