using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearningButton : MonoBehaviour
{
    public GameObject character;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => BattleManager.Instance.BattleSetting(true, character));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
