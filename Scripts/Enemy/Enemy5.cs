using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Enemy5:EnemyBase
    {
        private Transform pos;
        private void Awake()
        {
         
            pos=GameObject.Find("RedCirclePos").transform;
        }

        public override void LaunchSkill(Vector2 dir)
        {
            int childCount=pos.childCount;
            for (int i = 0; i < childCount; i++) {
             Transform childTransform=pos.GetChild(i);
                GameObject go = Instantiate(GameManager.Instance.redCircle_prefab, childTransform);
                StartCoroutine(PosAutoDestroy(go));
            }
        }

        IEnumerator PosAutoDestroy(GameObject go)
        {
           yield return new WaitForSeconds(1);
            //生成子弹
           GameObject bullet=Instantiate(GameManager.Instance.enemyBullet_prefab, go.transform.position,Quaternion.identity);
           Destroy(go);
            bullet.GetComponent<EnemyBullet>().deadTime = 2;//1秒后销毁
        }
    }
}
