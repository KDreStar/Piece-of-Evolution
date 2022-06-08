using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkillSetting : MonoBehaviour
{
    // 사용하는 슬롯 초기화(Drag.cs에서 사용)
    public int[] useSlot = new int[3];
    private int i;

    PrintPlayerInfo PPI = new PrintPlayerInfo();

    public void ChangeScene(int selected)
    {
        switch (selected)
        {
            case 1: // 이전 화면(GameMain)으로 돌아가기
                Debug.Log("선택된 숫자(selected): " + selected);
                SceneManager.LoadScene("GameMain");
                break;
            case 2: // 
                Debug.Log("선택된 숫자(selected): " + selected);
                //SceneManager.LoadScene("MonsterBattle");
                break;
            case 3: // 
                Debug.Log("선택된 숫자(selected): " + selected);
                break;
            case 4: // 
                Debug.Log("선택된 숫자(selected): " + selected);
                break;
            case 5: // 
                Debug.Log("선택된 숫자(selected): " + selected);
                break;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (i = 0; i < 3; i++)
        {
            useSlot[i] = 0;
        }

        // 사용자 정보(이름) 출력
        PPI.SetPlayerInfo("PlayerName_Text");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
