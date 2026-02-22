using UnityEngine;
using UnityEngine.InputSystem;

public class c_PlayerWeaponLogic : MonoBehaviour
{
    InputType InputType = InputType.None;

    GameObject CameraObject;

    InputAction IA_PrimaryAttack;
    bool AttackPressed;

    InputAction IA_CycleWeapon_1;
    InputAction IA_CycleWeapon_2;
    InputAction IA_CycleWeapon_3;
    InputAction IA_CycleWeapon_4;
    bool CycleWeapon1_ButtonState;
    bool CycleWeapon2_ButtonState;
    bool CycleWeapon3_ButtonState;
    bool CycleWeapon4_ButtonState;

    InputAction IA_Controller_CycleWeapon_Next;
    InputAction IA_Controller_CycleWeapon_Previous;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        START_Connections();


    }

    void START_Connections()
    {
        #region Controller Connections
        IA_Controller_CycleWeapon_Next = InputSystem.actions.FindAction("CycleWeapon_Next");
        IA_Controller_CycleWeapon_Previous = InputSystem.actions.FindAction("CycleWeapon_Previous");
        #endregion Controller Connections

        #region Keyboard Mouse Connections
        IA_CycleWeapon_1 = InputSystem.actions.FindAction("CycleWeapon1");
        IA_CycleWeapon_2 = InputSystem.actions.FindAction("CycleWeapon2");
        IA_CycleWeapon_3 = InputSystem.actions.FindAction("CycleWeapon3");
        IA_CycleWeapon_4 = InputSystem.actions.FindAction("CycleWeapon4");
        #endregion Keyboard Mouse Connections

        #region Global Input Connections
        IA_PrimaryAttack = InputSystem.actions.FindAction("Attack");
        #endregion Global Input Connections

        #region Game Object Connections
        CameraObject = gameObject.transform.parent.Find("Main Camera").gameObject;

        CurrentWeapon = gameObject.transform.parent.Find("WeaponCamera").transform.Find("WeaponSystem").transform.Find("Weapon_Pistol").gameObject;

        LastViewedObject = null;
        #endregion Game Object Connections

        Init_WeaponTypes();

        AssignNewWeapon(WeaponTypes.Pistol);

        GetInputType();
    }

    WEAPON_OBJ currWeap;
    Weapon_Pistol weapon_Pistol;
    Weapon_Shotgun weapon_Shotgun;
    Weapon_Fists weapon_Fists;
    void Init_WeaponTypes()
    {
        weapon_Pistol = gameObject.GetComponent<Weapon_Pistol>();
        weapon_Shotgun = gameObject.GetComponent<Weapon_Shotgun>();
        weapon_Fists = gameObject.GetComponent<Weapon_Fists>();
    }

    void GetInputType()
    {
        InputType = InputType.KeyboardMouse;

        /*
        InputType = gameObject.GetComponent<c_PlayerController>().InputType;
        print("Input Type: " + InputType);
        */
    }

    WeaponTypes CurrentlyAssignedWeapon;
    void AssignNewWeapon(WeaponTypes _weaponType)
    {
        if (CurrentlyAssignedWeapon == _weaponType) return;

        CurrentlyAssignedWeapon = _weaponType;

        switch (_weaponType)
        {
            case WeaponTypes.Fists:
                currWeap = weapon_Fists;
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

        currWeap.ApplyWeaponObjects(CurrentWeapon);
    }

    
    void LATEUPDATE_WeaponSwitchLogic()
    {
        switch (InputType)  
        {
            case InputType.Controller:
                WeaponSwitchLogic_Controller();
                break;
            case InputType.None:
                break;
            case InputType.KeyboardMouse:
            default:
                WeaponSwitchLogic_KeyboardMouse();
                break;
        }
    }

    void WeaponSwitchLogic_KeyboardMouse()
    {
        #region Check How Many Buttons are Being Held
        int numButtonsPressed = 0;
        if (IA_CycleWeapon_1.IsPressed()) numButtonsPressed++;
        if (IA_CycleWeapon_2.IsPressed()) numButtonsPressed++;
        if (IA_CycleWeapon_3.IsPressed()) numButtonsPressed++;
        if (IA_CycleWeapon_4.IsPressed()) numButtonsPressed++;

        if (numButtonsPressed > 1) return;
        #endregion Check How Many Buttons are Being Held

        #region Switch Weapons
        if (IA_CycleWeapon_1.IsPressed() && !CycleWeapon1_ButtonState)
        {
            AssignNewWeapon(WeaponTypes.Pistol);
            print("Pistol");
        }
        else if (IA_CycleWeapon_2.IsPressed() && !CycleWeapon2_ButtonState)
        {
            AssignNewWeapon(WeaponTypes.Shotgun);
            print("Shotgun");
        }

        CycleWeapon1_ButtonState = IA_CycleWeapon_1.IsPressed();
        CycleWeapon2_ButtonState = IA_CycleWeapon_2.IsPressed();
        #endregion Switch Weapons
    }

    void WeaponSwitchLogic_Controller()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        LATEUPDATE_PlayerInteract();

        LATEUPDATE_WeaponSwitchLogic();
    }

    // Used for Weapon Fire & various interactions (Use)
    bool AttackPressed_OLD;
    GameObject CurrentWeapon;
    void LATEUPDATE_PlayerInteract()
    {
        AttackPressed = IA_PrimaryAttack.IsPressed();

        #region Camera Raycast

        RaycastHit _hit;
        int layerMask = LayerMask.GetMask("Geo", "GameObject");

        if (Physics.Raycast(CameraObject.transform.position, CameraObject.transform.forward, out _hit, 1000f, layerMask))
        {
            // CursorRaycastOptions(_hit);
        }

        #endregion Camera Raycast

        if (AttackPressed && !AttackPressed_OLD)
        {
            currWeap.PullWeaponTrigger();
        }
        else if (!AttackPressed && AttackPressed_OLD)
        {
            currWeap.ReleaseWeaponTrigger();
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
