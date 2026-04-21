using System.Collections;
using UnityEngine;

class Weapon_LazerGun : WEAPON_OBJ
{
    c_LazerGunBall_Logic LazerGunBall;

    private protected override void Start()
    {
        base.Start();

        LazerGunBall = _projectileObject.GetComponent<c_LazerGunBall_Logic>();
    }

    private protected override void SetWeaponTransforms()
    {
        base.SetWeaponTransforms();

        Cylinder = GameObject.Find("BulletCastObject");
        Cylinder.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
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

        // CameraRaycastHitObject = _cameraRaycastHitObject;

        TriggerPulled = true;
        Cylinder.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
    }


    public override void PullSecondaryWeaponTrigger()
    {
        base.PullSecondaryWeaponTrigger();

        if (!LazerGunBall.IsActive)
        {
            Transform newTransform = Weapon_FrontPoint.transform;
            newTransform.forward = CameraObject.transform.forward;
            LazerGunBall.FireOrb(newTransform);
        }

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

        TriggerPulled = false;

        Cylinder.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
    }

    public override void ReleaseSecondaryWeaponTrigger()
    {
        base.ReleaseSecondaryWeaponTrigger();
    }

    public override void ReloadWeapon()
    {

    }

    float DrawWeaponTime = 0.1f;
    Coroutine WeaponCoroutine;
    public override float DrawWeapon()
    {
        // This is intended to get the animation for the weapon and play it.
        // This might not be the right way to go about this in the future.
        StartCoroutine( DrawWeaponAnimation() );

        WeaponCoroutine = StartCoroutine( PrimaryFireRaycast() );

        return DrawWeaponTime;
    }

    private IEnumerator DrawWeaponAnimation()
    {
        yield return null;
    }

    float HolsterWeaponTime = 0.1f;
    public override float HolsterWeapon()
    {
        StartCoroutine( HolsterWeaponAnimation() );

        StopCoroutine( WeaponCoroutine );

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

    GameObject Cylinder;
    private IEnumerator PrimaryFireRaycast()
    {
        RaycastHit _hit;
        int layerMask = LayerMask.GetMask("Water", "Geo", "GameObject");

        

        while( true )
        {
            if(TriggerPulled)
            {
                Cylinder.transform.position = Weapon_FrontPoint.position;
                Cylinder.transform.forward = Weapon_FrontPoint.forward;
                float distance = 50f;

                if (Physics.Raycast(CameraObject.transform.position, CameraObject.transform.forward, out _hit, distance, layerMask))
                {
                    distance = _hit.distance;
                }
                else
                {
                    Vector3 finalPos = CameraObject.transform.position + (CameraObject.transform.forward * 50f);
                    Debug.DrawLine(Weapon_FrontPoint.position, finalPos, Color.red, Time.deltaTime);
                }

                Cylinder.transform.localScale = new Vector3(1f, 1f, distance);
            }

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}