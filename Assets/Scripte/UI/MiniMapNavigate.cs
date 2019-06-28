using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniMapNavigate : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    Transform pointOnMap; //鼠标在MiniMap上点击后，PointClickOnMap物体要移动到鼠标点击的位置，所以point相当于是鼠标点击的位置
    float mapRectX;  //MiniMap在X方向上的尺寸
    float mapRectY;  //MiniMap在Y方向上的尺寸

    MeshRenderer circleRenderer; //表示目标点的圆圈的渲染器
    MeshRenderer arrowRenderer; //指示目标点的箭头的渲染器


    // Use this for initialization
    void Start()
    {
        pointOnMap = GameObject.Find("PointClickOnMap").transform;
        //地图尺寸
        mapRectX = GetComponent<RectTransform>().sizeDelta.x;
        mapRectY = GetComponent<RectTransform>().sizeDelta.y;

        circleRenderer = GameObject.Find("TargetIconCircleIcon").GetComponent<MeshRenderer>(); //circle是target(this)的子物体
        arrowRenderer = circleRenderer.transform.Find("TargetIconArrowIcon").GetComponent<MeshRenderer>(); //arrow是circle的子物体


    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance.nma.remainingDistance > 0) //物体激活的时候会显示动画，在寻路的过程中显示动画
        {
            circleRenderer.enabled = true;
            arrowRenderer.enabled = true;
        }
        else
        {
            circleRenderer.enabled = false;
            arrowRenderer.enabled = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointOnMap.position = eventData.position; //将PointClickOnMap物体移动到鼠标点击的位置
        float proportionX = pointOnMap.localPosition.x / mapRectX; //每次点击后，鼠标在地图上点击的位置的X坐标占地图X轴向尺寸的百分比
        float proprotionY = pointOnMap.localPosition.y / mapRectY; //每次点击后，鼠标在地图上点击的位置的Y坐标占地图Y轴向尺寸的百分比
        Vector2 p = new Vector2(proportionX, proprotionY); //将两个方向的百分比放在一个Vector2变量中
        TargetAlterPositon.Instance.FollowMap(p); //鼠标在地图上点击后，Player根据位置开始寻路
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
