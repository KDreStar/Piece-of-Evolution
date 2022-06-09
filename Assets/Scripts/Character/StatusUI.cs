using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System;
using TMPro;


public class StatusUI : MonoBehaviour
{
    public EquipSkills equipSkills;
    public Status status;

    public TextMeshProUGUI textName;
    public TextMeshProUGUI textCurrentHP;
    public TextMeshProUGUI textMaxHP;
    public TextMeshProUGUI textATK;
    public TextMeshProUGUI textDEF;
    public TextMeshProUGUI textSPD;
    public TextMeshProUGUI textCost;

    public Image HPBar;

    public string textHPFormat;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (textName != null)
            textName.text = status.name.ToString();

        if (textMaxHP != null)
            textMaxHP.text = status.MaxHP.ToString();

        if (textCurrentHP != null)
            textCurrentHP.text = status.CurrentHP.ToString();
        
        if (textATK != null)
            textATK.text = status.CurrentATK.ToString();

        if (textDEF != null)
            textDEF.text = status.CurrentDEF.ToString();

        if (textSPD != null)
            textSPD.text = status.CurrentSPD.ToString();

        if (textCost != null)
            textCost.text = equipSkills.CurrentSkillCost.ToString();

        if (HPBar != null)
            HPBar.fillAmount = (status.CurrentHP / status.MaxHP);
    }
}
