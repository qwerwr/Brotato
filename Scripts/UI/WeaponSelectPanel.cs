using Assets.Scripts;
using Assets.Scripts.Model;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectPanel : MonoBehaviour
{
  public static WeaponSelectPanel Instance; // 单例
    public CanvasGroup _canvasGroup;//画布组透明度
    public Transform _weaponContent;//武器内容父物体
    public CanvasGroup _weaponDetailsCanvasGroup;//画布组透明度

    
    public GameObject weapon_prefabs;//武器预制体
    public Transform _weaponlist;//武器列表父物体

    public Image  _weaponAvatar;//武器头像图片
    public TextMeshProUGUI _weaponName;//武器名称文本
    public TextMeshProUGUI _weaponType;//武器类型文本
    public TextMeshProUGUI _weaponDescribe;//武器描述文本

    public GameObject _weaponDetails;//武器详情面板文本

    private void Awake()
    {
        Instance = this;
        _canvasGroup = GetComponent<CanvasGroup>();
        _weaponContent=GameObject.Find("WeaponContent").transform;//获取武器内容父物体

      

        //生成武器预制体
        weapon_prefabs = Resources.Load<GameObject>("Prefabs/Weapon");
        _weaponlist=GameObject.Find("WeaponList").transform;//获取武器列表父物体

        _weaponAvatar=GameObject.Find("Avatar_Weapon").GetComponent<Image>();//获取武器头像图片
        _weaponName=GameObject.Find("WeaponName").GetComponent<TextMeshProUGUI>();//获取武器名称文本
        _weaponType=GameObject.Find("WeaponType").GetComponent<TextMeshProUGUI>();//获取武器类型文本
        _weaponDescribe =GameObject.Find("WeaponDescribe").GetComponent<TextMeshProUGUI>();//获取武器描述文本
        _weaponDetails =GameObject.Find("WeaponDetails");//获取武器详情面板文本
        _weaponDetailsCanvasGroup=GameObject.Find("WeaponDetails").GetComponent<CanvasGroup>();//获取内容画布组组件
    }
    private void Start()
    {
        foreach(var weaponData in GameManager.Instance.weaponDatas)
        {
            WeaponUI w=Instantiate(weapon_prefabs, _weaponlist).GetComponent<WeaponUI>();
            w.SetData(weaponData);
        }
    }
}
