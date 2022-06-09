using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

public class EnemyStatus : MonoBehaviour
{
    public Skill[] skillList = new Skill[8];
    public NNModel model;

    public float HP;
    public float ATK;
    public float DEF;
    public float SPD;

    public Sprite image;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
