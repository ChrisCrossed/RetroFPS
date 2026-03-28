using System.Collections;
using UnityEngine;

class Weapon_LazerGun : WEAPON_OBJ
{
    private protected override void SetWeaponTransforms()
    {
        base.SetWeaponTransforms();
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

        print("Fire Primary");
    }

    public virtual void PullWeaponTrigger( RaycastHit _cameraRaycastHitObject )
    {
        CameraRaycastHitObject = _cameraRaycastHitObject;

        TriggerPulled = true;
    }

    public override void PullSecondaryWeaponTrigger()
    {
        base.PullSecondaryWeaponTrigger();

        print("Fire Secondary");
    }

    /*
    public virtual void PullSecondaryWeaponTrigger(RaycastHit _cameraRaycastHitObject)
    {
        TriggerSecondaryPulled = true;

        CameraRaycastHitObject = _cameraRaycastHitObject;
    }
    */

    public override void ReleaseWeaponTrigger()
    {
        base.ReleaseWeaponTrigger();
    }

    public override void ReleaseSecondaryWeaponTrigger()
    {
        base.ReleaseSecondaryWeaponTrigger();
    }

    public override void ReloadWeapon()
    {

    }

    float DrawWeaponTime = 0.1f;
    public override float DrawWeapon()
    {
        // This is intended to get the animation for the weapon and play it.
        // This might not be the right way to go about this in the future.
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

        return HolsterWeaponTime;
    }

    private IEnumerator HolsterWeaponAnimation()
    {
        yield return null;
    }

    #endregion Weapon Actions

    protected override void Update()
    {
        
    }
}