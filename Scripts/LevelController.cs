using Assets.Scripts;
using Assets.Scripts.Model;
using Assets.Scripts.Weapon;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    public float waveTimer;//波数计时器
    private GameObject _faillPanel;//失败面板
    private GameObject _successPanel;//成功面板
    private GameObject enemy1_prefab;//敌人预制体
    private GameObject enemy2_prefab;//敌人预制体
    private GameObject enemy3_prefab;//敌人预制体
    private GameObject enemy4_prefab;//敌人预制体
    private GameObject enemy5_prefab;//敌人预制体
    private GameObject redFork_prefab;//红色叉子预制体
    private List<EnemyBase> enemy_list=new List<EnemyBase>();//当前场景中的敌人列表
    private Transform _map;
    private TextAsset levelTextAsset;//关卡数据文本
    private List<LevelData> levelDatas;//关卡数据列表
    private LevelData currentLevelData;//当前关卡数据
    private Transform enemyFather;//敌人父物体
    private Dictionary<string,GameObject> enemyPrefabDic=new Dictionary<string,GameObject>();//敌人集合

    private void Awake()
    {
        Instance = this;
        _faillPanel = GameObject.Find("FailPanel");
        _successPanel = GameObject.Find("SuccessPanel");
        enemy1_prefab=Resources.Load<GameObject>("Prefabs/Enemy1");
        enemy2_prefab = Resources.Load<GameObject>("Prefabs/Enemy2");
        enemy3_prefab = Resources.Load<GameObject>("Prefabs/Enemy3");
        enemy4_prefab = Resources.Load<GameObject>("Prefabs/Enemy4");
        enemy5_prefab = Resources.Load<GameObject>("Prefabs/Enemy5");
        redFork_prefab =Resources.Load<GameObject>("Prefabs/RedFork");
        _map =GameObject.Find("Map").transform;
        //加载关卡数据
        levelTextAsset = Resources.Load<TextAsset>("Data/"+GameManager.Instance.currentDifficulty.levelName);
        levelDatas = JsonConvert.DeserializeObject<List<LevelData>>(levelTextAsset.text);
       enemyFather=GameObject.Find("EnemyFather").transform;

        enemyPrefabDic.Add("enemy1", enemy1_prefab);
        enemyPrefabDic.Add("enemy2", enemy2_prefab);
        enemyPrefabDic.Add("enemy3", enemy3_prefab);
        enemyPrefabDic.Add("enemy4", enemy4_prefab);
        enemyPrefabDic.Add("enemy5", enemy5_prefab);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLevelData=levelDatas[GameManager.Instance.currentLevel-1];//获取当前关卡数据,防止到第四场景回来时不知道是第几关
        //赋值波次时间
        waveTimer =currentLevelData.waveTimer;
        GenerateEnemy();//生成敌人
        GenerateWeapon();//生成武器
    }

    private void GenerateWeapon()
    {
        int i=0;
        foreach (WeaponData weaponData in GameManager.Instance.currentWeapons)
        {
            //通过名字获取武器
            GameObject go = Resources.Load<GameObject>("Prefabs/" + weaponData.name);
          WeaponBase wb=  Instantiate(go,Player.Instance.weaponsPos.GetChild(i)).GetComponent<WeaponBase>();
            wb.data= weaponData;//生成武器赋值
            
            i++;
        
        }
    }

    private void GenerateEnemy()
    {
        foreach (WaveData waveData in currentLevelData.enemys)
        {
            for (int  i = 0;  i < waveData.count;  i++)
            {
                StartCoroutine(SwawnEnemies(waveData));//每次生成 敌人启动一次
            }
        }
       
    }
    //生成敌人协程
    IEnumerator SwawnEnemies(WaveData waveData)
    {
        yield return new WaitForSeconds(waveData.timeAxis);//等待时间轴到达
       if(waveTimer > 0 && !Player.Instance.isDead)
        {
            Vector3 random = GetRandomPos(_map.GetComponent<SpriteRenderer>().bounds);//获取随机位置
            GameObject go=Instantiate(redFork_prefab,random, Quaternion.identity);//生成红叉
            yield return new WaitForSeconds(1f);
            Destroy(go);
            if (waveTimer > 0 && !Player.Instance.isDead)
            {
              EnemyBase enemy = Instantiate(enemyPrefabDic[waveData.enemyName], random, Quaternion.identity).GetComponent<EnemyBase>();//生成敌人
                 enemy.transform.parent = enemyFather;
                foreach (EnemyData item in GameManager.Instance.enemyDatas)
                {
                    if (item.name == waveData.enemyName)
                    {
                        enemy.enemydata = item;
                        if (waveData.elite==1)//精英怪赋值
                        {
                            enemy.SetElite();
                        }
                    }
                }


                enemy_list.Add(enemy);
            }
        }
      

    }
    //获取地图内随机位置
    private Vector3 GetRandomPos(Bounds bounds)
    {
        float safeDistance = 3.5f;//安全距离，避免生成在边界附近

        float randomX = UnityEngine.Random.Range(bounds.min.x+ safeDistance, bounds.max.x- safeDistance);
        float randomY = UnityEngine.Random.Range(bounds.min.y+ safeDistance, bounds.max.y- safeDistance);
        return new Vector3(randomX, randomY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (waveTimer > 0) {
        waveTimer-= Time.deltaTime;
            if (waveTimer <=0)
            {
                waveTimer=0;
                if (GameManager.Instance.currentLevel < 5)
                {
                    NextWave();
                }
                else {
                    GoodGame();
                }
               
            }

        }
        GamePanel.Instance.RenewCountDown(waveTimer);
    }
    //下一关
    private void NextWave()
    {
        GameManager.Instance.money += GameManager.Instance.propData.harvest;//收获添加金币中
        SceneManager.LoadScene("04-Shop");//跳转到商店场景
        GameManager.Instance.currentLevel += 1;//波次+1
    }

    //生成敌人

    //游戏胜利
    public void GoodGame()
    {
        _successPanel.GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(GoMenu());
        //所有敌人销毁
        foreach (var enemy in enemy_list)
        {
            if (enemy!=null)
            {
                enemy.Dead();
            }
        }
        //解锁多面手角色
        if (PlayerPrefs.GetInt("多面手") == 0)
        {
            PlayerPrefs.SetInt("多面手", 1);
            for (int i = 0; i < GameManager.Instance.roleDatas.Count; i++) {
                if (GameManager.Instance.roleDatas[i].name == "多面手")
                {
                    GameManager.Instance.roleDatas[i].unlock = 1;
                }
            }
        }
    }
    //波次完成

    //游戏失败
    public void BadGame()
    {
        _faillPanel.GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(GoMenu());
        //所有敌人销毁
        foreach (var enemy in enemy_list)
        {
            if (enemy != null)
            {
                enemy.Dead();
            }
        }
    }
    
    private IEnumerator GoMenu()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
       


    }
}
