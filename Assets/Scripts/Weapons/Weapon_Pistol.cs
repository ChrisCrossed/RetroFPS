using UnityEngine;

class Weapon_Pistol : WEAPON_OBJ
{
    float FireRateTimer = 0f;

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

        if(FireRateTimer <= 0f)
        {
            FireRateTimer = FireRate();
        }
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
        bool canFire = false;

        if(FireRateTimer >= FireRate())
        {
            canFire = true;
        }

        print(canFire);

        // if (TriggerPulled && canFire)
        if (canFire)
        {
            #region Raycast Check & Apply Damage
            RaycastHit _hit;

            if(GetAimedAtObject( out _hit ))
            {
                print("Hit: " + _hit.transform.name + " for " + (DamagePerProjectile() * NumProjectiles() + " damage."));
            }
            else print("EMPTY");
        }
        #endregion Raycast Check & Apply Damage

        // Need to figure out how to continue allowing the fire system to continue when holding the trigger while not interfering with single-action fire
        #region Decrease FireRate Timer
        if (FireRateTimer > 0f)
        {
            FireRateTimer -= Time.deltaTime;

            if(FireRateTimer < 0f)
            {
                if (TriggerPulled)
                {
                    FireRateTimer += FireRate();
                    print("Reset (Held)");
                }
                else
                {
                    FireRateTimer = 0f;
                    print("Reset (Release)");
                }
            }
        }
        #endregion
    }
}
