using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniMapNavigate : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler{
    Transform pointOnMap; //鼠标在MiniMap上点击后，PointClickOnMap物体要移动到鼠标点击的位置，所以point相当于是鼠标点击的位置
    float mapRectX;  //MiniMap在X方向上的尺寸
    float mapRectY;  //MiniMap在Y方向上的尺寸
    // Use this for initialization
    void Start () {
        pointOnMap = GameObject.Find("PointClickOnMap").transform;
        //地图尺寸
        mapRectX = GetComponent<RectTransform>().sizeDelta.x;
        mapRectY = GetComponent<RectTransform>().sizeDelta.y;


    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        pointOnMap.position = eventData.position; //将PointClickOnMap物体移动到鼠标点击的位置
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
