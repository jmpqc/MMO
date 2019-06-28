using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        TargetAlterPositon.Instance.enabled = false; //当鼠标指针进入小地图时，将英雄的寻路目标target的变换位置功能变为禁用
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TargetAlterPositon.Instance.enabled = true; //当鼠标指针离开小地图时，将英雄的寻路目标target的变换位置功能变为启用
    }

    public void OnClick()
    {
        transform.parent.gameObject.SetActive(false);
        TargetAlterPositon.Instance.enabled = true; //当鼠标指针离开小地图时，将英雄的寻路目标target的变换位置功能变为启用
    }
}
