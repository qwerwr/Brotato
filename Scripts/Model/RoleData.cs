using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class RoleData
    {
        public int id;//编号
        public string name;//名称
        public string avatar;//头像
        public string describe;//描述
        public int slot;//槽位
        public int record;//是否记录
        public int unlock;//是否解锁
        public string unlockConditions;//解锁条件
    }
}
