using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform center; //摄像机围绕的中心
    float distanceToCenter; //摄像机与中心之间的距离
    float disFactor; //距离伸缩因子

    Vector3 tempVector; //临时向量，每一帧计算后，作为改变摄像机位置参数
    float HorizontalAngle; //临时向量绕Y轴旋转的角度(累积)
    float VerticalAngle; //临时向量绕X轴旋转的角度(累积)

    float speedY; //绕Y轴旋转的速度
    float speedX; //绕X轴旋转的速度

    void Start()
    {
        center = GameObject.Find("CenterAroundTheCamera").transform; //获得摄像机旋转参照中心的transform
        speedY = 8f; //横向旋转速度系数
        speedX = 5f; //纵向旋转速度系数
        distanceToCenter = 5f;//距离
        disFactor = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera(); //旋转摄像机
        ChangeDistance();//改变摄像机与中心的距离
    }

    void RotateCamera()
    {
        tempVector = Vector3.forward; //每一帧的开始，将临时向量归到初始位置
        center.forward = Vector3.forward; //每一帧的开始，将中心z轴正方向归到初始位置

        if (Input.GetMouseButton(1))
        {
            HorizontalAngle = HorizontalAngle + Input.GetAxis("Mouse X") * speedY; //将鼠标右键在横向上的拖动距离（经过计算）累加到横向上的旋转角度
            VerticalAngle = VerticalAngle + Input.GetAxis("Mouse Y") * speedX; //将鼠标右键在纵向上的拖动距离（经过计算）累加到纵向上的旋转角度
        }

        tempVector = Quaternion.AngleAxis(HorizontalAngle, Vector3.up) * tempVector; //横向旋转临时向量
        center.forward = Quaternion.AngleAxis(HorizontalAngle, Vector3.up) * center.forward; //横向旋中心z轴正向向量

        //限制纵向（累积）角度值
        if (VerticalAngle < -80f) VerticalAngle = -80f; Debug.Log(VerticalAngle);
        if (VerticalAngle > 20f) VerticalAngle = 20f;
        tempVector = Quaternion.AngleAxis(VerticalAngle, center.right) * tempVector; //纵向旋转临时向量

        transform.position = center.position + (tempVector.normalized * distanceToCenter); //依据中心位置、临时向量、和距离值改变摄像机的位置

        //下面计算摄像机的朝向
        Quaternion q1 = Quaternion.identity;
        //计算出摄像机旋转四元数的绝对值
        q1.SetLookRotation(center.position - transform.position, center.up); //绕着中心的Y轴，参摄像机位置指向中心点位置的向量为参数，计算旋转值

        transform.rotation = q1;//摄像机旋转
    }

    void ChangeDistance()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            distanceToCenter -= disFactor;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            distanceToCenter += disFactor;
        }
    }
}
