using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.MLAgents;

public class ResultUI : MonoBehaviour
{
    public TextMeshProUGUI textMessage;

    // Start is called before the first frame update
    void Start()
    {
        textMessage.text = BattleManager.Instance.result;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
