using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Enemy3:EnemyBase
    {
        //发射子弹
        public override void LaunchSkill(Vector2 dir)
        {
            GameObject go = Instantiate(GameManager.Instance.enemyBullet_prefab,transform.position , Quaternion.identity);
            go.GetComponent<EnemyBullet>().dir=dir;
        }
    }
}
