using Assets.Scripts;
using Assets.Scripts.Weapon;
using UnityEngine;

public class MedicalGunWeapon : WeaponLong
{
    public new void Start()
    {
        base.Start();
        //如果是医生
        if (GameManager.Instance.currentRole.name == "医生") {
            data.cooling /= 3;
        }

    }
    public override GameObject GenerateBullet(Vector2 dir)
    {
        Bullet bullet = Instantiate(GameManager.Instance.medicalBullet_prefab, transform.position, Quaternion.identity).
            GetComponent<Bullet>();
        bullet.dir = dir;
        return bullet.gameObject;
    }
}
