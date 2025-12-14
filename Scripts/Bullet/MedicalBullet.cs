using UnityEngine;

public class MedicalBullet : Bullet
{
    public new void Awake()
    {
        base.Awake();
        tagName = "Enemy";

    }
   
}
