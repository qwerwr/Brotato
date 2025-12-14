using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class EnemyData
    {
        public int id;
        public string name;
        public float hp;
        public float damage;
        public float speed;
        public float attackTime;
        public int provideExp;
        public float skillTime;
        public float range;
    }
}
