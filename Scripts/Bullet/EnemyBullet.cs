using UnityEngine;

public class EnemyBullet : Bullet
{
    public new void Awake()
    {
        base.Awake();
        tagName = "Player";

    }
}
