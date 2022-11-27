using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBManager
{
    private SkillDatabase skillDB;
    public SkillDatabase SkillDB {
        get {return skillDB; }
    }

    private MonsterDatabase monsterDB;
    public MonsterDatabase MonsterDB {
        get {return monsterDB; }
    }

    private AIFactory aiFactory;
    public AIFactory AIFactory {
        get {return aiFactory; }
    }

    public void Init() {
        skillDB = new SkillDatabase();
        monsterDB = new MonsterDatabase();
        aiFactory = new AIFactory();
    }
}
