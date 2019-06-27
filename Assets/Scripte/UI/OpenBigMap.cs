using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OpenBigMap : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {
    GameObject canvas; //Hierarchy视图的Canvas对象
    Transform bigMap; //大地图，点击中地图后弹出大地图

    // Use this for initialization
    void Start () {
        canvas = GameObject.Find("Canvas"); //获取Canvas对象
        bigMap = canvas.transform.Find("BigMap"); //获取大地图对象
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        bigMap.gameObject.SetActive(true); //当鼠标点击小地图时，弹出大地图
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TargetAlterPositon.Instance.enabled = false; //当鼠标指针进入小地图时，将英雄的寻路目标target的变换位置功能变为禁用
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TargetAlterPositon.Instance.enabled = true; //当鼠标指针离开小地图时，将英雄的寻路目标target的变换位置功能变为启用
    }
}
