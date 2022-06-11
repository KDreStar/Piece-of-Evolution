using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// DB에서 캐릭터 정보 받아옴.
public class SelectPlayer : MonoBehaviour
{

    public static SelectPlayer Instance;
    // 다음 씬에서도 SelectPlayerManager 오브젝트 사용할 수 있도록
    public GameObject DontDestroy_Object;
    private void Awake()
    {
        DontDestroy_Object = GameObject.Find("SelectPlayerManager");
        if (Instance != null)
        {
            Destroy(DontDestroy_Object); // 이미 오브젝트가 존재하면
            Instance = this; // 삭제하고 다시 생성
            DontDestroyOnLoad(DontDestroy_Object);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(DontDestroy_Object);
    }

    new Text name;
    private GameObject P;
    
    public int maxPlayer = 5;
    public int nowPlayer = 0;
    public int maxView = 3;
    public int mouseSelect = -1;
    int t;

    int InputButtonCount = 0;

    public InputField inputField_Value;

    // DB에서 가져온 캐릭터 정보 (+status 등) 현재는 테스트
    //int[,] arr = new int[2, 3] { { 1, 2, 3 }, { 4, 5, 6 } };
    //

    public string[,] Pname = new string[5, 5] // 행=캐릭터수, 열=캐릭터 보유 정보
    {   // 캐릭이름, hp,   dmg,   def,  spd
        {"가져옴1", "500", "51", "11", "5" },
        {"가져옴2", "450", "52", "12", "5" },
        {"가져옴3", "400", "53", "13", "5" },
        {"가져옴4", "350", "54", "14", "5" },
        {"가져옴5", "300", "55", "15", "5" }
    };

    //public string[] Pname = new string[5]
    //{ "가져옴1",
    //    "가져옴2", //1
    //    "가져옴3", //2
    //    "",  //3
    //    "" }; //4
    ////
    //public int[] Php = new int[5]
    //{
    //    550,
    //    500,
    //    450,
    //    400,
    //    350
    //};

    //public int[] Php = new int[5]
    //{
    //    550,
    //    500,
    //    450,
    //    400,
    //    350
    //};

    //public int[] Php = new int[5]
    //{
    //    550,
    //    500,
    //    450,
    //    400,
    //    350
    //};


    //P1.transform.Find("name1").gameObject.SetActive(false); // 자식접근
    // Start is called before the first frame update
    public void InputPname(string[,] nameArr, int start)
    {
        int k = 0;

        for (t = start; t < maxView+start; t++)
        {
            
            P = GameObject.Find("Player_name" + (k+1));
            name = P.GetComponent<Text>();
            //name.text = nameArr[t];
            name.text = nameArr[t, 0];

            k += 1;
        }
    }

    public void firstStartButtonPrint()
    {
        if (nowPlayer > maxView)
            GameObject.Find("Canvas").transform.Find("RightScroll_Button").gameObject.SetActive(true);
    }
    
    public void leftButton()
    {
        //nowPlayerCount();

        InputButtonCount += -1;
        Debug.Log("왼쪽!" + InputButtonCount);
        InputPname(Pname, InputButtonCount);

        CheckButtonPrint();
    }

    public void rightButton()
    {
        //nowPlayerCount();

        InputButtonCount += 1;
        Debug.Log("오른쪽!" + InputButtonCount);
        InputPname(Pname, InputButtonCount);

        CheckButtonPrint();
    }

    public void CheckButtonPrint()
    {
        Debug.Log("CBP: " + InputButtonCount + " " + nowPlayer);
        if (InputButtonCount > 0) // 왼쪽으로 갈 횟수가 남았다면
        {
            GameObject.Find("Canvas").transform.Find("LeftScroll_Button").gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("Canvas").transform.Find("LeftScroll_Button").gameObject.SetActive(false);
        }

        if (InputButtonCount < (nowPlayer - maxView)) // 오른쪽으로 갈 횟수가 남았다면
        {
            GameObject.Find("Canvas").transform.Find("RightScroll_Button").gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("Canvas").transform.Find("RightScroll_Button").gameObject.SetActive(false);
        }
    }

    public void Chch()
    {
        //nowPlayerCount();
        Debug.Log("CBP: " + InputButtonCount + " " + nowPlayer);
        //Debug.Log(Pname[0] + Pname[1] + Pname[2] + Pname[3] + Pname[4]);
        Debug.Log(nowPlayer);
    }

    public void OffScroll()
    {
        GameObject.Find("Canvas").transform.Find("LeftScroll_Button").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("RightScroll_Button").gameObject.SetActive(false);
    }

    public void CreateP(InputField inputField_Value)
    {
        Debug.Log("CreateP");

        if (inputField_Value.text == "") // 캐릭터 이름이 공백일경우
        {
            GameObject.Find("CreatePlayer_Button").transform.Find("NullNameAlert_PopUp").gameObject.SetActive(true);
            return;
        }

        if (nowPlayer == maxPlayer) // 캐릭슬롯 최대인 경우
        {
            Debug.Log("캐릭 못만듬");
            inputField_Value.text = ""; //입력필드 값 공백으로 초기화

            //경고 화면 출력
            GameObject.Find("CreatePlayer_Button").transform.Find("MaxCreateAlert_PopUp").gameObject.SetActive(true);
            return;
        }
        string[,] temp = new string[5, 5];
        System.Array.Copy(Pname, 0, temp, 1, nowPlayer);

        temp[0, 0] = inputField_Value.text; // 새로넣을 캐릭 이름
        //Debug.Log("te생성끝 : " + temp[0] + temp[1] + temp[2] + temp[3] + temp[4]);


        System.Array.Copy(temp, Pname, nowPlayer+1);

        //Debug.Log("Pn생성끝 : " + Pname[0] + Pname[1] + Pname[2] + Pname[3] + Pname[4]);
        nowPlayer += 1;

        Debug.Log("nowP? : " + nowPlayer);

        InputPname(Pname, 0);
        CheckButtonPrint();

        inputField_Value.text = ""; //생성후에 입력필드 값 공백으로 초기화
    }

    protected void nowPlayerCount()
    {
        nowPlayer = 0;
        for (t = 0; t < maxPlayer; t++)
        {
            //if (Pname[t] != "")
            if (Pname[t, 0] != "")
            {
                nowPlayer += 1;
            }
        }
        Debug.Log("현재플레이어:" + nowPlayer);
    }

    

    public void SelectedPlayer(int selected)
    {
        mouseSelect = selected + InputButtonCount;
        //Debug.Log(Pname[mouseSelect]);
    }

    public void ChangeScene(int selected)
    {
        switch (selected)
        {
            case 1: // 게임 접속
                SceneManager.LoadScene("GameMain");
                /* 캐릭터 선택해야 진입하는 부분
                Debug.Log("mouseSelect: " + mouseSelect);
                if (mouseSelect == -1)
                {
                    GameObject.Find("StartGame_Button")
                        .transform.Find("SelectPlayerAlert_PopUp")
                        .gameObject.SetActive(true);
                    OffScroll();
                }
                else
                {
                    SceneManager.LoadScene("GameMain");
                    Debug.Log("선택된 숫자(selected): " + selected);
                }
                */
                break;
            case 2: // 이전 화면
                SceneManager.LoadScene("Main");
                Debug.Log("선택된 숫자(selected): " + selected);
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

    void Start()
    {
        mouseSelect = -1;
        Debug.Log("첫시작 mouseSelect: " + mouseSelect);
        
        //InputPname(Pname, 0);

        //현재 플레이어 수 구하기

        //nowPlayerCount();
        nowPlayerCount();
        firstStartButtonPrint();

        Debug.Log("CBP: " + InputButtonCount + " " + nowPlayer + maxPlayer);
        //Debug.Log(Pname[0] + Pname[1] + Pname[2] + Pname[3] + Pname[4]);
        Debug.Log(nowPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
