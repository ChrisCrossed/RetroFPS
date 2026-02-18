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
    #region Init / Connections

    private protected GameObject CameraObject;
    private protected RaycastHit CameraRaycastHitObject;
    private protected Transform Weapon_BackPoint;
    private protected Transform Weapon_FrontPoint;
    private protected GameObject CurrentWeapon;

    private protected int layerMask;

    private protected virtual void Start()
    {
        START_Connections();
    }

    private protected virtual void START_Connections()
    {
        CameraObject = gameObject.transform.parent.Find("Main Camera").gameObject;

        layerMask = LayerMask.GetMask("Geo", "GameObject");
    }

    public virtual void ApplyWeaponObjects(GameObject _currentWeapon)
    {
        CurrentWeapon = _currentWeapon;

        Weapon_BackPoint = CurrentWeapon.transform.Find("Weapon_BackPoint").transform;
        Weapon_FrontPoint = CurrentWeapon.transform.Find("Weapon_FrontPoint").transform;
    }

    #endregion Init / Connections

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
    public virtual void PullWeaponTrigger( RaycastHit _cameraRaycastHitObject )
    {
        CameraRaycastHitObject = _cameraRaycastHitObject;

        TriggerPulled = true;
    }

    public virtual void ReleaseWeaponTrigger()
    {
        TriggerPulled = false;
    }

    public virtual void ReloadWeapon()
    {

    }

    // Holster Weapon function?

    // Draw Weapon function?

    protected virtual bool GetAimedAtObject( out RaycastHit _newHit, float _maxDistance = 1000f)
    {
        // RaycastHit _hit = new RaycastHit();
        bool objectHit = false;

        Vector3 dir = Weapon_BackPoint.position - Weapon_FrontPoint.position;
        dir.Normalize();
        float weaponBarrelLength = Vector3.Distance(Weapon_BackPoint.position, Weapon_FrontPoint.position);

        // Check from back of gun to front of gun. If it's clear, then fire weapon from front of the gun.
        if (!Physics.Raycast(Weapon_BackPoint.position, dir, out _newHit, weaponBarrelLength + 0.05f, layerMask))
        {
            // Show line from CAMERA to hit point
            Debug.DrawLine(CameraObject.transform.position, _newHit.point, Color.yellow, 0.1f);

            if (Physics.Raycast(Weapon_FrontPoint.position, dir, out _newHit, _maxDistance, layerMask))
            {
                // Show line from WEAPON BARREL to hit point
                Debug.DrawLine(Weapon_FrontPoint.transform.position, _newHit.point, Color.red, 0.1f);

                objectHit = true;
            }
        }
        else
        {
            // Show line from WEAPON BARREL to hit point
            Debug.DrawLine(Weapon_FrontPoint.transform.position, _newHit.point, Color.red, 0.1f);

            // Otherwise, apply impact at _hit.point & fire 'blank'
            objectHit = true;
        }

        return objectHit;
    }

    #endregion Weapon Actions



    protected virtual void Update()
    {
        
    }
}
