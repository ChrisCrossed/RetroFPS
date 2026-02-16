using UnityEngine;
using UnityEngine.InputSystem;

public class c_PlayerWeaponLogic : MonoBehaviour
{
    GameObject CameraObject;

    InputAction IA_PrimaryAttack;
    bool AttackPressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        START_Connections();


    }

    void START_Connections()
    {
        IA_PrimaryAttack = InputSystem.actions.FindAction("Attack");

        CameraObject = gameObject.transform.parent.Find("Main Camera").gameObject;

        CurrentWeapon = gameObject.transform.parent.Find("WeaponCamera").transform.Find("WeaponSystem").transform.Find("Weapon_Pistol").gameObject;

        LastViewedObject = null;

        Init_WeaponTypes();

        AssignNewWeapon(WeaponTypes.Pistol);
    }

    WEAPON_OBJ currWeap;
    Weapon_Pistol weapon_Pistol;
    Weapon_Shotgun weapon_Shotgun;
    void Init_WeaponTypes()
    {
        weapon_Pistol = gameObject.GetComponent<Weapon_Pistol>();
        weapon_Shotgun = gameObject.GetComponent<Weapon_Shotgun>();

    }

    void AssignNewWeapon(WeaponTypes _weaponType)
    {
        switch (_weaponType)
        {
            case WeaponTypes.Fists:
                break;
            case WeaponTypes.Spear:
                break;
            case WeaponTypes.Pistol:
                currWeap = weapon_Pistol;
                break;
            case WeaponTypes.Shotgun:
                currWeap = weapon_Shotgun;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        LATEUPDATE_PlayerInteract();
    }

    // Used for Weapon Fire & various interactions (Use)
    bool AttackPressed_OLD;
    RaycastHit CameraRaycastHitObject;
    Transform Weapon_BackPoint;
    Transform Weapon_FrontPoint;
    GameObject CurrentWeapon;
    void LATEUPDATE_PlayerInteract()
    {
        AttackPressed = IA_PrimaryAttack.IsPressed();

        #region Camera Raycast

        RaycastHit _hit;
        int layerMask = LayerMask.GetMask("Geo", "GameObject");

        if (Physics.Raycast(CameraObject.transform.position, CameraObject.transform.forward, out _hit, 1000f, layerMask))
        {
            CameraRaycastHitObject = _hit;

            CursorRaycastOptions(_hit);
        }

        #endregion Camera Raycast

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

        AttackPressed_OLD = AttackPressed;
    }

    #region Gameplay Functions

    GameObject LastViewedObject;
    void CursorRaycastOptions(RaycastHit hit)
    {
        if (LastViewedObject != hit.transform.gameObject && LastViewedObject != null)
        {
            switch (LastViewedObject.tag)
            {
                case "Enemy":
                    print("Enemy");
                    break;

                case "Ground":
                    print("Ground");
                    break;

                case "Wall":
                    print("Wall");
                    break;

                case "GameObject":
                    print("GameObject");
                    break;

                case "ElevatorButton":
                    LastViewedObject.transform.GetComponent<c_ElevatorButton>().LookAtButton = false;
                    break;

                default:
                    break;
            }

            LastViewedObject = null;
        }

        if (LastViewedObject == null)
        {
            LastViewedObject = hit.transform.gameObject;

            switch (LastViewedObject.tag)
            {
                case "Enemy":
                    print("Enemy");
                    break;

                case "Ground":
                    print("Ground");
                    break;

                case "Wall":
                    print("Wall");
                    break;

                case "GameObject":
                    print("GameObject");
                    break;

                case "ElevatorButton":
                    LastViewedObject.GetComponent<c_ElevatorButton>().LookAtButton = true;
                    break;

                default:
                    break;
            }
        }
    }

    #endregion Gameplay Functions
}
