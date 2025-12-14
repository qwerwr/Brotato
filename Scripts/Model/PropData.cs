using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    /// <summary>
    /// 角色属性
    /// </summary>
    [Serializable]
    public class PropData:ItemData
    {
       
        //生命值相关
        public float maxHp = 15;//最大生命值
        public float revive = 0;//生命值再生

        //武器相关
        public float short_damage = 1;//附加近战武器伤害 百分比
        public float long_damage = 1;//附加远战武器伤害 百分比
        public float short_range = 1;//附加近战武器范围 百分比
        public float long_range = 1;//附加远战武器范围 百分比
        public float short_attackSpeed = 1;//附加近战武器攻速 百分比
        public float long_attackSpeed = 1;//附加远战武器攻速 百分比

        //移动相关
        public float speed = 5;//基础移速
        public float speedPer = 1;//附加移速 百分比

        //游戏性相关
        public int harvest = 0;//金币收获
        public int slot = 6;//武器插槽
        public float shopDiscount = 1;//道具价格 百分比
        public float expMuti = 1;//经验倍率 百分比
        public float pickRange = 1;//拾取范围 百分比
        public float critical_strikes_probability = 0;//暴击率 百分比

    }
}
