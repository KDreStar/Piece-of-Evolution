using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateC : MonoBehaviour
{

    public new Text name;
    public InputField inputField_ID;


    /// <summary>
    /// 로그인 버튼 클릭시 실행
    /// </summary>
    public void LoginButtonClick()
    {
        name.text = inputField_ID.text;
    }
}
