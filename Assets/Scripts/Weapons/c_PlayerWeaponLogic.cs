using System.Collections;
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

        LastViewedObject = null;
        #endregion Game Object Connections

        Init_WeaponTypes();

        AssignNewWeapon(WeaponTypes.Pistol);

        GetInputType();
    }

    GameObject CurrentWeapon;
    GameObject WeaponObject_Parent;
    GameObject WeaponObject_Pistol;
    GameObject WeaponObject_Shotgun;
    GameObject WeaponObject_Fists;
    GameObject WeaponObject_Spear;
    WEAPON_OBJ currWeap;
    Weapon_Pistol weapon_Pistol;
    Weapon_Shotgun weapon_Shotgun;
    Weapon_Fists weapon_Fists;
    void Init_WeaponTypes()
    {
        weapon_Pistol = gameObject.GetComponent<Weapon_Pistol>();
        weapon_Shotgun = gameObject.GetComponent<Weapon_Shotgun>();
        weapon_Fists = gameObject.GetComponent<Weapon_Fists>();
        // weapon_Spear = gameObject.GetComponent<Weapon_Spear>();

        WeaponObject_Parent = gameObject.transform.parent.Find("WeaponCamera").transform.Find("WeaponSystem").gameObject;
        WeaponObject_Pistol = WeaponObject_Parent.transform.Find("Weapon_Pistol").gameObject;
        WeaponObject_Shotgun = WeaponObject_Parent.transform.Find("Weapon_Shotgun").gameObject;
        WeaponObject_Fists = WeaponObject_Parent.transform.Find("Weapon_Fists").gameObject;
        WeaponObject_Spear = WeaponObject_Parent.transform.Find("Weapon_Spear").gameObject;
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
        if (WeaponCurrentlySwitching) return;

        CurrentlyAssignedWeapon = _weaponType;

        WEAPON_OBJ prevWeap = currWeap;

        switch (_weaponType)
        {
            case WeaponTypes.Fists:
                currWeap = weapon_Fists;
                CurrentWeapon = WeaponObject_Fists;
                break;
            case WeaponTypes.Spear:
                // currWeap = weapon_Spear;
                CurrentWeapon = WeaponObject_Spear;
                break;
            case WeaponTypes.Pistol:
                currWeap = weapon_Pistol;
                CurrentWeapon = WeaponObject_Pistol;
                break;
            case WeaponTypes.Shotgun:
                currWeap = weapon_Shotgun;
                CurrentWeapon = WeaponObject_Shotgun;
                break;
            default:
                break;
        }

        if (prevWeap != null)
        {
            StartCoroutine(ChangeWeaponThread(prevWeap, currWeap));
        }

        WeaponObject_Spear.SetActive(_weaponType == WeaponTypes.Spear);
        WeaponObject_Fists.SetActive(_weaponType == WeaponTypes.Fists);
        WeaponObject_Pistol.SetActive(_weaponType == WeaponTypes.Pistol);
        WeaponObject_Shotgun.SetActive(_weaponType == WeaponTypes.Shotgun);

        currWeap.ApplyWeaponObjects(CurrentWeapon);

        print(currWeap.DrawWeapon());
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

    bool WeaponCurrentlySwitching;
    IEnumerator ChangeWeaponThread(WEAPON_OBJ _prevWeap, WEAPON_OBJ _nextWeap)
    {
        WeaponCurrentlySwitching = true;

        // Get 'Holster Weapon' timer for current weapon's animation
        float holsterTimerMax = _prevWeap.HolsterWeapon();
        
        float timer = 0f;

        float angle = 0f;

        while (timer < holsterTimerMax)
        {
            timer += Time.deltaTime;

            angle = Mathf.Lerp(0f, 60f, timer /  holsterTimerMax);
            print("DOWN: " + timer + " (" + angle + ")");
            WeaponObject_Parent.transform.localEulerAngles = new Vector3 (angle, 0f, 0f);

            yield return new WaitForEndOfFrame();
        }

        print("---");

        holsterTimerMax = _nextWeap.DrawWeapon();
        timer = holsterTimerMax;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            angle = Mathf.Lerp(0f, 60f, timer / holsterTimerMax);
            print("UP: " + timer + " (" + angle + ")");
            WeaponObject_Parent.transform.localEulerAngles = new Vector3(angle, 0f, 0f);

            yield return new WaitForEndOfFrame();
        }

        WeaponCurrentlySwitching = false;

        yield return null;
    }

    void WeaponSwitchLogic_Controller()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        LATEUPDATE_WeaponSwitchLogic();

        LATEUPDATE_PlayerInteract();
    }

    // Used for Weapon Fire & various interactions (Use)
    bool AttackPressed_OLD;
    
    void LATEUPDATE_PlayerInteract()
    {
        // TODO: Ensure if weapon is being switched, DO NOT allow gunfire
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
