using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponRandom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image backImage;//背景图片
    public Button _button;//按钮
    public List<WeaponUI> weaponList = new List<WeaponUI>();//所有武器列表
    private void Awake()
    {
        backImage = GetComponent<Image>();
        _button = GetComponent<Button>();
    }
    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
           
            foreach (WeaponUI weapon in WeaponSelectPanel.Instance._weaponlist.GetComponentsInChildren<WeaponUI>())
            {
               
                    weaponList.Add(weapon);//添加已解锁武器到列表
              
            }
            WeaponUI w = GameManager.Instance.GetRandom(weaponList) as WeaponUI;
            w.RenewUI(w.weaponData);//更新UI文本
            w.OnButtonClick(w.weaponData);//点击随机到的武器
           
        });
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        backImage.color = new Color(207 / 255f, 207 / 255f, 207 / 255f);
        //RoleSelectPanel.Instance._contentcanvasGroup.alpha = 0;//隐藏角色信息面板

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backImage.color = new Color(34 / 255f, 34 / 255f, 34 / 255f);
    }

}
