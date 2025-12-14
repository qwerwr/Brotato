using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Weapon
{
    public class WeaponBase:MonoBehaviour
    {
        public WeaponData data;//武器数据

        public bool isAttack = false;//是否正在攻击
        public bool isCooling = false;//是否正在冷却
        public bool isAiming = false;//是否正在瞄准
        public float attackTimer = 0f;//攻击计时器
        public float moveSpeed = 5f;//移动速度
        public Transform enemy;//敌人位置
        public float originZ;//原始z轴旋转角度
        public void Awake()
        {
            originZ=transform.eulerAngles.y;
        }


        public void Start()
        {
            //对在选择商店中选择武器后进行数值修改
            data.critical_strikes_probability *= GameManager.Instance.propData.critical_strikes_probability;//修改武器暴击率
                                                                                                            //近战修改
            if (data.isLong == 0)
            {
                data.range *= GameManager.Instance.propData.short_range;
                data.damage*=(GameManager.Instance.propData.short_damage*data.grade);
                data.cooling /= GameManager.Instance.propData.short_attackSpeed;
            }else if (data.isLong == 1)
            {
                data.range *= GameManager.Instance.propData.long_range;
                data.damage *= (GameManager.Instance.propData.long_damage*data.grade);
                data.cooling /= GameManager.Instance.propData.long_attackSpeed;
            }
        }

        private void Update()
        {
            if (Player.Instance.isDead)
            {
                return;
            }
            //自动瞄准
            if (isAiming)
            {
                Aiming();
            }
           
            //判断攻击
            if(isAttack&&!isCooling)
            {
                Fire();
            }
            //冷却计时
            if (isCooling)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= data.cooling)
                {
                    attackTimer = 0f;
                    isCooling = false;
                }
            }
        }

        public virtual void Fire()
        {
           
        }

        private void Aiming()
        {
            Collider2D[] enemisInRange = Physics2D.OverlapCircleAll(transform.position,
                data.range, 
                LayerMask.GetMask("Enemy"));//检测范围内的敌人

            //如果有敌人
            if(enemisInRange.Length>0)
            {
                isAttack = true;
                Collider2D nearestEnemy=enemisInRange.OrderBy(
                  enemy=>Vector2.Distance(transform.position,enemy.transform.position
                      )).First();//获取第一个离武器最近的敌人
                enemy = nearestEnemy.transform;
                Vector2 enemyPos = enemy.transform.position;
                Vector2 direction = enemyPos - (Vector2)transform.position;//获得两者的距离
                float angleDegrees=Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;//把距离调成角度    
                transform.eulerAngles = new Vector3(transform.eulerAngles.x,
                   transform.eulerAngles.y, angleDegrees + originZ);//旋转武器角度
            }   
            else
            {
                isAttack = false;
                enemy = null;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x,
                  transform.eulerAngles.y, originZ);//旋转武器角度

            }
        }
        //计算是否暴击
        public bool CriticalHits()
        {
            float randomValue = Random.Range(0, 1f);
            return randomValue < data.critical_strikes_probability;
        }
    }

}
