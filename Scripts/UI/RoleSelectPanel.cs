using Assets.Scripts;
using Assets.Scripts.Model;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleSelectPanel : MonoBehaviour
{
    public static RoleSelectPanel Instance; // 单例
 
    public Transform _roleList;// 角色列表父物体
    public GameObject role_prefab;// 角色预制体

    public TextMeshProUGUI _roleName;//角色名称文本
    public Image _avatar;//角色头像图片
    public TextMeshProUGUI _roleDescribe;//角色描述文本
    public TextMeshProUGUI _text3;//解锁条件文本

    public CanvasGroup _canvasGroup;//画布组透明度
    public GameObject _roleDetails;//角色详情面板
    public CanvasGroup _contentcanvasGroup;//画布组透明度
    private void Awake()
    {
        Instance = this;
        _roleList=GameObject.Find("RoleList").transform;//获取角色列表父物体
        role_prefab =Resources.Load<GameObject>("Prefabs/Role");//加载角色预制体
       
        _roleName =GameObject.Find("RoleName").GetComponent<TextMeshProUGUI>();//获取角色名称文本
        _avatar =GameObject.Find("Avatar_Role").GetComponent<Image>();//获取角色头像图片
     
        _roleDescribe =GameObject.Find("RoleDescribe").GetComponent<TextMeshProUGUI>();//获取角色描述文本
        _text3 =GameObject.Find("Text3").GetComponent<TextMeshProUGUI>();//获取解锁条件文本
       _roleDetails=GameObject.Find("RoleDetails");//获取角色详情面板
        _canvasGroup = GetComponent<CanvasGroup>();//获取画布组组件
        _contentcanvasGroup= GameObject.Find("RoleContent").GetComponent<CanvasGroup>();//获取内容画布组组件
    }

    private void Start()
    {
       
        foreach (RoleData roleData in GameManager.Instance.roleDatas)
        {
            
            RoleUI r=Instantiate(role_prefab,_roleList).GetComponent<RoleUI>();//实例化角色预制体
        
            r.SetRoleUI(roleData);//设置角色UI
        }
    }
}
