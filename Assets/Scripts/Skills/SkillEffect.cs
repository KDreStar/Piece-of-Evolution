using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    public GameObject caster;
    public Status status;

    public void Init(GameObject character) {
        caster = character;

        status = caster.GetComponent<Status>();
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
