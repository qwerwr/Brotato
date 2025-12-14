using Assets.Scripts;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public WeaponData weaponData;

    public Image _backImage;//背景图片
    public Image _avatar;//头像图片
    public Button _button;//按钮

    private void Awake()
    {
        _backImage=GetComponent<Image>();
        _button=GetComponent<Button>();
        _avatar=transform.GetChild(0).GetComponent<Image>();
    }

    public void SetData(WeaponData w)
    {
       this.weaponData=w;//设置武器数据
        _avatar.sprite=Resources.Load<Sprite>(weaponData.avatar);//设置头像图片
        _button.onClick.AddListener(() =>
        {
            OnButtonClick(weaponData);
        });//按钮点击事件
    }
    //按钮点击事件
    public void OnButtonClick(WeaponData w)
    {
        //记录当前选择的武器数据
        GameManager.Instance.currentWeapons.Add(w);
        //克隆UI
        GameObject weapon_clone=Instantiate(WeaponSelectPanel.Instance._weaponDetails,DiffcuitySelectPanel.Instance._difficultyConent);
        weapon_clone.transform.SetSiblingIndex(0);//设置克隆UI在父物体的第一个位置
        weapon_clone.GetComponent<CanvasGroup>().alpha = 1;//设置克隆UI可见
        GameObject role_clone = Instantiate(RoleSelectPanel.Instance._roleDetails, DiffcuitySelectPanel.Instance._difficultyConent);
        role_clone.transform.SetSiblingIndex(0);//设置克隆UI在父物体的第一个位置

        //关闭当前面板
        WeaponSelectPanel.Instance._canvasGroup.alpha=0;
        WeaponSelectPanel.Instance._canvasGroup.blocksRaycasts=false;
        WeaponSelectPanel.Instance._canvasGroup.interactable=false;
        //打开下一个面板
        DiffcuitySelectPanel.Instance._canvasGroup.alpha = 1;
        DiffcuitySelectPanel.Instance._canvasGroup.blocksRaycasts = true;
        DiffcuitySelectPanel.Instance._canvasGroup.interactable = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _backImage.color=new Color(34/255f,34/255f,34/255f);//鼠标离开恢复背景颜色
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _backImage.color=new Color(207/255f,207/255f,207/255f);//鼠标进入改变背景颜色
        if (WeaponSelectPanel.Instance._weaponDetailsCanvasGroup.alpha != 1)
        {
            WeaponSelectPanel.Instance._weaponDetailsCanvasGroup.alpha = 1;
        }
        RenewUI(weaponData);
    }

   public void RenewUI(WeaponData w)
    {
        WeaponSelectPanel.Instance._weaponAvatar.sprite=Resources.Load<Sprite>(w.avatar);//更新武器头像
        WeaponSelectPanel.Instance._weaponName.text=w.name;//更新武器名称
        WeaponSelectPanel.Instance._weaponType.text = w.isLong == 0 ? "近战" : "远程";
        WeaponSelectPanel.Instance._weaponDescribe.text=w.describe;//更新武器描述
      
    }
}
