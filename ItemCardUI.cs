using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardUI : MonoBehaviour
{
    public ItemData itemData;//脚本身上的数据
    public Button _button;//按钮
    public CanvasGroup _canvasGroup;//设置透明度
    private void Awake()
    {
        _button = transform.GetChild(4).GetComponent<Button>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //监听购买按钮
        _button.onClick.AddListener(() =>
        {
            //购物
           bool result= ShopPanel.Instance.Shopping(itemData);
            if (result) { _canvasGroup.alpha = 0;
                _canvasGroup.interactable = false;
            }
         
        });
    }

 
}
