using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public static Main Instance;
    public GameObject DontDestroy_Object;
    private void Awake()
    {

        DontDestroy_Object = GameObject.Find("SelectPlayerManager");
        if (DontDestroy_Object != null)
        {
            Destroy(DontDestroy_Object); // 이미 오브젝트가 존재하면
        }
    }

    public void SceneChangeToLogin()
    {
        SceneManager.LoadScene("Login");
    }
    public void StartGame()
    {
        SceneManager.LoadScene("SelectCharacter");
    }
    public void HowToPlay()
    {
        //SceneManager.LoadScene("Ho");
    }
    public void Option()
    {
        //SceneManager.LoadScene("SelectPlayer");
    }
    public void Credit()
    {
        //SceneManager.LoadScene("SelectPlayer");
    }
    public void Exit()
    {
        Debug.Log("Exit1");

        #if (UNITY_EDITOR)
        Debug.Log("Exit2");
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        Debug.Log("Exit3");
        Application.Quit();
            
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
