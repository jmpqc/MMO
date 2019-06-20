using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAlterPositon : MonoBehaviour
{
    RaycastHit hitInfo; //射线信息
    Ray ray; //射线
    Transform hero;//英雄
    // Use this for initialization
    void Start()
    {
        hero = GameObject.Find("Player").transform; //获得英雄物体
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
            transform.position = hero.position; //让Target从英雄的位置开始出发，改变位置

            //第一个参数是摄像机自身坐标系的right向量，第二个参数是摄像机位置指向地面的向量
            Vector3 projectX = VectorProject(Camera.main.transform.right, new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z) - Camera.main.transform.position);
            transform.right = projectX.normalized; //目标物体与摄像机投影的方向一致
            //根据摄像机的方向和键盘方向盘改变Target的位置

            Vector3 v1= new Vector3(transform.position.x, transform.position.y, transform.position.z); //从坐标原点到Target的向量
            Vector3 v2 = transform.right * typeH; //键盘水平方法移动的分量
            Vector3 v3 = transform.forward * typeV; //键盘垂直方向移动的分量
            Vector3 v = v1 + v2 + v3; //目标位置
            transform.position = Vector3.MoveTowards(transform.position, v, 2f);//移动Target

        }
    }

    /// <summary>
    /// 求向量平面上的投影
    /// </summary>
    /// <param name="vector">向量</param>
    /// <param planeNormal="vector">平面的法线</param>
    /// <returns>投影向量</returns>
    Vector3 VectorProject(Vector3 vector, Vector3 planeNormal)
    {
        Vector3 response = Vector3.ProjectOnPlane(vector, planeNormal); //求得投影
        return response; //返回值
    }

}
