using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//스킬 드래그시 임시로 마우스에 나오는 것
public class DragSkillSlot : MonoBehaviour
{
    private static DragSkillSlot instance = null;
    public static DragSkillSlot Instance {
        get { return instance; }
    }
    public SkillSlot skillSlot;

    public Image image;

    public void SetSkillSlot(SkillSlot skillSlot) {
        this.skillSlot = skillSlot;

        gameObject.SetActive(true);
    }

    public void Clear() {
        this.skillSlot = null;

        gameObject.SetActive(false);
    }

    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        image.sprite = skillSlot.skill.Icon;
    }
}
