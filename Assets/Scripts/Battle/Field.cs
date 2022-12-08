using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public List<SkillEffect> skillEffects;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject upWall;
    public GameObject downWall;

    [HideInInspector]
    public Vector2 fieldSize;

    public void Add(SkillEffect skillEffect) {
        skillEffects.Add(skillEffect);
    }

    public void Remove(SkillEffect skillEffect) {
        skillEffects.Remove(skillEffect);
    }

    public List<SkillEffect> GetList() {
        return skillEffects;
    }

    public void ClearEffects() {
        Debug.Log("전체 사라짐" + skillEffects.Count);
        while (skillEffects.Count > 0) { 
            skillEffects[0].DestroySkillEffect();
        }
    }

    void Awake() {
        skillEffects = new List<SkillEffect>();

        float startX = leftWall.transform.localPosition.x + leftWall.transform.localScale.x / 2;
        float startY = downWall.transform.localPosition.y + downWall.transform.localScale.y / 2;

        float endX = rightWall.transform.localPosition.x - rightWall.transform.localScale.x / 2;
        float endY = upWall.transform.localPosition.y    - upWall.transform.localScale.y    / 2;

        float fieldWidth = endX - startX;
        float fieldHeight = endY - startY;

        Debug.Log("" + startX + " " + startY + " " + endX + " " + endY);

        fieldSize = new Vector2(fieldWidth, fieldHeight);
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
