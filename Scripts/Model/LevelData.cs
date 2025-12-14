using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public  class LevelData
    {
        public int id;
        public int waveTimer;
        public List<WaveData> enemys;//波次数据
    }
    public class WaveData
    {
        public string enemyName;//敌人名称
        public int timeAxis;//时间轴
        public int count;//数量
        public int elite;//是否为精英怪1是
    }
}
