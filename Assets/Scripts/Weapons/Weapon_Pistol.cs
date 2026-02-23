using System.Collections;
using UnityEngine;

class Weapon_Pistol : WEAPON_OBJ
{
    private protected override void SetWeaponTransforms()
    {
        // base.SetWeaponTransforms();
        // WeaponReadyPosition = gameObject.
    }

    #region Weapon Stats
    private protected override WeaponProjectileType ProjectileType()
    {
        return _projectileType;
    }

    private protected override int DamagePerProjectile()
    {
        return _damagePerProjectile;
    }

    private protected override int NumProjectiles()
    {
        return _numProjectiles;
    }

    private protected override float FireRate()
    {
        return _shotsPerSecond;
    }

    private protected override GameObject ProjectileObject()
    {
        return _projectileObject;
    }

    #endregion Weapon Stats

    #region Weapon Actions

    public override void PullWeaponTrigger()
    {
        base.PullWeaponTrigger();
    }

    public override void ReleaseWeaponTrigger()
    {
        base.ReleaseWeaponTrigger();
    }

    public override void ReloadWeapon()
    {
        base.ReloadWeapon();
    }

    float DrawWeaponTime = 0.1f;
    Transform WeaponReadyPosition;
    Transform WeaponHolsterPosition;
    public override float DrawWeapon()
    {
        StartCoroutine(DrawWeaponAnimation());

        return DrawWeaponTime;
    }

    private IEnumerator DrawWeaponAnimation()
    {
        yield return null;
    }

    float HolsterWeaponTime = 0.1f;
    public override float HolsterWeapon()
    {
        StartCoroutine(HolsterWeaponAnimation());

        return 0.1f;
    }

    private IEnumerator HolsterWeaponAnimation()
    {
        yield return null;
    }

    #endregion Weapon Actions

    protected override void Update()
    {
        UPDATE_WeaponAutoFireLoop();
    }
}
