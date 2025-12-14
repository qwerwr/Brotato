using Assets.Scripts;
using Assets.Scripts.Model;
using Random = UnityEngine.Random;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;

public class ShopPanel : MonoBehaviour
{
  public static ShopPanel Instance;

    public Button _startButton;//进入下一关按钮
    public Button _refreshButton;//刷新商店按钮
    public TMP_Text _shopText;//商店信息
    public TMP_Text _moneyText;//金币信息
    public TMP_Text _weaponTitle;//武器信息

    public Transform _attrLayout;//属性布局
    public Transform _propLayout;//道具布局
    public Transform _weaponsLayout;//武器布局
    public Transform _itemLayout;//选择道具布局

    public List<ItemData> props=new List<ItemData>();
    private void Awake()
    {
       Instance=this;
        _startButton=GameObject.Find("StartButton").GetComponent<Button>();
        _refreshButton= GameObject.Find("RefreshButton").GetComponent<Button>();
        _shopText=GameObject.Find("ShopText").GetComponent<TMP_Text>();
        _moneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
        _weaponTitle = GameObject.Find("WeaponTitle").GetComponent<TMP_Text>();
        _attrLayout = GameObject.Find("AttrLayout").transform;
        _propLayout= GameObject.Find("PropLayout").transform;
        _weaponsLayout= GameObject.Find("WeaponsLayout").transform;
        _itemLayout=GameObject.Find("ItemLayout").transform;
    }
    private void Start()
    {
        //商店信息
        _shopText.text = "商店（第" + (GameManager.Instance.currentLevel - 1) + "波)";
        //开始按钮
        _startButton.transform.GetChild(0).GetComponent<TMP_Text>().text =
            "出发（第" + (GameManager.Instance.currentLevel) + "波）";
        _startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("03-GamePlay");
        });
        //金币值
        _moneyText.text=GameManager.Instance.money.ToString();
        SetAttrUI();//设置属性UI

        //展示武器
        ShowCurrentWeapons();

        //展示道具
        ShowCurrentProp();
        //随机四个对象
        RandomProps();
        //展示在ui上
        ShowPropUI();
        //刷新物品
        _refreshButton.onClick.AddListener(() => {
            RefreshItem();
        } );

        //设置武器槽索引
        SetWeaponSlotIndex();
    }

    private void SetWeaponSlotIndex()
    {
       int count=_weaponsLayout.childCount;
        for (int i = 0; i < count; i++)
        {
            _weaponsLayout.GetChild(i).GetComponent<WeaponSlot>().slotCount = i;
        }
    }

    private void RefreshItem()
    {
        if (GameManager.Instance.money < 3) { return; }
        GameManager.Instance.money -= 3;//扣钱
        //更新金币UI
        _moneyText.text = GameManager.Instance.money.ToString();
        RandomProps();//重新刷新


    }

    /// <summary>
    /// 展示在ui上
    /// </summary>
    private void ShowPropUI()
    {
        int i = 0;
        foreach (ItemData item in props)
        {
            //设置数据
            _itemLayout.GetChild(i).GetComponent<ItemCardUI>().itemData = item;
            //设置透明度
            _itemLayout.GetChild(i).GetComponent<ItemCardUI>()._canvasGroup.alpha = 1;
            _itemLayout.GetChild(i).GetComponent<ItemCardUI>()._canvasGroup.interactable = true;//设置交互
            //设置名称
            _itemLayout.GetChild(i).GetChild(0).GetChild(1).GetComponent<TMP_Text>().text
                = item.name;
            //设置类型
            if(item is WeaponData)
            {
                _itemLayout.GetChild(i).GetChild(2).GetComponent<TMP_Text>().text = "武器";
                //设置头像
                _itemLayout.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite
                    = Resources.Load<Sprite>(item.avatar);
            }
            else
            {
                _itemLayout.GetChild(i).GetChild(2).GetComponent<TMP_Text>().text = "道具";
                //设置头像
                _itemLayout.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite
                    = GameManager.Instance.propsAtlas.GetSprite(item.name);
            }
            //设置描述
            _itemLayout.GetChild(i).GetChild(3).GetComponent<TMP_Text>().text = item.describe;
            //设置价格
            _itemLayout.GetChild(i).GetChild(4).GetChild(0).GetComponent<TMP_Text>().text
                =item.price.ToString();
            i++;
        }
    }

    private void RandomProps()
    {
        props.Clear();
        //获取要生成的道具数量
        int propCount = Random.Range(2, 4);
        //随机生成道具
        for (int i = 0; i < propCount; i++)
        {
            props.Add(GameManager.Instance.GetRandom(GameManager.Instance.propDatas) as ItemData);
        }
        //随机武器
        for (int i = 0; i <4- propCount; i++)
        {
            props.Add(GameManager.Instance.GetRandom(GameManager.Instance.weaponDatas) as ItemData);
        }

    }

    public void ShowCurrentWeapons()
    {
        int count = _weaponsLayout.childCount;
        for (int i = 0; i < count; i++)
        {
            WeaponSlot slot = _weaponsLayout.GetChild(i).GetComponent<WeaponSlot>();
            //设置ui,有武器
            if (i < GameManager.Instance.currentWeapons.Count)
            {
                //设置ui
                slot._weaponIcon.enabled = true;
                slot.weaponData = GameManager.Instance.currentWeapons[i];
                slot._weaponIcon.sprite =
                    Resources.Load<Sprite>(slot.weaponData.avatar);
                //判断等级，设置不同颜色
                switch (slot.weaponData.grade)
                {
                    case 1:
                        slot._weaponBG.color = new Color(29 / 255f, 29 / 255f, 29 / 255f);
                        break;
                    case 2:
                        slot._weaponBG.color = new Color(58 / 255f, 83 / 255f, 99 / 255f);
                        break;
                    case 3:
                        slot._weaponBG.color = new Color(79 / 255f, 58 / 255f, 99 / 255f);
                        break;
                    case 4:
                        slot._weaponBG.color = new Color(99 / 255f, 50 / 255f, 50 / 255f);
                        break;
                }
            }
            else//没武器,空格子
            {
               slot._weaponIcon.enabled = false;
                slot._weaponBG.color = new Color(29 / 255f, 29 / 255f, 29 / 255f);
                slot.weaponData = null;
            }
        }
        //武器数量显示
        _weaponTitle.text = "武器（" + GameManager.Instance.currentWeapons.Count +
            "/" + GameManager.Instance.propData.slot + "）";
    }

    private void ShowCurrentProp()
    {
     int count=_propLayout.childCount;
        for (int i = 0; i < count; i++)
        {
            //设置ui
            if (i < GameManager.Instance.currentProps.Count)
            {
                _propLayout.GetChild(i).GetChild(0).GetComponent<Image>().enabled = true;

                _propLayout.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                    GameManager.Instance.propsAtlas.GetSprite(GameManager.Instance.currentProps[i].name);
            }
            else
            {
                _propLayout.GetChild(i).GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
    }

    private void SetAttrUI()
    {
        //设置等级
        _attrLayout.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text=
            ((int)(GameManager.Instance.exp/12)).ToString();
        //设置最大生命值
        _attrLayout.GetChild(1).GetChild(2).GetComponent<TMP_Text>().text =
            ((GameManager.Instance.propData.maxHp)).ToString();
        //生命值再生
        _attrLayout.GetChild(2).GetChild(2).GetComponent<TMP_Text>().text =
         ((GameManager.Instance.propData.revive)).ToString();
        //近战伤害
        _attrLayout.GetChild(3).GetChild(2).GetComponent<TMP_Text>().text =
      ((GameManager.Instance.propData.short_damage)).ToString();
        //远程伤害
        _attrLayout.GetChild(4).GetChild(2).GetComponent<TMP_Text>().text =
      ((GameManager.Instance.propData.long_damage)).ToString();
        //暴击伤害
        _attrLayout.GetChild(5).GetChild(2).GetComponent<TMP_Text>().text =
      ((GameManager.Instance.propData.critical_strikes_probability)).ToString();
        //拾取范围
        _attrLayout.GetChild(6).GetChild(2).GetComponent<TMP_Text>().text =
      ((GameManager.Instance.propData.pickRange)).ToString();
        //速度
        _attrLayout.GetChild(7).GetChild(2).GetComponent<TMP_Text>().text =
      ((GameManager.Instance.propData.speedPer)).ToString();
        //收获
        _attrLayout.GetChild(8).GetChild(2).GetComponent<TMP_Text>().text =
      ((GameManager.Instance.propData.harvest)).ToString();
        //经验值倍率
        _attrLayout.GetChild(9).GetChild(2).GetComponent<TMP_Text>().text =
      ((GameManager.Instance.propData.expMuti)).ToString();
        //商店折扣
        _attrLayout.GetChild(10).GetChild(2).GetComponent<TMP_Text>().text =
      ((GameManager.Instance.propData.shopDiscount)).ToString();



    }
    //购物按钮功能
    public bool Shopping(ItemData itemData)
    {
        if (GameManager.Instance.money < 3)
        {
            return false;   
        }
        //武器是否超过6
        if(itemData is WeaponData&& GameManager.Instance.currentWeapons.Count >= GameManager.Instance.propData.slot)
        {
            return false;
        }
        //道具是否超过20个
        if (itemData is PropData && GameManager.Instance.currentProps.Count >=20)
        {
            return false;
        }
        GameManager.Instance.money -= itemData.price;//扣钱
        _moneyText.text = GameManager.Instance.money.ToString();
      
        //是武器
        if(itemData is WeaponData)
        {
            //问题：在购买后进行武器合成数据进而加合会污染从json读取出的原始weaponData数据
            //原因：currentWeapons是引用类型
            //解决方法：取出来后用个临时空间进行储存
            WeaponData tempItem = JsonConvert.DeserializeObject<WeaponData>(JsonConvert.SerializeObject(itemData));
            GameManager.Instance.currentWeapons.Add(tempItem);
            ShowCurrentWeapons();
        }
        else//是道具
        {
            PropData tempItem = JsonConvert.DeserializeObject<PropData>(JsonConvert.SerializeObject(itemData));
            GameManager.Instance.currentProps.Add(tempItem);
            ShowCurrentProp();
            //注入关联属性
            GameManager.Instance.FusionAttr(tempItem);
            //更新属性面板
            SetAttrUI();
        }
        return true;
    }
}
