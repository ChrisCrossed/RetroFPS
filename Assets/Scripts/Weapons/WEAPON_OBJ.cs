using UnityEngine;

public enum WeaponProjectileType
{
    Melee,
    HitScan,
    Projectile,
    Null
}

public enum WeaponTypes
{
    Fists,
    Spear,
    Pistol,
    Shotgun
}


class WEAPON_OBJ : MonoBehaviour
{
    [SerializeField]
    protected WeaponProjectileType _projectileType = WeaponProjectileType.Null;
    public virtual WeaponProjectileType ProjectileType()
    {
        return WeaponProjectileType.Null;
    }

    [SerializeField]
    protected int _damagePerProjectile = 1;
    public virtual int DamagePerProjectile()
    {
        return _damagePerProjectile;
    }

    [SerializeField]
    protected int _numProjectiles = 1;
    public virtual int NumProjectiles()
    {
        return _numProjectiles;
    }

    [SerializeField]
    protected float _shotsPerSecond = 1.0f;
    public virtual float FireRate()
    {
        return _shotsPerSecond;
    }

    [SerializeField]
    protected GameObject _projectileObject;
    public virtual GameObject ProjectileObject()
    {
        return _projectileObject;
    }
}
