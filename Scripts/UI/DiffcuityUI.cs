using Assets.Scripts;
using Assets.Scripts.Model;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class DiffcuityUI : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public DiffcuityData diffcuityData;//难度数据

    public Image _backImage;//背景图片
    public Image _avatar;//头像图片
    public Button _button;//按钮

   

    internal void SetData(DiffcuityData data)
    {
        this.diffcuityData = data;
     
        _avatar.sprite = Resources.Load<SpriteAtlas>("Image/UI/危险等级").GetSprite(diffcuityData.name);
        SetBackColor(diffcuityData.id);
        _button.onClick.AddListener(() =>
        {
            //记录当前选择的难度数据
            GameManager.Instance.currentDifficulty = diffcuityData;
            //跳转到游戏场景
            SceneManager.LoadScene(2);
        });
    }

    private void Awake()
    {
        _backImage = GetComponent<Image>();
        _button = GetComponent<Button>();
        _avatar = transform.GetChild(0).GetComponent<Image>();

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
      _backImage.color = new Color(207 / 255f, 207 / 255f, 207 / 255f);//鼠标进入改变背景颜色
        RenewUI();
    }
    private void RenewUI()
    {
        DiffcuitySelectPanel.Instance._difficultyAvatar.sprite =_avatar.sprite;//更新难度头像
        DiffcuitySelectPanel.Instance._difficultyName.text = diffcuityData.name;//更新难度名称
        DiffcuitySelectPanel.Instance._difficultyDescribe.text = GetDifficultyDescribe();//更新难度描述
    }

    private string GetDifficultyDescribe()
    {
        string result = "";

        foreach (DiffcuityData d in GameManager.Instance.difficultyDatas)
        {
            result += d.describe + "\n";
            if (d==diffcuityData)
            {
                break;
            }
        }
        //当前是危险0
        if (result == "\n")
        {
            result = "无修改";
        }
        else//危险1-5
        {
            result = result.TrimStart('\n');
        }
        return result;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       // _backImage.color = new Color(34 / 255f, 34 / 255f, 34 / 255f);//鼠标离开恢复背景颜色
        SetBackColor(diffcuityData.id);
    }

    private void SetBackColor(int id)
    {
        switch (id)
        {
            case 1:
                _backImage.color = new Color(33 / 255f, 33 / 255f, 33 / 255f);
                break;
            case 2:
                _backImage.color = new Color(63 / 255f, 88 / 255f, 104 / 255f);
                break;
            case 3:
                _backImage.color = new Color(83 / 255f, 62 / 255f, 103 / 255f);
                break;
            case 4:
                _backImage.color = new Color(103 / 255f, 54 / 255f, 54 / 255f);
                break;
            case 5:
                _backImage.color = new Color(103 / 255f, 69 / 255f, 54 / 255f);
                break;
            case 6:
                _backImage.color = new Color(91 / 255f, 87 / 255f, 55 / 255f);
                break;
        }
    }
}
