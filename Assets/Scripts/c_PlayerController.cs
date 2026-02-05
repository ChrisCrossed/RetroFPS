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

        PlayerCollider = gameObject.GetComponent<CapsuleCollider>();
        PlayerController = gameObject.GetComponent<CharacterController>();
        CameraObject = gameObject.transform.Find("Main Camera").gameObject;

        TextUI = GameObject.Find("Text");
    }

    void START_Settings()
    {
        MouseCursorState(false);
    }


    // Update is called once per frame
    void Update()
    {
        UPDATE_GetPlayerInput();
        UPDATE_PlayerLook();
        UPDATE_PlayerMovement();
        print(PlayerController.velocity.magnitude);
    }

    Vector2 v2_PlayerInputVector;
    Vector2 v2_PlayerMoveVector;
    InputAction IA_Move;

    InputAction IA_Look;
    Vector2 v2_MouseInput;
    [SerializeField] float HorizontalLookMultiplier = 5f;
    [SerializeField] float VerticalLookMultiplier = 5f;
    void UPDATE_GetPlayerInput()
    {
        v2_PlayerInputVector = IA_Move.ReadValue<Vector2>();
        v2_PlayerInputVector.Normalize();

        v2_MouseInput = IA_Look.ReadValue<Vector2>();
    }

    float CameraAngle = 0f;
    void UPDATE_PlayerLook()
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
        int layerMask = LayerMask.GetMask("Ground", "GameObject");

        if(Physics.Raycast(CameraObject.transform.position, CameraObject.transform.forward, out _hit, 1000f, layerMask))
        {
            Debug.DrawLine(CameraObject.transform.position, _hit.point, Color.red);

            CursorRaycastOptions(_hit);
        }
    }

    float VelocitySpeedMult = 8f;
    int LayerMask_Ground;
    float Gravity = -9.81f * 3.5f;
    float yVel;
    Vector3 CliffPushVelocity;
    float CliffPushSpeed = 3f;
    GameObject TextUI;
    void UPDATE_PlayerMovement()
    {
        #region Convert player input into desired movement velocity
        v2_PlayerMoveVector = MovementVelocityPerc(v2_PlayerMoveVector, v2_PlayerInputVector);

        Vector3 v3_InputVector = new Vector3(v2_PlayerMoveVector.x, 0f, v2_PlayerMoveVector.y);
        #endregion Convert player input into desired movement velocity

        #region Cast downward to ground

        RaycastHit _hit;
        Vector3 playerVector = new Vector3();

        LayerMask_Ground = LayerMask.GetMask("Ground");
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

            Debug.DrawRay(gameObject.transform.position, v3_InputVector * 5f, Color.red);

            PlayerController.Move(-Vector3.up * 0.1f);

            #region Cliff Edge Check

            string cliffMagText = "";
            if (contactDegrees >= 20f)
            {
                if (!Physics.Raycast(gameObject.transform.position, Vector3.down, PlayerCollider.radius + 0.71f, LayerMask_Ground))
                {
                    Vector3 playerPos = gameObject.transform.position;
                    Vector3 hitPos = _hit.point;
                    playerPos.y = 0;
                    hitPos.y = 0;
                    Vector3 dir = hitPos - playerPos;
                    dir.Normalize();
                    Debug.DrawRay(gameObject.transform.position, -dir, Color.yellow);

                    float pushPerc = (contactDegrees - 20f) / 50f;
                    // print(pushPerc);
                    CliffPushVelocity = (-dir * pushPerc * CliffPushSpeed * Time.deltaTime);
                    //pushPerc *= 50f;
                    //CliffPushVelocity = (-dir * pushPerc * Time.deltaTime);
                    //cliffMagText = CliffPushVelocity.magnitude.ToString();
                }
            }
            else
                CliffPushVelocity = new Vector3();

            TextUI.GetComponent<Text>().text = cliffMagText;
            /*
            if (contactDegrees >= 20f)
            {
                if(!Physics.Raycast(gameObject.transform.position, Vector3.down, PlayerCollider.radius + 0.71f, LayerMask_Ground))
                {
                    Vector3 playerPos = gameObject.transform.position;
                    Vector3 hitPos = _hit.point;
                    playerPos.y = 0;
                    hitPos.y = 0;
                    Vector3 dir = hitPos - playerPos;
                    dir.Normalize();
                    Debug.DrawRay(gameObject.transform.position, -dir, Color.yellow);

                    float pushPerc = (contactDegrees - 20f) / 50f;
                    pushPerc *= 50f;
                    CliffPushVelocity = (-dir * pushPerc * Time.deltaTime);
                }
                else
                    CliffPushVelocity = new Vector3();
            }
            else
                CliffPushVelocity = new Vector3();
            */

            playerVector += CliffPushVelocity;

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

    void CursorRaycastOptions( RaycastHit hit )
    {
        switch( hit.collider.tag )
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

            case "ElevatorButton":
                hit.transform.GetComponent<c_ElevatorButton>().LookAtButton();
                break;

            default:
                break;
        }
    }

    #endregion Gameplay Functions

}
