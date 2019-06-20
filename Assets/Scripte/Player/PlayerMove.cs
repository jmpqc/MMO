using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    GameObject target; //英雄当前的移到目标位置
    GameObject hero; //当前英雄
    Animator animator; //英雄的动画
    float moveSpeed; //角色移动速度
    float rotateSpeed; //角色旋转速度
                       // Use this for initialization
    void Start()
    {
        //////////////////////////////////////这里后面要改成从JSON加载
        hero = GameObject.Find("male");
        //////////////////////////////////////------------------------
        target = GameObject.Find("PlayerMoveTarget");
        animator = hero.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HeroMoving(); //英雄移动位置
    }

    void HeroMoving()
    {
        if (target)
        {
            GetComponent<NavMeshAgent>().destination = target.transform.position; //设置寻路目标
        }
    }
}
