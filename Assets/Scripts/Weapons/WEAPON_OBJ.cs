using UnityEngine;

public enum WeaponProjectileType
{
    Melee,
    HitScan,
    Projectile,
    Null
}


class WEAPON_OBJ : MonoBehaviour
{
    public virtual WeaponProjectileType ProjectileType()
    {
        return WeaponProjectileType.Null;
    }

    protected int _numProjectiles = 1;
    public virtual int NumProjectiles()
    {
        return _numProjectiles;
    }

    protected int _damagePerProjectile = 1;
    public virtual int DamagePerProjectile()
    {
        return _damagePerProjectile;
    }

    protected float _fireRate = 1.0f;
    public virtual float FireRate()
    {
        return _fireRate;
    }
}
