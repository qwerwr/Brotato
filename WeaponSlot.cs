using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour,IPointerClickHandler
{
    public WeaponData weaponData;//数据
    public Image _weaponIcon;//图标
    public Image _weaponBG;//背景色
    public int slotCount;//槽位索引

    private void Awake()
    {
        _weaponIcon=transform.GetChild(1).GetComponent<Image>();
        _weaponBG=transform.GetChild(0).GetComponent<Image>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //右键点击
        if (eventData.button==PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
        //左键点击
        if (eventData.button == PointerEventData.InputButton.Left) {
            OnLeftClick();
        }
    }
    //左键合成
    private void OnLeftClick()
    { if (weaponData == null) { return; }//空的
        for (int i = 0; i < GameManager.Instance.currentWeapons.Count; i++)
        {
            //循环搜索到自己了
            if (i == slotCount) {
                continue;//跳过当次循环
            }
            //判断id和等级是否相等
            if (weaponData.id == GameManager.Instance.currentWeapons[i].id
                && weaponData.grade == GameManager.Instance.currentWeapons[i].grade)
            {
               
                GameManager.Instance.currentWeapons[i].grade += 1;//被点击的武器进行升级
                GameManager.Instance.currentWeapons[i].price *= 2;//价格翻倍

                GameManager.Instance.currentWeapons.RemoveAt(i);//删除GM中的数据
                ShopPanel.Instance.ShowCurrentWeapons();//更新武器ui

                break;
            }

            
        }
    
    }
    //右键出售
    private void OnRightClick()
    {
        //如果当前插槽没有武器
        if (weaponData == null) { return; }
        GameManager.Instance.money += (int)(weaponData.price * 0.5f);//折损一半卖出
       ShopPanel.Instance._moneyText.text=GameManager.Instance.money.ToString();//更新金币ui
        weaponData = null;//数据清空
        _weaponIcon.enabled = false;//关闭图标
        GameManager.Instance.currentWeapons.RemoveAt(slotCount);//删除GM中的数据
        ShopPanel.Instance.ShowCurrentWeapons();
    }

  
}
