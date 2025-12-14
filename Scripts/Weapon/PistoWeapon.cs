using Assets.Scripts;
using Assets.Scripts.Weapon;
using UnityEngine;

public class PistoWeapon : WeaponLong
{
    public override GameObject GenerateBullet(Vector2 dir)
    {
        Bullet bullet = Instantiate(GameManager.Instance.pistolBullet_prefab, transform.position, Quaternion.identity).
            GetComponent<Bullet>();
        bullet.dir = dir;
        return bullet.gameObject;
    }
}
