using UnityEngine;

class Weapon_Pistol : WEAPON_OBJ
{
    [SerializeField]
    WeaponProjectileType _projectileType = WeaponProjectileType.Null;

    public override WeaponProjectileType ProjectileType()
    {
        return _projectileType;
    }
    
    public override int DamagePerProjectile()
    {
        // return base.damage();
        return 5;
    }

    public override float FireRate()
    {
        return _fireRate;
    }
}
