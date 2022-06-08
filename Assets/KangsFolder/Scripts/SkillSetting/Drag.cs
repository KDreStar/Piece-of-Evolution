using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// GameObject.Find("Canvas").transform.Find("RightScroll_Button").GetComponent<Animator>()
// GameObject.Find("PlayerSkill"+i).transform.GetChild(i-1).gameObject;
public class Drag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    public static Vector2 DefaultPos; // 드래그 시작 시 초기 위치 저장 변수
    
    private GameObject[] Slot = new GameObject[3]; // 게임 시작 시 Player의 Slot 저장 변수

    public SkillSetting Setting; // SkillSetting Class의 useSlot 변수 사용 위함.

    int i; // 반복문용 변수

    // 드래그 시작 시(클릭 시)
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        DefaultPos = this.transform.position;
    }

    // 드래그 도중, Vector2로 드래그 중인 위치를 가져와 오브젝트의 위치로 지정
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 currentPos = eventData.position;
        this.transform.position = currentPos;
    }

    public bool CheckBoxIn(GameObject target, PointerEventData eventData)
    {
        int rangeNum = 40;

        //Debug.Log("target.transform.position.x: " + target.transform.position.x);
        //Debug.Log("target.transform.position.y: " + target.transform.position.y);

        if (target.transform.position.x - rangeNum <= eventData.position.x &&
            eventData.position.x <= target.transform.position.x + rangeNum &&
            target.transform.position.y - rangeNum <= eventData.position.y &&
            eventData.position.y <= target.transform.position.y + rangeNum)
        {
            return true;
        }
        return false;  
    }

    public bool CheckBoxOut(GameObject target, PointerEventData eventData)
    {
        int rangeNum = 40;

        //Debug.Log("target.transform.position.x: " + target.transform.position.x);
        //Debug.Log("target.transform.position.y: " + target.transform.position.y);

        if (!(target.transform.position.x - rangeNum <= eventData.position.x &&
            eventData.position.x <= target.transform.position.x + rangeNum &&
            target.transform.position.y - rangeNum <= eventData.position.y &&
            eventData.position.y <= target.transform.position.y + rangeNum))
        {
            return true;
        }
        return false;
    }

    // 드래그를 끝냈을 때
    private GameObject temp; // 삭제할 게임 오브젝트를 담기 위함
    public int[] BoxIndex = new int[] {0, 0, 0}; // PlayerSkill에서 오브젝트 사라지게할 때의 조건용 변수
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        for (i = 1; i < 4; i++)
        {
            if (Setting.useSlot[i - 1] == 0) // 슬롯을 사용하지 않고 있을 경우
            {
                if (CheckBoxIn(Slot[i - 1], eventData) == true)
                {
                    Setting.useSlot[i - 1] = 1; // 사용한 자리는 더이상 사용 못하게
                                                
                    BoxIndex[i - 1] = i; // 사용할 박스 위치 설정
                    //생성(GameObject, 위치, 회전, 부모)
                    Instantiate(this, 
                        Slot[i - 1].transform.position, 
                        Quaternion.identity, 
                        Slot[i - 1].transform);
                    BoxIndex[i - 1] = 0; 
                    this.transform.position = DefaultPos;

                    break;
                }
            }
            else
            {
                this.transform.position = DefaultPos;
            }

            // 슬롯을 사용하고, 박스의 인덱스도 일치해야 함
            if (Setting.useSlot[i - 1] == 1 && BoxIndex[i - 1] == i)
            {
                if (CheckBoxOut(Slot[i - 1], eventData) == true)
                {
                    Setting.useSlot[i - 1] = 0; // 사용한 자리 초기화
                    // GameObject.Find("Canvas").transform.Find("RightScroll_Button").GetComponent<Animator>()
                    temp = GameObject.Find("PlayerSkill" + i)
                        .transform.GetChild(0)
                        .gameObject;
                    Destroy(temp); //자기 삭제

                    break;
                }
            }
            else
            {
                this.transform.position = DefaultPos;
            }
        }

        
        //for (i = 1; i < 4; i++)
        //{

        //}

        Debug.Log("드래그 끝: " + eventData.position);
        Debug.Log("드래그 끝, useSlot: " + Setting.useSlot[0] + Setting.useSlot[1] + Setting.useSlot[2]);

    }

    void Start()
    {

        Setting = GameObject.Find("SkillSettingManager").GetComponent<SkillSetting>();

        for (i=1; i<4; i++)
        {
            Slot[i-1] = GameObject.Find("PlayerSkill" + i);
        }

    }
    void Update()
    {
        

    }
}