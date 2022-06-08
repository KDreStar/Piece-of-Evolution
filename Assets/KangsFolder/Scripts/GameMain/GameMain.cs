using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMain : MonoBehaviour
{
    //private GameObject P;
    //private string PlayerName;
    //private Text texts; 
    //int a;
    //int q;

    // 사용자 이름 가져오기
    PrintPlayerInfo PPI = new PrintPlayerInfo();

    public void arB()
    {
        SceneManager.LoadScene("GameMain");
    }
    public void skillManage()
    {
        SceneManager.LoadScene("skillManage");
    }
    public void LoadBattleScene()
    {
        SceneManager.LoadScene("Battle");
    }

    public void ChangeScene(int selected)
    {
        switch (selected)
        {
            case 1: // Main으로 돌아가기
                Debug.Log("선택된 숫자(selected): " + selected);
                SceneManager.LoadScene("Main");
                break;
            case 2: // 몬스터 전투
                Debug.Log("선택된 숫자(selected): " + selected);
                SceneManager.LoadScene("MonsterBattle");
                break;
            case 3: // 스킬 관리
                Debug.Log("선택된 숫자(selected): " + selected);
                SceneManager.LoadScene("SkillSetting");
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
        //P = GameObject.Find("SelectPlayerManager");
        //a = P.GetComponent<SelectPlayer>().nowPlayer;
        ////Debug.Log("이전 Scene의 Object에서 가져온 nowPlayer 값: " + a);

        //// 사용자 이름 가져와서 넣기
        //q = P.GetComponent<SelectPlayer>().mouseSelect;
        //PlayerName = P.GetComponent<SelectPlayer>().Pname[q];
        
        //texts = GameObject.Find("PlayerName_Text").GetComponent<Text>();
        //texts.text = PlayerName;

        // 사용자 정보(이름) 출력
        //PPI.SetPlayerInfo("PlayerName_Text");
        PPI.SetPlayerInfo("PlayerInfo");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
