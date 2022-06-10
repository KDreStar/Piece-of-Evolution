using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

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

    //적의 정보를 세팅
    //학습 버튼을 클릭한 경우 mlagents-learn.exe 매개변수 관리후 실행
    //없는 경우 실행 불가
    public void BattleSetting(bool isLearning, GameObject enemy) {
        if (isLearning) {

        }

        this.isLearning = isLearning;

        EnemyData.Instance.Set(enemy);

        //오래걸리므로 나중에 로딩씬 추가
        GameManager.Instance.ChangeScene("Battle");
    }

    public void EndBattle() {
        if (isLearning) {
            //mlagents-learn 종료
        }

        //GameManager.Instance.ChangeScene("Result");        
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
