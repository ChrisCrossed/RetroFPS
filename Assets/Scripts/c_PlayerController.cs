using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class c_PlayerController : MonoBehaviour
{
    CapsuleCollider PlayerCollider;
    CharacterController PlayerController;
    GameObject CameraObject;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        START_Connections();
        START_Settings();
    }

    void START_Connections()
    {
        IA_Move = InputSystem.actions.FindAction("Move");
        IA_Look = InputSystem.actions.FindAction("Look");
        IA_PrimaryAttack = InputSystem.actions.FindAction("Attack");

        PlayerCollider = gameObject.GetComponent<CapsuleCollider>();
        PlayerController = gameObject.GetComponent<CharacterController>();
        CameraObject = gameObject.transform.Find("Main Camera").gameObject;

        LastViewedObject = null;

        CurrentWeapon = gameObject.transform.Find("WeaponCamera").transform.Find("WeaponSystem").transform.Find("Weapon_Pistol").gameObject;
    }

    void START_Settings()
    {
        MouseCursorState(false);
    }


    // Update is called once per frame
    void Update()
    {
        UPDATE_GetPlayerInput();
        UPDATE_PlayerMovement();
    }

    private void FixedUpdate()
    {
        FIXEDUPDATE_CliffEdgeVelocity();
    }

    private void LateUpdate()
    {
        LATEUPDATE_PlayerLook();
        LATEUPDATE_PlayerInteract();
    }

    Vector2 v2_PlayerInputVector;
    Vector2 v2_PlayerMoveVector;
    InputAction IA_Move;

    InputAction IA_Look;
    Vector2 v2_MouseInput;
    [SerializeField] float HorizontalLookMultiplier = 5f;
    [SerializeField] float VerticalLookMultiplier = 5f;

    InputAction IA_PrimaryAttack;
    bool AttackPressed;

    void UPDATE_GetPlayerInput()
    {
        v2_PlayerInputVector = IA_Move.ReadValue<Vector2>();
        v2_PlayerInputVector.Normalize();

        v2_MouseInput = IA_Look.ReadValue<Vector2>();

        AttackPressed = IA_PrimaryAttack.IsPressed();
    }

    float CameraAngle = 0f;
    void LATEUPDATE_PlayerLook()
    {
        if (v2_MouseInput == new Vector2())
            return;

        if(v2_MouseInput.x != 0f)
        {
            Vector3 v3_PlayerDirection = PlayerController.transform.localEulerAngles;
            v3_PlayerDirection.y += v2_MouseInput.x * HorizontalLookMultiplier;
            PlayerController.transform.localEulerAngles = v3_PlayerDirection;
        }

        if(v2_MouseInput.y != 0f)
        {
            CameraAngle -= v2_MouseInput.y * VerticalLookMultiplier;
            CameraAngle = Mathf.Clamp(CameraAngle, -89.9f, 89.9f);
            CameraObject.transform.localEulerAngles = new Vector3(CameraAngle, 0f, 0f);
        }

        RaycastHit _hit;
        int layerMask = LayerMask.GetMask("Geo", "GameObject");

        if(Physics.Raycast(CameraObject.transform.position, CameraObject.transform.forward, out _hit, 1000f, layerMask))
        {
            CameraRaycastHitObject = _hit;

            CursorRaycastOptions(_hit);
        }
    }

    // Used for Weapon Fire & various interactions (Use)
    bool AttackPressed_OLD;
    RaycastHit CameraRaycastHitObject;
    Transform Weapon_BackPoint;
    Transform Weapon_FrontPoint;
    GameObject CurrentWeapon;
    void LATEUPDATE_PlayerInteract()
    {
        if(AttackPressed && !AttackPressed_OLD)
        {
            LayerMask mask = LayerMask.GetMask("Geo", "GameObject");
            RaycastHit _hit;

            Debug.DrawLine(CameraObject.transform.position, CameraRaycastHitObject.point, Color.red, 0.1f);

            // These will properly update when the player switches weapons
            Weapon_BackPoint = CurrentWeapon.transform.Find("Weapon_BackPoint").transform;

            Weapon_FrontPoint = CurrentWeapon.transform.Find("Weapon_FrontPoint").transform;
            Vector3 dir = Weapon_BackPoint.position - Weapon_FrontPoint.position;
            dir.Normalize();
            float dist = Vector3.Distance(Weapon_BackPoint.position, Weapon_FrontPoint.position);

            // Check from back of gun to front of gun. If it's clear, then fire weapon from front of the gun.
            if(!Physics.Raycast(Weapon_BackPoint.position, dir, out _hit, dist + 0.05f, mask ))
            {
                //
                Debug.DrawLine(Weapon_FrontPoint.position, CameraRaycastHitObject.point, Color.yellow, 0.1f);

                WEAPON_OBJ currWeap = new WEAPON_OBJ();
                currWeap = new Weapon_Pistol();
                print(currWeap.DamagePerProjectile());
            }
            else
            {
                // Otherwise, apply impact at _hit.point & fire 'blank'
            }

                print("Attack");
        }
        else if(!AttackPressed && AttackPressed_OLD)
        {
            print("Attack Released");
        }

        AttackPressed_OLD = AttackPressed;
    }

    float VelocitySpeedMult = 8f;
    int LayerMask_Ground;
    float Gravity = -9.81f * 3.5f;
    float yVel;
    Vector3 CliffPushVelocity;
    void UPDATE_PlayerMovement()
    {
        #region Convert player input into desired movement velocity
        v2_PlayerMoveVector = MovementVelocityPerc(v2_PlayerMoveVector, v2_PlayerInputVector);

        Vector3 v3_InputVector = new Vector3(v2_PlayerMoveVector.x, 0f, v2_PlayerMoveVector.y);
        #endregion Convert player input into desired movement velocity

        #region Cast downward to ground

        RaycastHit _hit;
        Vector3 playerVector = new Vector3();

        LayerMask_Ground = LayerMask.GetMask("Geo");
        bool onGround = false;

        if (Physics.SphereCast(gameObject.transform.position, PlayerCollider.radius - 0.001f, Vector3.down, out _hit, PlayerCollider.radius + 0.71f, LayerMask_Ground))
        {
            onGround = true;

            float contactDegrees = Vector3.Angle(-Vector3.up, -_hit.normal);
            // print(contactDegrees);

            yVel = 0f;

            // Convert player's desired movement against the character's forward direction
            playerVector = gameObject.transform.rotation * v3_InputVector;

            // Project the new vector against the ground's normal
            v3_InputVector = Vector3.ProjectOnPlane(playerVector, -_hit.normal);

            // Debug.DrawRay(gameObject.transform.position, v3_InputVector * 5f, Color.red);

            PlayerController.Move(-Vector3.up * 0.1f);

            #region Cliff Edge Check

            bool isCliffLogicEnabled = contactDegrees >= minimumGroundAngle;
            SetCliffLogicState(isCliffLogicEnabled, contactDegrees, _hit);

            #endregion Cliff Edge Check

            // Debug.DrawRay(gameObject.transform.position, playerVector * 100.0f, Color.red);

        }

        if (!onGround)
        {
            yVel += Gravity * Time.deltaTime;
        }

        playerVector *= 5f;
        playerVector += yVel * Vector3.up;

        PlayerController.Move(playerVector * Time.deltaTime);


        #endregion Cast downward to ground

    }

    bool CliffEdgeLogic_IsEnabled;
    float CliffEdgeLogic_ContactDegrees;
    RaycastHit CliffEdgeLogic_hit;
    Vector3 CliffEdgeLogic_CliffVector;
    void SetCliffLogicState(bool _isEnabled, float _contactDegrees, RaycastHit _hit)
    {
        CliffEdgeLogic_IsEnabled = _isEnabled;
        CliffEdgeLogic_ContactDegrees = _contactDegrees;
        CliffEdgeLogic_hit = _hit;
    }

    void FIXEDUPDATE_CliffEdgeVelocity()
    {
        if (CliffEdgeLogic_IsEnabled)
        {
            CliffEdgeLogic_CliffVector = CliffEdgeLogic(CliffEdgeLogic_ContactDegrees, CliffEdgeLogic_hit);

            PlayerController.Move(CliffEdgeLogic_CliffVector * Time.fixedDeltaTime);
        }
    }
    #region Math Functions

    Vector2 MovementVelocityPerc(Vector2 VelPercValue, Vector2 PlayerInputValue)
    {
        Vector2 output = new Vector2();

        output.x = MovementVelocityPerc(VelPercValue.x, PlayerInputValue.x);
        output.y = MovementVelocityPerc(VelPercValue.y, PlayerInputValue.y);

        return output;
    }
    float MovementVelocityPerc(float VelPercValue, float PlayerInputValue)
    {
        if (PlayerInputValue != VelPercValue)
        {
            if (PlayerInputValue < VelPercValue)
            {
                VelPercValue -= Time.deltaTime * VelocitySpeedMult;

                if (VelPercValue < PlayerInputValue)
                    VelPercValue = PlayerInputValue;
            }
            else
            {
                VelPercValue += Time.deltaTime * VelocitySpeedMult;

                if (VelPercValue > PlayerInputValue)
                    VelPercValue = PlayerInputValue;
            }
        }

        return VelPercValue;
    }

    #endregion Math Functions

    #region Settings

    void MouseCursorState( bool IsVisible )
    {
        if (!IsVisible)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        Cursor.visible = IsVisible;
    }

    #endregion Settings

    #region Gameplay Functions

    float minimumGroundAngle = 10f;
    float maximumGroundAngle = 80f;
    float pushPercMultiplier = 20f;
    float minimumPushMagnitude = 10f;
    float maximumPushMagnitude = 25f;
    Vector3 CliffEdgeLogic(float _contactDegrees, RaycastHit _hit)
    {
        float colliderRadius = 0.5f;
        float colliderHeight = 2.0f;
        if (_contactDegrees >= minimumGroundAngle)
        {
            if (!Physics.Raycast(gameObject.transform.position, Vector3.down, PlayerCollider.radius + 0.71f, LayerMask_Ground))
            {
                // Slightly extends the player's CharacterController collider to forcibly push away from walls
                // Think like a Pinball bumper.
                colliderRadius += 0.1f;
                colliderHeight += 0.1f;

                Vector3 playerPos = gameObject.transform.position;
                Vector3 hitPos = _hit.point;
                playerPos.y = 0;
                hitPos.y = 0;
                Vector3 dir = hitPos - playerPos;
                dir.Normalize();
                // Debug.DrawRay(gameObject.transform.position, -dir, Color.yellow);

                // Creates a percentage between the minimum degree to begin pushing (at 0.0f) to max speed (1.0f)
                float pushPerc = (_contactDegrees - minimumGroundAngle) / (maximumGroundAngle - minimumGroundAngle);
                pushPerc *= pushPercMultiplier;

                pushPerc = Mathf.Clamp(pushPerc, minimumPushMagnitude, maximumPushMagnitude);

                CliffPushVelocity = (-dir * pushPerc);
            }
        }
        else
            CliffPushVelocity = new Vector3();

        PlayerCollider.radius = colliderRadius;
        PlayerCollider.height = colliderHeight;

        return CliffPushVelocity * Time.fixedDeltaTime;
    }

    GameObject LastViewedObject;
    void CursorRaycastOptions( RaycastHit hit )
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
