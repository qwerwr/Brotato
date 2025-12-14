using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Enemy4:EnemyBase
    {
        private float timer=0;//冲锋时间

        //冲锋
        public override void LaunchSkill(Vector2 dir)
        {
            StartCoroutine(Charge(dir));
        }

        IEnumerator Charge(Vector2 dir)
        {
            skilling = true;
            while (timer<0.6f)
            {
                transform.position += (Vector3)dir * enemydata.speed * 1.8f * Time.deltaTime;
                timer += Time.deltaTime;
                yield return null;
            }

            skilling = false;
        }
    }
}
