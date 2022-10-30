using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFactory
{
    public EnemyAI Create(string name) {
        switch (name) {
            case "ScarecrowAI":
                return new ScarecrowAI();

            case "Scarecrow2AI":
                return new Scarecrow2AI();
        }

        return null;
    }
}
