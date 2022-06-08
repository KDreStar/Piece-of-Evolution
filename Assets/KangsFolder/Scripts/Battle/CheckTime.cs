using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CheckTime : MonoBehaviour
{
    public Text[] ClockText;
    float time;
    int minute;
    int second;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        ClockText[0].text = ((int)time / 3600).ToString();
        ClockText[1].text = ((int)time / 60 % 60).ToString();
        ClockText[2].text = ((int)time % 60).ToString();
    }
}
