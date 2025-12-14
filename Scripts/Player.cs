using Assets.Scripts;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private float horizontalInput;
    private float verticalInput;
    public Transform _playerVisual;//玩家视觉对象
    public Animator anim;//动画控制器
    public bool isDead = false;
    public float reviveTimer;//生命再生计时器
    public Transform weaponsPos;//武器生成位置
    private void Awake()
    {
        Instance=this;
        _playerVisual =GameObject.Find("PlayerVisual").transform;
        weaponsPos = GameObject.Find("WeaponsPos").transform;
        anim = _playerVisual.GetComponent<Animator>();

        //初始化角色属性
        if (GameManager.Instance.currentLevel==1)
        {
            GameManager.Instance.InitProp();
        }
    }
    private void Start()
    {
        if (GameManager.Instance.propData.maxHp >= 50)
        {
            if (PlayerPrefs.GetInt("公牛") == 0)
            {
                PlayerPrefs.SetInt("公牛", 1);
                for (int i = 0; i < GameManager.Instance.roleDatas.Count; i++)
                {
                    if (GameManager.Instance.roleDatas[i].name == "公牛")
                    {
                        GameManager.Instance.roleDatas[i].unlock = 1;
                    }
                }
            }
        }
    }
    private void Update()
    {
        if(isDead)
        {
            return;
        }
        Move();
        Revive();
        EatMoney();
    }
    //生命再生
    private void Revive()
    {
       reviveTimer+= Time.time;
     
        if (reviveTimer>=1f)
        {
            //不扣血
            if (GameManager.Instance.propData.revive <= 0)
            {
                return;
            }
            //控制生命恢复在一定范围内
            GameManager.Instance.hp = Mathf.Clamp(
                GameManager.Instance.hp + GameManager.Instance.propData.revive
                , 0, GameManager.Instance.propData.maxHp
                );
            //公牛生命恢复翻倍
            if (GameManager.Instance.currentRole.name == "公牛")
            {
                GameManager.Instance.hp = Mathf.Clamp(
               GameManager.Instance.hp + GameManager.Instance.propData.revive
               , 0, GameManager.Instance.propData.maxHp
               );
            }
        }
        reviveTimer= 0f;
    }

    //移动
    public void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Vector2  movement = new Vector2(horizontalInput, verticalInput).normalized;
        transform.Translate(movement * GameManager.Instance.propData.speed
            * GameManager.Instance.propData.speedPer * Time.deltaTime);
        if (movement.magnitude != 0)
        {
            anim.SetBool("IsMove", true);
        }
        else
        {
            anim.SetBool("IsMove", false);
        }
            TurnAround(horizontalInput);
    }

    //转头
    public  void TurnAround(float h)
    {
        if (h == -1)
        {
            _playerVisual.localScale=new Vector3(-1,_playerVisual.localScale.y,_playerVisual.localScale.z);
        }
        else if(h == 1)
        {
            _playerVisual.localScale=new Vector3(1,_playerVisual.localScale.y,_playerVisual.localScale.z);
        }

    }

    //受伤
    public void Injure(float damage)
    {
      
        if (isDead)
        {
            return;
        }
        if (GameManager.Instance.hp - damage <= 0)
        {
            GameManager.Instance.hp = 0;
            isDead = true;
            Desd();
           
        }
        else
        {
            GameManager.Instance.hp -= damage;
            //文字
            Number number = Instantiate(GameManager.Instance.number_prefab).GetComponent<Number>();
            number.text.text=damage.ToString();
            number.text.color = new Color(255 / 255f, 0, 0);
            number.transform.position=transform.position;
            //音效
            Instantiate(GameManager.Instance.hurtMusic);
        }
        GamePanel.Instance.RenewHp();
    }
    //死亡
    public void Desd()
    {
        isDead = true;
        anim.speed = 0;

        //调取游戏结束界面
        LevelController.Instance.BadGame();
    }
    private void EatMoney()
    {
        Collider2D[] moenyInRange = Physics2D.OverlapCircleAll(
            transform.position, 0.5f * GameManager.Instance.propData.pickRange,
            LayerMask.GetMask("Item"));
        if(moenyInRange.Length > 0)
        {
            for (int i = 0; i < moenyInRange.Length; i++)
            {
                Destroy(moenyInRange[i].gameObject);//销毁金币物体
                GameManager.Instance.money += 1;//增加金币数据
                GamePanel.Instance.RenewMoney();//更新UI
                 //文字
                Number number = Instantiate(GameManager.Instance.number_prefab).GetComponent<Number>();
                number.text.text = "+1";
                number.text.color = new Color(86 / 255f, 185/255f, 86/255f);
                number.transform.position = transform.position;
            }
        }
    }
}
