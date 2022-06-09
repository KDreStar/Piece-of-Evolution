using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class BattleUI : MonoBehaviour
{
    public TextMeshProUGUI textTime;
    public BattleEnvController battleEnvController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (textTime != null)
            textTime.text = Mathf.Ceil(battleEnvController.MaxBattleTime - battleEnvController.timer).ToString();
    }
}
