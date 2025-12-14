using Assets.Scripts;
using Assets.Scripts.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiffcuitySelectPanel : MonoBehaviour
{
   public static DiffcuitySelectPanel Instance; // 单例
    public CanvasGroup _canvasGroup;//画布组透明度
    public Transform _difficultyConent;//难度内容父物体

 

    public GameObject difficulty_prefab;//难度预制体
    public Transform _difficultyList;//难度列表父物体
    internal Image _difficultyAvatar;
    internal TextMeshProUGUI _difficultyName;
    internal TextMeshProUGUI _difficultyDescribe;

    private void Awake()
    {
        Instance = this;
        _canvasGroup = GetComponent<CanvasGroup>();
        _difficultyConent=GameObject.Find("DiffcuityContent").transform;//获取难度内容父物体

       
        //获取难度详情UI组件
        _difficultyList = GameObject.Find("DiffcuityList").transform;//获取难度列表父物体    
        difficulty_prefab=Resources.Load<GameObject>("Prefabs/Diffcuity");//加载难度预制体
        
        _difficultyAvatar = GameObject.Find("Avatar_Diffcuity").GetComponent<Image>();
        _difficultyName = GameObject.Find("DiffcuityName").GetComponent<TextMeshProUGUI>();
        _difficultyDescribe = GameObject.Find("DiffcuityDescribe").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        //实例化难度预制体
        foreach (var data in GameManager.Instance.difficultyDatas)
        {
            GameObject difficultyItem = Instantiate(difficulty_prefab, _difficultyList);
            DiffcuityUI diffcuityUI = difficultyItem.GetComponent<DiffcuityUI>();
            diffcuityUI.SetData(data);
        }
    }
}
