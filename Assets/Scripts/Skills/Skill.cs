using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SkillData")]
public class Skill : ScriptableObject
{
    [SerializeField]
    private int no;
    public int No {
        get { return no; }
    }

    [SerializeField]
    private string name;
    public string Name {
        get { return name; }
    }
    
    [SerializeField]
    [TextArea]
    private string description;
    public string Description {
        get { return description; }
    }

    [SerializeField]
    private int cost;
    public int Cost {
        get { return cost; }
    }

    [SerializeField]
    private Effect[] effects;
    public Effect[] Effects {
        get { return effects; }
    }

    [SerializeField]
    private Sprite icon;
    public Sprite Icon {
        get { return icon; }
    }
}
