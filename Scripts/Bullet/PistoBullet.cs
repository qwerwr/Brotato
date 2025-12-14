using UnityEngine;

public class PistoBullet : Bullet
{
    public new void Awake()
    {
        base.Awake();
        tagName = "Enemy";

    }
    
}
