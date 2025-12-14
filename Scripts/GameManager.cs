using Assets.Scripts.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Random = System.Random;
namespace Assets.Scripts
{
    public class GameManager:MonoBehaviour
    {
       public static GameManager Instance { get; private set; }
        public RoleData currentRole;//角色数据 
        public List<WeaponData> currentWeapons=new List<WeaponData>();//武器数据列表
        [SerializeField]
        public PropData propData=new PropData();//当前的属性
        public List<PropData> currentProps=new List<PropData>();//当前的道具列表

        public DiffcuityData currentDifficulty;//难度数据
        public int currentLevel=1;//关卡数据
        public List<EnemyData> enemyDatas=new List<EnemyData>();//当前关卡敌人数据列表
        public TextAsset enemyTextAsset;//敌人数据文本资源
        public GameObject enemyBullet_prefab;//敌人子弹预制体
        public GameObject money_prefabs;//掉落的金钱预制体
        public GameObject redCircle_prefab;//红圈预制体
        public GameObject arrowBullet_prefab;//弓箭子弹预制体
        public GameObject pistolBullet_prefab;//手枪子弹预制体
        public GameObject medicalBullet_prefab;//医疗枪子弹预制体
        public List<RoleData> roleDatas = new List<RoleData>(); // 角色数据列表
        public TextAsset roleTextAsset; // 角色数据文本资源

        public List<WeaponData> weaponDatas = new List<WeaponData>();//武器数据列表
        public TextAsset weaponTextAsset;//武器数据文本资源

        public List<DiffcuityData> difficultyDatas = new List<DiffcuityData>();//难度数据列表
        public TextAsset difficultyTextAsset;//难度数据文本资源

        //道具数据信息
        public List<PropData> propDatas = new List<PropData>();
        public TextAsset propTextAsset;

        public GameObject number_prefab;//文字伤害预制体
        public SpriteAtlas propsAtlas;//图集

        public GameObject attackMusic;//攻击音效
        public GameObject shootMusic;//射击音效
        public GameObject menuMusic;//菜单音效
        public GameObject hurtMusic;//受伤音效

        public int money = 0;//当前金钱
        public float hp = 15f;//当前生命值
        public float exp;//当前经验值

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            //没有解锁的角色
            if (!PlayerPrefs.HasKey("多面手"))
            {
                PlayerPrefs.SetInt("多面手", 0);
            }
            if (!PlayerPrefs.HasKey("公牛"))
            {
                PlayerPrefs.SetInt("公牛", 0);
            }

            enemyTextAsset=Resources.Load<TextAsset>("Data/enemy");
            enemyDatas=JsonConvert.DeserializeObject<List<EnemyData>>(enemyTextAsset.text);
            enemyBullet_prefab=Resources.Load<GameObject>("Prefabs/EnemyBullet");
            money_prefabs = Resources.Load<GameObject>("Prefabs/Money");
            number_prefab= Resources.Load<GameObject>("Prefabs/Number");

            //音效
            attackMusic= Resources.Load<GameObject>("Prefabs/AttackMusic");
            shootMusic = Resources.Load<GameObject>("Prefabs/ShootMusic");
            menuMusic = Resources.Load<GameObject>("Prefabs/MenuMusic");
            hurtMusic = Resources.Load<GameObject>("Prefabs/HurtMusic");

            redCircle_prefab = Resources.Load<GameObject>("Prefabs/RedCircle");


            //读取json文件并获取角色数据
            roleTextAsset = Resources.Load<TextAsset>("Data/role");// 加载角色数据文本资源
            roleDatas = JsonConvert.DeserializeObject<List<RoleData>>(roleTextAsset.text);// 反序列化角色数据


            //读取武器数据
            weaponTextAsset = Resources.Load<TextAsset>("Data/weapon");
            weaponDatas = JsonConvert.DeserializeObject<List<WeaponData>>(weaponTextAsset.text);
            //读取道具数据
            propTextAsset = Resources.Load<TextAsset>("Data/prop");
            propDatas = JsonConvert.DeserializeObject<List<PropData>>(propTextAsset.text);

            //加载难度数据
            difficultyTextAsset = Resources.Load<TextAsset>("Data/difficulty");
            difficultyDatas = JsonConvert.DeserializeObject<List<DiffcuityData>>(difficultyTextAsset.text);

            propsAtlas = Resources.Load<SpriteAtlas>("Image/其他/Props");

            arrowBullet_prefab = Resources.Load<GameObject>("Prefabs/ArrowBullet");
            pistolBullet_prefab = Resources.Load<GameObject>("Prefabs/PostoBullet");
            medicalBullet_prefab= Resources.Load<GameObject>("Prefabs/MedicalBullet");
        }
        internal object GetRandom<T>(List<T> list)
        {
           if(list==null || list.Count==0)
            {
                return null;
            }
           Random rand = new Random();
           int index = rand.Next(0, list.Count);
           return list[index];
        }
        public  void InitProp()
        {
            if (currentRole.name=="全能者")
            {
                propData.maxHp += 5;
                propData.speedPer += 0.05f;
                propData.harvest += 8;
            }else if (currentRole.name == "斗士")
            {
                propData.short_attackSpeed += 0.5f;
                propData.long_damage -= 0.5f;
                propData.short_range -= 0.5f;
                propData.long_damage -= 0.5f;
            }
            else if (currentRole.name == "医生")
            {
                propData.revive += 5f;
                propData.short_attackSpeed -= 0.5f;
                propData.long_attackSpeed -= 0.5f;

            }
            else if (currentRole.name == "公牛")
            {
                propData.maxHp += 20f;
                propData.revive += 15f;
                propData.slot = 0;
            }
            else if (currentRole.name == "多面手")
            {
                propData.long_damage += 0.2f;
                propData.short_damage += 0.2f;
                propData.slot = 12;

            }
            hp = propData.maxHp;
            money = 30;
            exp = 0;
        }
        /// <summary>
        /// 融合属性
        /// </summary>
        /// <param name="itemData"></param>
        public void FusionAttr(PropData shopProp)
        {
            propData.maxHp += shopProp.maxHp;
            propData.revive += shopProp.revive;
            propData.short_damage += shopProp.short_damage;

            propData.short_range += shopProp.short_range;
            propData.short_attackSpeed += shopProp.short_attackSpeed;

            propData.long_damage += shopProp.long_damage;
            propData.long_range += shopProp.long_range;
            propData.long_attackSpeed += shopProp.long_attackSpeed;
            propData.speedPer += shopProp.speedPer;
            propData.harvest += shopProp.harvest;
            propData.shopDiscount += shopProp.shopDiscount;
            propData.expMuti += shopProp.expMuti;
            propData.pickRange += shopProp.pickRange;
            propData.critical_strikes_probability += shopProp.critical_strikes_probability;
        }
    }
}
