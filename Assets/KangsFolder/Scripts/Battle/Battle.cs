using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    //private GameObject P;
    //private string PlayerName;
    //private Text texts;
    //int q;

    //Common 폴더의 ->PrintPlayerInfo.cs의 클래스 선언
    PrintPlayerInfo PPI = new PrintPlayerInfo();

    // Start is called before the first frame update
    void Start()
    {
        PPI.SetPlayerInfo("PlayerInfo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
