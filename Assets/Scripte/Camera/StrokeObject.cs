using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrokeObject : MonoBehaviour
{
    //射线变量
    RaycastHit hitInfo; //射线信息
    Ray ray; //射线

    //材质变量
    //npc描边材质
    public Material npcFeetStroke;
    public Material npcBeardStroke;
    public Material npcHandsStroke;
    public Material npcHeadStroke;
    public Material npcOutfitStroke;
    //npc正常材质
    public Material npcFeet;
    public Material npcBeard;
    public Material npcHands;
    public Material npcHead;
    public Material npcOutfit;






    //控制变量
    //npc控制变量
    bool isNpcStroke = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition); //发出射线
        if (Physics.Raycast(ray, out hitInfo, 1000)) //判断射线是否穿过了某个物体
        {
            if (hitInfo.transform.name == "npc")
            {
                if (isNpcStroke == false)
                {
                    AddStrokeNpc();
                    isNpcStroke = true;
                }
            }
            else
            {
                if (isNpcStroke == true)
                {
                    RemoveStrokeNpc();
                    isNpcStroke = false;
                }
            }




        }
    }

    void AddStrokeNpc()
    {
        foreach (Transform sub in hitInfo.transform)
        {
            SkinnedMeshRenderer meshRenderer = sub.GetComponent<SkinnedMeshRenderer>();
            switch (sub.name)
            {
                case "beard":
                    meshRenderer.material = npcBeardStroke;
                    break;
                case "feet":
                    meshRenderer.material = npcFeetStroke;
                    break;
                case "hands":
                    meshRenderer.material = npcHandsStroke;
                    break;
                case "head":
                    meshRenderer.material = npcHeadStroke;
                    break;
                case "outfit":
                    meshRenderer.material = npcOutfitStroke;
                    break;

            }
        }
    }
    void RemoveStrokeNpc()
    {
        Transform npc = GameObject.Find("npc").transform;
        foreach (Transform sub in npc)
        {
            SkinnedMeshRenderer meshRenderer = sub.GetComponent<SkinnedMeshRenderer>();
            switch (sub.name)
            {
                case "beard":
                    meshRenderer.material = npcBeard;
                    break;
                case "feet":
                    meshRenderer.material = npcFeet;
                    break;
                case "hands":
                    meshRenderer.material = npcHands;
                    break;
                case "head":
                    meshRenderer.material = npcHead;
                    break;
                case "outfit":
                    meshRenderer.material = npcOutfit;
                    break;

            }
        }
    }
}
