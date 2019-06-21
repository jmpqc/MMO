using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using System;

public class PlayerMove : MonoBehaviour
{
    GameObject target; //英雄当前的移到目标位置
    GameObject hero; //当前英雄
    Animator animator; //英雄的动画
    float moveSpeed; //角色移动速度
    float rotateSpeed; //角色旋转速度
    HeroInfo heroInfo; //存储从json文件读取的角色信息
    string path; //角色配置文件的加载路径
                 // Use this for initialization
    void Start()
    {
        PathSetting();//设置配置文件的路径
        FileInfo fileInfo = new FileInfo(path); //读取文件的属性信息
        if (fileInfo.Exists)
        {
            string jsonText = File.ReadAllText(path); //读取json文件内容
            heroInfo = JsonUtility.FromJson<HeroInfo>(jsonText); //将json内容转化成HeroInfo类的对象


            ////////////////////////////////////////这里应该改成（根据json内容和预置体）实例化英雄对象
            hero = GameObject.Find(heroInfo.sex);
            //////////////////////////////////////------------------------
            
        }

        target = GameObject.Find("PlayerMoveTarget");
        animator = hero.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HeroMoving(); //英雄移动位置
    }

    /// <summary>
    /// 当前采用NavMesh寻路，当前英雄会根据destination的改变而开始寻路
    /// </summary>
    void HeroMoving()
    {
        if (target)
        {
            GetComponent<NavMeshAgent>().destination = target.transform.position; //设置寻路目标
        }
    }

    void PathSetting()
    {
#if UNITY_EDITOR

        path = @"C:\Users\dream\Desktop\UnityProjects\MMO\Configuration\hero.json";

#else

            path="";

#endif

    }
}
[Serializable]
public class HeroInfo
{
    public string sex;
}
