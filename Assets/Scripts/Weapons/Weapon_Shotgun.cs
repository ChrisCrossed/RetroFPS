using UnityEngine;

class Weapon_Shotgun : WEAPON_OBJ
{
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

    public override void PullWeaponTrigger(RaycastHit _cameraRaycastHitObject)
    {
        CameraRaycastHitObject = _cameraRaycastHitObject;

        TriggerPulled = true;
    }

    public override void ReleaseWeaponTrigger()
    {
        base.ReleaseWeaponTrigger();
    }

    public override void ReloadWeapon()
    {
        base.ReloadWeapon();
    }

    #endregion Weapon Actions

    protected override void Update()
    {

    }
}
