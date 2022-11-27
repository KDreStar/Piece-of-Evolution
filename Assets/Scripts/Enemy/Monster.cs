using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda.ONNX;
using Unity.Barracuda;

[CreateAssetMenu(menuName = "Monster")]
public class Monster : ScriptableObject
{
    [SerializeField]
    private string name;
    public string Name {
        get { return name; }
    }
    
    [SerializeField]
    private float baseHP;
    public float BaseHP {
        get { return baseHP; }
    }

    [SerializeField]
    private float baseATK;
    public float BaseATK {
        get { return baseATK; }
    }

    [SerializeField]
    private float baseDEF;
    public float BaseDEF {
        get { return baseDEF; }
    }

    [SerializeField]
    private float baseSPD;
    public float BaseSPD {
        get { return baseSPD; }
    }

    [SerializeField]
    private Skill[] skills = new Skill[8];
    public Skill[] Skills {
        get { return skills; }
    }

    [SerializeField]
    private string fileName;
    public string FileName {
        get { return fileName; }
    }

    [SerializeField]
    private bool isOnnx;
    public bool IsOnnx {
        get { return isOnnx; }
    }

    [SerializeField]
    private string[] fields;
    public string[] Fields {
        get { return fields; }
    }

    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite {
        get { return sprite; }
    }
}
