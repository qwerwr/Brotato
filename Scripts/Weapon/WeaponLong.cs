using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Weapon
{
    public class WeaponLong : WeaponBase
    {
        public override void Fire()
        {
            if (isCooling)
            {
                return;
            }
            //音效
            Instantiate(GameManager.Instance.shootMusic);
            // 获取方向
            Vector2 dir=(Vector2)(enemy.position - transform.position).normalized;
            GameObject bullet=GenerateBullet(dir);
            //设置子弹方向
            SetZ(bullet);
            //设置暴击伤害
            bool isCritical = CriticalHits();
            bullet.GetComponent<Bullet>().isCritical = isCritical;
            if (isCritical) {

                //设置暴击
             
                bullet.GetComponent<Bullet>().damage=data.damage*data.critical_strikes_multiple;
            }
            else
            {
                bullet.GetComponent <Bullet>().damage=data.damage;
            }

            //设置子弹初速度
          
            bullet.GetComponent <Bullet>().speed=15f;
            isCooling=true;
        
        }

        private void SetZ(GameObject bullet)
        {
          bullet.transform.eulerAngles=new Vector3(bullet.transform.eulerAngles.x,
              bullet.transform.eulerAngles.y,transform.eulerAngles.z-originZ);//获取子弹朝向
          
        }

        public virtual GameObject GenerateBullet(Vector2 dir)
        {
            return null;
        }
    }
}
