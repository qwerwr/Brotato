using Assets.Scripts;
using Assets.Scripts.Model;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoleUI : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public Image _backImage;//背景图片
    public Image _avatarImage;//头像图片
    public Button _button;//按钮
    public RoleData roleData;

    private void Awake()
    {
        _backImage = GetComponent<Image>();
        _avatarImage =transform.GetChild(0).GetComponent<Image>();
        _button = GetComponent<Button>();
    }

    public void SetRoleUI(RoleData roleData)
    {
        this.roleData = roleData;
        if (roleData.unlock == 0&&PlayerPrefs.GetInt(roleData.name,1)==0)
        {
            //未解锁
            _avatarImage.sprite = Resources.Load<Sprite>("Image/UI/锁");//设置头像图片为锁
           
        }
        else
        {//已解锁
            _avatarImage.sprite = Resources.Load<Sprite>(roleData.avatar);//设置头像图片
        
        }
        _button.onClick.AddListener(() =>
        {
            ButtonClick(roleData);
        });
    }
    public void ButtonClick(RoleData r)
    {   //记录下选角色的信息
        GameManager.Instance.currentRole = r;
        //关闭角色选择面板
        RoleSelectPanel.Instance._canvasGroup.alpha = 0;
        RoleSelectPanel.Instance._canvasGroup.blocksRaycasts = false;
        RoleSelectPanel.Instance._canvasGroup.interactable = false;
        //克隆角色UI
        GameObject go= Instantiate(RoleSelectPanel.Instance._roleDetails,WeaponSelectPanel.Instance._weaponContent);
        go.transform.SetSiblingIndex(0);//设置为第一个子物体即放左边
        //打开武器面板
        WeaponSelectPanel.Instance._canvasGroup.alpha = 1;
        WeaponSelectPanel.Instance._canvasGroup.blocksRaycasts = true;
        WeaponSelectPanel.Instance._canvasGroup.interactable = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _backImage.color = new Color(34 / 255f, 34 / 255f, 34 / 255f);
    }
    //更新UI文本
   public void RenewUI(RoleData r)
    {
       
        if (r.unlock==0)
        {//未解锁
            RoleSelectPanel.Instance._roleName.text = "???";
            RoleSelectPanel.Instance._avatar.sprite = Resources.Load<Sprite>("Image/UI/锁");
            RoleSelectPanel.Instance._roleDescribe.text = r.unlockConditions;
            RoleSelectPanel.Instance._text3.text = "尚无记录";
        }
        else//已解锁
        {
            RoleSelectPanel.Instance._roleName.text = r.name;
            RoleSelectPanel.Instance._avatar.sprite = Resources.Load<Sprite>(r.avatar);
            RoleSelectPanel.Instance._roleDescribe.text = r.describe;
            RoleSelectPanel.Instance._text3.text = GetRecord(r.record);
        }
    }
    //获取通关记录文本
    private string GetRecord(int record)
    {
        string result = "";
        switch (record)
        {
            case -1:
                result = "尚无记录";
                    break;
            case 0:
                result = "通关危险0";
                break;
            case 1:
                result = "通关危险1";
                break;
            case 2:
                result = "通关危险2";
                break;
            case 3:
                result = "通关危险3";
                break;
            case 4:
                result = "通关危险4";
                break;
            case 5:
                result = "通关危险5";
                break;

        }
        return result;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      
        _backImage.color = new Color(207 / 255f, 207 / 255f, 207 / 255f);
        if (RoleSelectPanel.Instance._contentcanvasGroup.alpha != 1)
        {
            RoleSelectPanel.Instance._contentcanvasGroup.alpha = 1;
        }
        RenewUI(roleData);
    }
}
