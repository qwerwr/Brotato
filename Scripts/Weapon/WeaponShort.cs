
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public class WeaponShort : WeaponBase
    {

        public new void Awake()
        {
            base.Awake();
            moveSpeed = 10;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                //设置暴击伤害
                bool isCritical = CriticalHits();
                if (isCritical)
                {
                    collision.GetComponent<EnemyBase>().Injure(data.damage * data.critical_strikes_multiple);
                    //文字
                    Number number = Instantiate(GameManager.Instance.number_prefab).GetComponent<Number>();
                    number.text.text = (data.damage * data.critical_strikes_multiple).ToString();
                    number.text.color = new Color(255 / 255f, 188 / 255f, 0);
                    number.transform.position = transform.position;
                }
                else
                {
                    collision.GetComponent<EnemyBase>().Injure(data.damage);
                    //文字
                    Number number = Instantiate(GameManager.Instance.number_prefab).GetComponent<Number>();
                    number.text.text = (data.damage).ToString();
                    number.text.color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                    number.transform.position = transform.position;
                }
                //音效
                Instantiate(GameManager.Instance.attackMusic);
            }
        }

        //开火
        public override void Fire()
        {

            if (isCooling)
            {
                return;
            }
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;//打开碰撞器
            isAiming = false;//关闭瞄准
            StartCoroutine(GoPosition());//武器移动到目标位置
            isCooling = true;
        }

        IEnumerator GoPosition()
        {
            //敌人位置
            var enemyPos = enemy.position + new Vector3(0, enemy.GetComponent<SpriteRenderer>().size.y / 2, 0); // corrected to use bounds size
            while (Vector2.Distance(transform.position, enemyPos) > 0.1f)
            {
                Vector3 direction = (enemyPos - (Vector3)transform.position).normalized;
                Vector3 moveAmount = direction * moveSpeed * Time.deltaTime;//计算移动量
                transform.position += moveAmount;//移动武器
                yield return null;
            }
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;//关闭碰撞器
            StartCoroutine(ReturnPosition());//返回原始位置
        }

        IEnumerator ReturnPosition()
        {
            while ((Vector3.zero - transform.localPosition).magnitude > 0.1f)
            {
                Vector3 direction = (Vector3.zero - transform.localPosition).normalized;
                transform.localPosition += direction * moveSpeed * Time.deltaTime;//计算移动量
                yield return null;
            }
            isAiming = true;//开启瞄准
        }

    }
}
