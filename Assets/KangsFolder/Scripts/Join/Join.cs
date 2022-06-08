using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Join : MonoBehaviour
{
    public GameObject JoinView;

    
    public InputField InputFieldID;
    public InputField InputFieldName;
    public InputField InputFieldPW;

    public Button CommitButton;

	string LoginURL = "http://localhost/Joindb.php";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CommitButtonClick()
    {
        StartCoroutine(RegisterUser(InputFieldID.text, InputFieldName.text, InputFieldPW.text));
    }

    IEnumerator RegisterUser(string ID, string Name, string password)
	{
		WWWForm form = new WWWForm ();
        
		form.AddField ("IDPost", ID);
        form.AddField("usernamePost", Name);
		form.AddField ("passwordPost", password);

		WWW www = new WWW (LoginURL, form);

		yield return www;
		Debug.Log (www.text);

	}
}
