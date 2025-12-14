using Assets.Scripts;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 1;//攻击力
    public float deadTime = 5;//超时销毁
    public float speed = 8;//速度
    private float timer;//定时器
    public Vector2 dir = Vector2.zero;//方向
    public string tagName;//碰撞检测的对象
    public bool isCritical = false;//是否暴击

    public void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //超时销毁
        timer += Time.deltaTime;
        if (timer > deadTime)
        {
            Destroy(gameObject);
        }
        transform.position += (Vector3)dir * speed * Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagName))
        {
            if (tagName == "Player")//对象是玩家
            {
                Player.Instance.Injure(damage);
               
            }
           else if(tagName == "Enemy")//对象是敌人
            {
                if (isCritical) {
                    //文字
                    Number number = Instantiate(GameManager.Instance.number_prefab).GetComponent<Number>();
                    number.text.text = (damage).ToString();
                    number.text.color = new Color(255 / 255f, 178 / 255f, 0);
                    number.transform.position = transform.position;
                }
                else
                {
                    //文字
                    Number number = Instantiate(GameManager.Instance.number_prefab).GetComponent<Number>();
                    number.text.text = (damage).ToString();
                    number.text.color = new Color(255 / 255f, 255 / 255f, 255/255f);
                    number.transform.position = transform.position;
                }
                collision.gameObject.GetComponent<EnemyBase>().Injure(damage);

            }
            Destroy(gameObject);
        }
    }
}
