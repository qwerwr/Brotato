using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class ItemData
    {
        public int id; // 道具ID
        public string name;// 武器名称
        public string avatar;// 武器头像
        public int grade;// 武器等级
        public int price;//价格
        public string describe;//描述

    }
}
