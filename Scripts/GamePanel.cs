using Assets.Scripts;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    public static GamePanel Instance;

    public Slider _hpSlider;//生命值滑动条
    public Slider _expSlider;//经验值
    public TextMeshProUGUI _money;//金钱
    public TextMeshProUGUI _grade;//等级
    public TextMeshProUGUI _hp;//生命值文本
    public TextMeshProUGUI _countDown;//倒计时
    public TextMeshProUGUI _waveCount;//波次
    private string MaxHp;

    private void Awake()
    {
        Instance = this;
        _hpSlider = GameObject.Find("HpSlider").GetComponent<Slider>();
        _expSlider = GameObject.Find("ExpSlider").GetComponent<Slider>();
        _money = GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>();
        _grade = GameObject.Find("ExpCount").GetComponent<TextMeshProUGUI>();
        _hp = GameObject.Find("HpCount").GetComponent<TextMeshProUGUI>();
        _countDown = GameObject.Find("CountDown").GetComponent<TextMeshProUGUI>();
        _waveCount = GameObject.Find("WaveCount").GetComponent<TextMeshProUGUI>();   
    }

    private void Start()
    {
        //初始化经验值
        RenewExp();
        //刷新生命值
        RenewHp();
        //刷新金钱
        RenewMoney();
        //刷新波次
        RenewWaveCount();
    }


    public void RenewExp()
    {
        _expSlider.value = GameManager.Instance.exp % 12/ 12;
        _grade.text = "Lv：" + (GameManager.Instance.exp / 12 + 1).ToString();
    }
  
    public void RenewHp()
    {
        _hp.text = GameManager.Instance.hp.ToString()+"/"+ GameManager.Instance.propData.maxHp.ToString();
        _hpSlider.value = GameManager.Instance.hp / (float)GameManager.Instance.propData.maxHp;
    }
    public void RenewMoney()
    {
        _money.text = GameManager.Instance.money.ToString();
    }
    //刷新倒计时
    public void RenewCountDown(float time)
    {
        _countDown.text = time.ToString("F0");
        //最后5秒，颜色变成红色
        if (time <= 5)
        {
            _countDown.color = new Color(255 / 255f, 0, 0);
        }
    }
    //刷新波次
    public void RenewWaveCount()
    {
        _waveCount.text ="第"+ GameManager.Instance.currentLevel.ToString()+"波";
    }

}
