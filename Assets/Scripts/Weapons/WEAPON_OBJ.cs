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

    // Draw Weapon function
    public virtual void DrawWeapon()
    {

    }

    // Holster Weapon function
    public virtual void HolsterWeapon()
    {

    }

    protected virtual bool GetAimedAtObject( out RaycastHit _newHit, float _maxDistance = 1000f)
    {
        print("Back: " + Weapon_BackPoint.position);
        print("Front: " + Weapon_FrontPoint.position);

        RaycastHit _hit = new RaycastHit();
        bool objectHit = false;
        layerMask = LayerMask.GetMask("Geo", "GameObject");

        Vector3 cameraDir = CameraObject.transform.forward;
        Vector3 weaponDir = Weapon_FrontPoint.position - Weapon_BackPoint.position;
        weaponDir.Normalize();
        float weaponBarrelLength = Vector3.Distance(Weapon_BackPoint.position, Weapon_FrontPoint.position);

        Debug.DrawRay(CameraObject.transform.position, cameraDir * 10f, Color.aliceBlue, 0.1f);
        
        // Cast from Camera forward to find a valid game object
        if(Physics.Raycast(CameraObject.transform.position, cameraDir, out _hit, _maxDistance + 0.05f, layerMask))
        {
            Vector3 hitPointDir = _hit.point - Weapon_FrontPoint.position;
            hitPointDir.Normalize();

            float weapDist = Vector3.Distance(Weapon_BackPoint.position, _hit.point);

            if (Physics.Raycast(Weapon_FrontPoint.position, hitPointDir, out _hit, weapDist + 0.05f, layerMask))
            {
                Debug.DrawRay(Weapon_FrontPoint.transform.position, hitPointDir * weapDist, Color.blue, 0.1f);

                print("Hit: " + _hit.transform.position);
                print("---");

                objectHit = true;
            }
        }

        _newHit = _hit;
        return objectHit;
    }

    #endregion Weapon Actions



    protected virtual void Update()
    {
        
    }
}
