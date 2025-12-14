using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoleRandom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image backImage;//背景图片
    public Button _button;//按钮
    public List<RoleUI> unlockedRoles = new List<RoleUI>();
    private void Awake()
    {
        backImage = GetComponent<Image>();
        _button = GetComponent<Button>();
    }
    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            foreach (RoleUI role in RoleSelectPanel.Instance._roleList.GetComponentsInChildren<RoleUI>())
            {
                if (role.roleData.unlock == 1)
                {
                    unlockedRoles.Add(role);//添加已解锁角色到列表
                }
            }
            RoleUI r = GameManager.Instance.GetRandom(unlockedRoles) as RoleUI;
            r.RenewUI(r.roleData);//更新UI文本
            r.ButtonClick(r.roleData);//点击随机到的角色
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        backImage.color = new Color(207 / 255f, 207 / 255f, 207 / 255f);
        RoleSelectPanel.Instance._contentcanvasGroup.alpha = 0;//隐藏角色信息面板

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backImage.color = new Color(34 / 255f, 34 / 255f, 34 / 255f);
    }
}
