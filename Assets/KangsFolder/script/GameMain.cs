using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMain : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
