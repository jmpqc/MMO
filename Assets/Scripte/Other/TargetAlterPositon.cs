using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAlterPositon : MonoBehaviour
{
    RaycastHit hitInfo; //射线信息
    Ray ray; //射线
    float moveCoefficient; //Target移动系统
    Transform hero;//英雄
    // Use this for initialization
    void Start()
    {
        hero = GameObject.Find("Player").transform; //获得英雄物体
        moveCoefficient = 3f; //移动系数的初值
    }

    // Update is called once per frame
    void Update()
    {
        AlterTargetPosition(); //改变Target位置
    }


    /// <summary>
    /// 根据鼠标点击或键盘方向键改变Target的位置，从而使用Player产生移到
    /// </summary>
    void AlterTargetPosition()
    {
        //鼠标操作
        if (Input.GetMouseButtonDown(0)) //点击鼠标左键时按点击位置移到Target
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition); //发出射线
            if (Physics.Raycast(ray, out hitInfo, 1000)) //判断射线是否穿过了某个物体
            {
                transform.position = hitInfo.point + new Vector3(0, 0.02f, 0); //目标点的移到鼠标点击的位置
                GetComponent<MeshRenderer>().enabled = true; //用图片显示位置
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            GetComponent<MeshRenderer>().enabled = false; //隐藏位置
        }


        //键盘操作
        if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) //Input.GetAxis(“Horizontal")
        {
            float typeH = Input.GetAxis("Horizontal");
            float typeV = Input.GetAxis("Vertical");
            //根据Player的位置和键盘的方向键改变Target的位置
            transform.position = new Vector3(hero.position.x + typeH * moveCoefficient, hero.position.y, hero.position.z + typeV * moveCoefficient);
        }
    }

}
