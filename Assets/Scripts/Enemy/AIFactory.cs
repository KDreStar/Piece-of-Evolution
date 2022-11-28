using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFactory
{
    public EnemyAI Create(string name) {
        switch (name) {
            case "Scarecrow":
                return new Scarecrow1AI();

            case "Scarecrow2":
                return new Scarecrow2AI();

            case "Scarecrow3":
                return new Scarecrow3AI();

            case "Slime":
                return new SlimeAI();
        }

        return null;
    }
}
