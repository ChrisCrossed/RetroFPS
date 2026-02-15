using UnityEngine;

class Weapon_Pistol : WEAPON_OBJ
{
    [SerializeField]
    WeaponProjectileType _projectileType = WeaponProjectileType.Null;

    [SerializeField]
    int DamagePerAmmo = 0;

    public Weapon_Pistol()
    {

    }

    public override WeaponProjectileType ProjectileType()
    {
        return _projectileType;
    }
    
    public override int DamagePerProjectile()
    {
        // return base.damage();
        return DamagePerAmmo;
    }

    public override float FireRate()
    {
        return _fireRate;
    }
}
