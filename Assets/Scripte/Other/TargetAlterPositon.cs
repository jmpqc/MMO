using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetAlterPositon : MonoBehaviour
{
    private static TargetAlterPositon instance; //单例
    public static TargetAlterPositon Instance
    {
        get { return instance; }
        set { }
    }

    RaycastHit hitInfo; //射线信息
    Ray ray; //射线
    Transform hero;//英雄

    MeshRenderer circleRenderer; //表示目标点的圆圈的渲染器
    MeshRenderer arrowRenderer; //指示目标点的箭头的渲染器
    Texture2D[] circleTexture = new Texture2D[4]; //存放圆圈循环的4张图片
    Texture2D[] arrowTexture = new Texture2D[2]; //存放箭头循环的2张图片
    int circleNum;
    int arrowNum;

    Terrain ground; //地形


    // Use this for initialization
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        hero = GameObject.Find("Player").transform; //获得英雄物体
        PlayerController.Instance.nma = hero.GetComponent<NavMeshAgent>(); //获得英雄NavMeshAgent



        circleRenderer = transform.Find("TargetIconCircleIcon").GetComponent<MeshRenderer>(); //circle是target(this)的子物体
        arrowRenderer = circleRenderer.transform.Find("TargetIconArrowIcon").GetComponent<MeshRenderer>(); //arrow是circle的子物体

        //获取纹理
        circleTexture[0] = Resources.Load(@"Map\AimCircle0") as Texture2D;
        circleTexture[1] = Resources.Load(@"Map\AimCircle1") as Texture2D;
        circleTexture[2] = Resources.Load(@"Map\AimCircle2") as Texture2D;
        circleTexture[3] = Resources.Load(@"Map\AimCircle3") as Texture2D;

        arrowTexture[0] = Resources.Load(@"Map\AimArrow0") as Texture2D;
        arrowTexture[1] = Resources.Load(@"Map\AimArrow1") as Texture2D;

        circleNum = 0; //圆圈图片的循环变量
        arrowNum = 0; //箭头图片的循环变量

        InvokeRepeating("ChangeCirclePicture", 0.2f, 0.2f); //播放圆圈动画
        InvokeRepeating("ChangeArrowPicture", 0.3f, 0.3f); //播放箭头动画

        ground = GameObject.Find("TerrainVillage").GetComponent<Terrain>();
    }

    // Update is called once per frame
    void Update()
    {
        AlterTargetPosition(); //改变Target位置   
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
                if (hitInfo.collider.tag == "Ground")
                {
                    transform.position = hitInfo.point + new Vector3(0, 0.05f, 0); //目标点的移到鼠标点击的位置

                    PlayerController.Instance.HeroMoving(); //让英雄根据Target新的位置寻路

                    GetComponent<MeshRenderer>().enabled = true; //用图片显示位置
                }
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

            Vector3 v1 = new Vector3(transform.position.x, transform.position.y, transform.position.z); //从坐标原点到Target的向量
            Vector3 v2 = transform.right * typeH; //键盘水平方法移动的分量
            Vector3 v3 = transform.forward * typeV; //键盘垂直方向移动的分量
            Vector3 v = v1 + v2 + v3; //目标位置
            transform.position = Vector3.MoveTowards(transform.position, v, 2f);//移动Target


            PlayerController.Instance.HeroMoving();//让英雄根据Target新的位置寻路
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

    /// <summary>
    /// 显示圆圈动画
    /// </summary>
    void ChangeCirclePicture()
    {
        circleRenderer.material.mainTexture = circleTexture[circleNum++];
        circleNum = circleNum >= 4 ? 0 : circleNum;
    }
    /// <summary>
    /// 显示箭头动画
    /// </summary>
    void ChangeArrowPicture()
    {
        arrowRenderer.material.mainTexture = arrowTexture[arrowNum++];
        arrowNum = arrowNum >= 2 ? 0 : arrowNum;
    }

    /// <summary>
    /// 根据鼠标在地图上点击的位置，改变Target的位置
    /// </summary>
    /// <param name="p">比例数值</param>
    public void FollowMap(object p)
    {
        
        float x = ground.terrainData.size.x * ((Vector2)p).x;//根据比例求出坐标
        float z = ground.terrainData.size.z * ((Vector2)p).y;//
        transform.position = new Vector3(x, transform.position.y, z); //鼠标点击地图的点，转换成实际地形上的坐标
        PlayerController.Instance.HeroMoving(); //让英雄根据Target新的位置寻路
    }

}
