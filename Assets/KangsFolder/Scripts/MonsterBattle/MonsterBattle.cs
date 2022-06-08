using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MonsterBattle : MonoBehaviour
{
    public void ChangeScene(int selected)
    {
        switch (selected)
        {
            case 1: // 이전 화면(GameMain)으로 돌아가기
                Debug.Log("선택된 숫자(selected): " + selected);
                SceneManager.LoadScene("GameMain");
                break;
            case 2: // 몬스터 전투
                Debug.Log("선택된 숫자(selected): " + selected);
                SceneManager.LoadScene("MonsterBattle");
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
