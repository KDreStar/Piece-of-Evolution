using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;

public class BattleManager : MonoBehaviour
{
    public bool isLearning; //true면 학습용도 전투 false면 1번 전투후 결과

    private static BattleManager instance = null;
    public static BattleManager Instance {
        get { return instance; }
    }

    public float startCounter;
    public string result;

    public string judgeMessage;
    public Skill getSkill;

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
        this.isLearning = isLearning;
        
        EnemyData.Instance.Set(enemy);

        CharacterData.Instance.Save();
        EnemyData.Instance.Save();

        if (isLearning) {


            //빈 파일을 생성함
            //열때 빈파일이 있으면 바로 러닝씬으로감
            //유니티 종료시 빈 파일 삭제
            GameManager.Instance.CreateLearningFile();

            string path = Application.persistentDataPath + "/";
            string arg = path + "Character.yaml "
                       + "--run-id=" + "Character "
                       + "--env=Piece-of-Evolution "
                       + "--num-envs=4 "
                       + "--width=480 "
                       + "--height=270 ";

            if (Directory.Exists("results/Character") == true)
                arg += "--resume ";

            Process.Start("mlagents-learn", arg);
            //GameManager.Instance.ChangeScene("Learning");
        } else {
            CharacterData.Instance.Load();
            EnemyData.Instance.Load();

            Time.timeScale = 0;
            GameManager.Instance.ChangeScene("Battle");
        }
    }

    public void StartBattle() {
        if (isLearning == false) {
            Time.timeScale = 0;
            startCounter = 4.0f;
            StartCoroutine(DecreaseCount());
        } else {
            //Time.timeScale = 1.0f;
        }
    }

    public void StartLearning() {
        isLearning = true;
        CharacterData.Instance.Load();
        EnemyData.Instance.Load();

        GameManager.Instance.ChangeScene("Learning");
    }

    IEnumerator DecreaseCount() {
        while (startCounter > 1) {
            startCounter -= Time.unscaledDeltaTime;
            yield return null;
        }

        //움직일 수 있음
        Time.timeScale = 1.0f;

        while (startCounter > 0) {
            startCounter -= Time.unscaledDeltaTime;
            yield return null;
        }

        //끝
    }

    public void EndBattle() {
        if (isLearning) {
            //mlagents-learn 종료
        }

        getSkill = null;
        getSkill = GameManager.Instance.JudgeGetSkill();

        if (getSkill != null) {
            judgeMessage = "스킬 획득!";
            SkillInventoryData.Instance.AddSkill(getSkill);
        } else {
            judgeMessage = "스킬 획득 실패...";
        }

        GameManager.Instance.ChangeScene("Result");        
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
