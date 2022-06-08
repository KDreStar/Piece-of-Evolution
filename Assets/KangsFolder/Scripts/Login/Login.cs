using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

using UnityEngine.UI;


public class Login : MonoBehaviour {

    public GameObject LoginView;
    
	public InputField InputFieldID;
    public InputField InputFieldPW;
    public Button LoginButton;

	string LoginURL = "http://localhost/logindb.php";

	public static string SessionID;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		
	}

    public void LoginButtonClick()
    {
        StartCoroutine(LoginToDB(InputFieldID.text, InputFieldPW.text));

		SessionID = InputFieldID.text;

		Debug.Log( "Login_Test"+ SessionID);
    }

	public void LoadGame(string LoginCorrect)
    {
    //   if(LoginCorrect == "login success")
	   //{
		  // SceneManager.LoadScene("Fight");
	   //}

		//테스트를 위해 그냥 넘어가도록
		SceneManager.LoadScene("SelectPlayer");
	}

	IEnumerator LoginToDB(string ID, string password)
	{
		WWWForm form = new WWWForm ();
		form.AddField ("IDPost", ID);
		form.AddField ("passwordPost", password);

		WWW www = new WWW (LoginURL, form);
		yield return www;
		
		Debug.Log (www.text);

		string LoginCorrectData = www.text;

		LoadGame(LoginCorrectData);
	}

	

}