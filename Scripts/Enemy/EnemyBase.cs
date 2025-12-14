using Assets.Scripts;
using Assets.Scripts.Model;
using System;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    public EnemyData enemydata;//敌人数据
   

    
    public float attackTimer=0;//攻击计时器
    public int provideExp;//击败后提供的经验值
    public bool isContact=false;//是否接触到玩家
    public bool isCooling=false;//是否在冷却
   

  

    public float skillTimer=0f;//技能计时器
    public bool skilling=false;//是否在使用技能
   

    // Update is called once per frame
    void Update()
    {
        if (Player.Instance.isDead)
        {
            return;
        }
        Move();
        
        if (isContact&&!isCooling)//接触到玩家且不在冷却
        {
           
            Attack();
        }
        //更新攻击计时器
        if (isCooling)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                attackTimer = 0;
                isCooling = false;
            }
        }
        UpdateSkill();
    }

    private void UpdateSkill()
    {
        if (enemydata.skillTime < 0)
        {
            return;
        }
        if (skillTimer <= 0)//可以释放技能
        {
            //判断距离
            float dis = Vector2.Distance(transform.position, Player.Instance.transform.position);
            if (dis <= enemydata.range)
            {
                //发动技能
                Vector2 dir = (Player.Instance.transform.position - transform.position).normalized;
                LaunchSkill(dir);
                skillTimer = enemydata.skillTime;//加冷却时间
            }

        }
        else
        {
            skillTimer -= Time.deltaTime;
            if (skillTimer < 0)
            {
                skillTimer = 0;
            }
        }
    }

    public virtual void LaunchSkill(Vector2 dir)
    {
       
    }

    public void SetElite()
    {
        enemydata.hp *= 2;
        enemydata.damage *= 2;
        GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 134 / 255f, 134 / 255f);
    }

    public void Move()
    {
        if (skilling)//Enemy4使用
        {
            return;
        }
        Vector2 direction= (Player.Instance.transform.position - transform.position).normalized;//朝向玩家的方向
    
        transform.Translate(direction * enemydata.speed * Time.deltaTime);//移动
        TurnAround();
    }
    //转向
    public void TurnAround()
    {
        if (Player.Instance.transform.position.x < transform.position.x)//玩家在敌人左侧
        {
            transform.localScale=new Vector3(-Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);//朝左
        }
        else
        {
            transform.localScale=new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y,transform.localScale.z);
        }
    }
    //攻击
    public void Attack()
    {
        if (isCooling)
        {
            return;
        }
        Player.Instance.Injure(enemydata.damage);

        //开始冷却
        isCooling = true;
        Debug.Log("EnemyBase Start Cooling");
        attackTimer = enemydata.attackTime;
    }
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("EnemyBase OnTriggerEnter2D");
        if (collision.gameObject.CompareTag("Player"))//接触到玩家
        {
            isContact = true;//标记为接触
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("EnemyBase OnTriggerExit2D");
        if (collision.gameObject.CompareTag("Player"))//接触到玩家
        {
            isContact = false;//标记为接触
        }
    }

    //受伤
    public void Injure(float damage)
    {
        
       
        if (enemydata.hp - damage <= 0)
        {
            enemydata.hp = 0;
            Dead();
            // anim.SetTrigger("Die");
        }
        else
        {
            enemydata.hp -= damage;
        }

    }

    //死亡
    public void Dead()
    {
        //增加玩家经验值
        GameManager.Instance.exp += enemydata.provideExp*GameManager.Instance.propData.expMuti;
        GamePanel.Instance.RenewExp();
        //掉落金钱
        Instantiate(GameManager.Instance.money_prefabs, transform.position, Quaternion.identity);
        //销毁自己
        Destroy(gameObject);
    }

}
