using Unity.VisualScripting;
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
    #region Weapon Stats

    [SerializeField]
    private protected WeaponProjectileType _projectileType = WeaponProjectileType.Null;
    private protected virtual WeaponProjectileType ProjectileType()
    {
        return WeaponProjectileType.Null;
    }

    [SerializeField]
    private protected int _damagePerProjectile = 1;
    private protected virtual int DamagePerProjectile()
    {
        return _damagePerProjectile;
    }

    [SerializeField]
    private protected int _numProjectiles = 1;
    private protected virtual int NumProjectiles()
    {
        return _numProjectiles;
    }

    [SerializeField]
    private protected float _shotsPerSecond = 1.0f;
    private protected virtual float FireRate()
    {
        return _shotsPerSecond;
    }

    [SerializeField]
    private protected GameObject _projectileObject;
    private protected virtual GameObject ProjectileObject()
    {
        return _projectileObject;
    }

    #endregion Weapon Stats

    #region Weapon Actions

    private protected bool TriggerPulled;
    public virtual void PullWeaponTrigger()
    {
        TriggerPulled = true;
    }

    public virtual void ReleaseWeaponTrigger()
    {
        TriggerPulled = false;
    }

    public virtual void ReloadWeapon()
    {

    }

    #endregion Weapon Actions

    protected virtual void Update()
    {
        
    }
}
