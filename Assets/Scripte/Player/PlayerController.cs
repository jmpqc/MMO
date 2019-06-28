using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using System;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance; //单例字段
    public static PlayerController Instance //单例属性
    {
        get { return instance; }
        set { }
    }

    GameObject target; //英雄当前的移到目标位置
    GameObject hero; //当前英雄，通过预置体实例化，与当前脚本一同桂在 Player物体上
    Animator animator; //英雄的动画
    HeroInfo heroInfo; //存储从json文件读取的角色信息
    public NavMeshAgent nma; //角色身上的NavMeshAgent组件
    string path; //角色配置文件的加载路径

    List<Transform> MaleEquipments = new List<Transform>(); //角色的装备
                 // Use this for initialization
    private void Awake()
    {
        instance = this; //初始化单例字
    }
    void Start()
    {
        target = GameObject.Find("PlayerMoveTarget");//英雄在寻路追踪的目标物体
        PathSetting();//设置配置文件的路径
        FileInfo fileInfo = new FileInfo(path); //读取文件的属性信息
        if (fileInfo.Exists) //如果json配置文件存在，读取并设置属性信息
        {
            string jsonText = File.ReadAllText(path); //读取json文件内容
            heroInfo = JsonUtility.FromJson<HeroInfo>(jsonText); //将json内容转化成HeroInfo类的对象


            if (heroInfo.sex == "male")
            {
                ////////////////////////////////////////这里应该改成（根据json内容和预置体）实例化英雄对象
                hero = GameObject.Find(heroInfo.sex);
                //////////////////////////////////////------------------------

                GetComponent<NavMeshAgent>().enabled = false; //只有NavMeshAgent禁用了之后，角色才依据position的数值到达正确位置
                transform.position = new Vector3(heroInfo.position[0], heroInfo.position[1], heroInfo.position[2]);//从json中读取英雄的位置信息
                transform.rotation = new Quaternion(heroInfo.rotation[0], heroInfo.rotation[1], heroInfo.rotation[2], heroInfo.rotation[3]); //从json中读取英雄的旋转信息
                target.transform.position = this.transform.position; //移动目标物体要与英雄位置重合
                GetComponent<NavMeshAgent>().enabled = true; //启用NavMeshAgent，恢复导航功能

                //获得角色装备
                foreach (Transform sub in transform.Find("male"))
                {
                    if (sub.name.ToLower().Contains("male_warrior"))
                    {
                        MaleEquipments.Add(sub);
                    }
                }

            }
            else //另一种性别代表另一种职业
            {

            }

        }


        nma = GetComponent<NavMeshAgent>();
        animator = hero.GetComponent<Animator>(); //获得动画控制器
    }

    // Update is called once per frame
    void Update()
    {
        if (nma.remainingDistance > 0)
        {
            target.transform.position = nma.path.corners[nma.path.corners.Length - 1];
        }
        ///下面是设置动画
        if (nma.remainingDistance != 0)
        {
            AnimationSetHeroRun();
        }
        else
        {
            AnimationSetHeroIdle();
        }
    }

    /// <summary>
    /// 当前采用NavMesh寻路，当前英雄会根据destination的改变而开始寻路
    /// </summary>
    public void HeroMoving()
    {
        nma.SetDestination(target.transform.position); //设置寻路目标
    }



    void PathSetting()
    {
#if UNITY_EDITOR

        path = @"C:\Users\dream\Desktop\UnityProjects\MMO\Configuration\hero.json";

#else

        path="";

#endif

    }

    /// <summary>
    /// 模型运行Idle动画
    /// </summary>
    void AnimationSetHeroIdle()
    {
        animator.SetBool("MOVING", false);
        animator.SetBool("CASTING", false);
        animator.SetBool("DEAD", false);

        foreach(Transform equipment in MaleEquipments)
        {
            equipment.GetComponent<Animator>().SetBool("MOVING", false);
            equipment.GetComponent<Animator>().SetBool("CASTING", false);
            equipment.GetComponent<Animator>().SetBool("DEAD", false);
        }
    }
    /// <summary>
    /// 模型运行Run动画
    /// </summary>
    void AnimationSetHeroRun()
    {
        animator.SetBool("MOVING", true);
        foreach (Transform equipment in MaleEquipments)
        {
            equipment.GetComponent<Animator>().SetBool("MOVING", true);
        }
    }

    /// <summary>
    /// 游戏结束时将角色信息写入Json
    /// </summary>
    private void OnDisable()
    {
        //将游戏结束时，角色的位置信息存入josn
        heroInfo.position[0] = hero.transform.position.x;
        heroInfo.position[1] = hero.transform.position.y;
        heroInfo.position[2] = hero.transform.position.z;
        //将游戏结束时，角色的旋转信息存入josn
        heroInfo.rotation[0] = hero.transform.rotation.x;
        heroInfo.rotation[1] = hero.transform.rotation.y;
        heroInfo.rotation[2] = hero.transform.rotation.z;
        heroInfo.rotation[3] = hero.transform.rotation.w;
        string json = JsonUtility.ToJson(heroInfo, true); //将对象转换成字符串
        File.WriteAllText(path, json, System.Text.Encoding.UTF8);
    }
    
}



[Serializable]
public class HeroInfo //与json对应的类，读取角色性别、位置和旋转等信息
{
    public string sex;
    public float[] position;
    public float[] rotation;
}


