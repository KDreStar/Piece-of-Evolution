using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//매니저들을 모은곳
public class Managers : MonoBehaviour
{
    private static Managers instance = null;
    public static Managers Instance {
        get { return instance; }
    }

    private DataManager data = new DataManager();
    public static DataManager Data {
        get { return Instance.data; }
    }

    private BattleManager battle = new BattleManager();
    public static BattleManager Battle {
        get { return Instance.battle; }
    }

    private PoolManager pool = new PoolManager();
    public static PoolManager Pool {
        get { return Instance.pool; }
    }

    //싱글톤
    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
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
