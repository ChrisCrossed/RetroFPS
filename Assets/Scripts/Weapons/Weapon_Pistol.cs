using UnityEngine;

class Weapon_Pistol : WEAPON_OBJ
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

    public override void PullWeaponTrigger( RaycastHit _cameraRaycastHitObject )
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
        if(TriggerPulled)
        {
            RaycastHit _hit;

            Vector3 dir = Weapon_BackPoint.position - Weapon_FrontPoint.position;
            dir.Normalize();
            float dist = Vector3.Distance(Weapon_BackPoint.position, Weapon_FrontPoint.position);

            // From Camera to Hit Point
            Debug.DrawLine(CameraObject.transform.position, CameraRaycastHitObject.point, Color.red, 0.1f);

            // Check from back of gun to front of gun. If it's clear, then fire weapon from front of the gun.
            if (!Physics.Raycast(Weapon_BackPoint.position, dir, out _hit, dist + 0.05f, layerMask))
            {
                //
                Debug.DrawLine(Weapon_FrontPoint.position, CameraRaycastHitObject.point, Color.yellow, 0.1f);


                print("Damage: " + DamagePerProjectile());
            }
            else
            {
                // Otherwise, apply impact at _hit.point & fire 'blank'
            }

            print("Attack!!!");
        }

        /*
            if (AttackPressed && !AttackPressed_OLD)
            {

                Debug.DrawLine(CameraObject.transform.position, CameraRaycastHitObject.point, Color.red, 0.1f);

                // These will properly update when the player switches weapons
                Weapon_BackPoint = CurrentWeapon.transform.Find("Weapon_BackPoint").transform;

                Weapon_FrontPoint = CurrentWeapon.transform.Find("Weapon_FrontPoint").transform;
                Vector3 dir = Weapon_BackPoint.position - Weapon_FrontPoint.position;
                dir.Normalize();
                float dist = Vector3.Distance(Weapon_BackPoint.position, Weapon_FrontPoint.position);

                // Check from back of gun to front of gun. If it's clear, then fire weapon from front of the gun.
                if (!Physics.Raycast(Weapon_BackPoint.position, dir, out _hit, dist + 0.05f, layerMask))
                {
                    //
                    Debug.DrawLine(Weapon_FrontPoint.position, CameraRaycastHitObject.point, Color.yellow, 0.1f);

                
                    print(currWeap.DamagePerProjectile());
                }
                else
                {
                    // Otherwise, apply impact at _hit.point & fire 'blank'
                }

                print("Attack");
            }
            else if (!AttackPressed && AttackPressed_OLD)
            {
                print("Attack Released");
            }
         
         */
    }
}
