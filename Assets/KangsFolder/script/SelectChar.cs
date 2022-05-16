using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; //�ؽ�Ʈ ǥ���� ���� ��ũ��Ʈ

public class SelectChar : MonoBehaviour
{
    public new Text name;

    public int mouseSelect = 0;

    //ĳ���͸� ������ ���
    public void firstSelect()
    {
        mouseSelect = 1;
        Debug.Log(mouseSelect);
    }
    public void secondSelect()
    {
        mouseSelect = 2;
        Debug.Log(mouseSelect);
    }
    public void thirdSelect()
    {
        mouseSelect = 3;
        Debug.Log(mouseSelect);
    }

    //�ٸ� ����� ������ ���
    public void deleteC()
    {
        mouseSelect = 4;
        Debug.Log(mouseSelect);
    }
    public void CreateC()
    {
        mouseSelect = 5;
        Debug.Log(mouseSelect);
    }

    public void ChangeScene()
    {
        switch (mouseSelect)
        {
            case 1:
                SceneManager.LoadScene("GameMain");
                Debug.Log(mouseSelect);
                break;
            case 2:
                SceneManager.LoadScene("GameMain");
                Debug.Log(mouseSelect);
                break;
            case 3:
                SceneManager.LoadScene("GameMain");
                Debug.Log(mouseSelect);
                break;
            case 4:
                Debug.Log(mouseSelect);
                break;
            case 5:
                Debug.Log(mouseSelect);
                break;

        }
    }


    // Start is called before the first frame update
    void Start()
    {
        name = GameObject.Find("name1").GetComponent<Text>();
        name.text = "1 ĳ�� �̸�";

        name = GameObject.Find("name2").GetComponent<Text>();
        name.text = "2 ĳ�� �̸�";

        name = GameObject.Find("name3").GetComponent<Text>();
        name.text = "3 ĳ�� �̸�";

        

        mouseSelect = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
