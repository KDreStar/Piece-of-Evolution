using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintPlayerInfo
{
    private GameObject PlayerObj;
    private string[] PlayerInfo = new string[5];
    
    private int selectedNum;
    private Text texts;

    private int t, k; //반복문용 변수.

    public void SetPlayerInfo(string PlayerInfo_obj)
    {
        PlayerObj = GameObject.Find("SelectPlayerManager");
        Debug.Log("1");
        // 사용자 이름 가져와서 넣기
        selectedNum = PlayerObj.GetComponent<SelectPlayer>().mouseSelect;
        Debug.Log("2");

        for (t = 0; t < 5; t++)
        {
            PlayerInfo[t] = PlayerObj.GetComponent<SelectPlayer>().Pname[selectedNum, t];

        }

        for (k = 0; k < 5; k++)
        {
            texts = GameObject.Find(PlayerInfo_obj).transform.GetChild(k).gameObject.GetComponent<Text>();
            texts.text = PlayerInfo[k];
        }
         
    }

}
