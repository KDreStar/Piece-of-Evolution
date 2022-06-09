using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public bool isLearning; //true면 학습용도 전투 false면 1번 전투후 결과

    private static BattleManager instance = null;
    public static BattleManager Instance {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    //클릭한 적의 정보를 배틀 화면에 전달
    public void BattleSetting(bool isLearning, GameObject enemy) {
        this.isLearning = isLearning;

        EnemyData.Instance.Set(enemy);

        GameManager.Instance.ChangeScene("Battle");
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
