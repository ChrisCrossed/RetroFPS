using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class c_PlayerController : MonoBehaviour
{
    CapsuleCollider PlayerCollider;
    CharacterController PlayerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        START_Connections();
    }

    void START_Connections()
    {
        IA_Move = InputSystem.actions.FindAction("Move");

        PlayerCollider = gameObject.GetComponent<CapsuleCollider>();
        PlayerController = gameObject.GetComponent<CharacterController>();

        
    }


    // Update is called once per frame
    void Update()
    {
        UPDATE_GetPlayerInput();
        UPDATE_PlayerMovement();

        if(Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene("Level_1");
            // SceneManager.UnloadSceneAsync("Level_2");
        }
        else if(Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("Level_2");
            // SceneManager.UnloadSceneAsync("Level_1");
        }
    }

    Vector2 v2_PlayerInputVector;
    Vector2 v2_PlayerMoveVector;
    InputAction IA_Move;
    void UPDATE_GetPlayerInput()
    {
        v2_PlayerInputVector = IA_Move.ReadValue<Vector2>();
        v2_PlayerInputVector.Normalize();
    }

    float VelocitySpeedMult = 8f;
    int LayerMask_Ground;
    float Gravity = -9.81f * 3.5f;
    float yVel;
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


        if (Physics.SphereCast(gameObject.transform.position, PlayerCollider.radius - 0.001f, Vector3.down, out _hit, PlayerCollider.radius + 0.25f, LayerMask_Ground))
        {
            yVel = 0f;

            v3_InputVector = Vector3.ProjectOnPlane(v3_InputVector, -_hit.normal);

            playerVector = gameObject.transform.rotation * v3_InputVector;

            Debug.DrawRay(gameObject.transform.position, playerVector * 100.0f, Color.red);
        }
        else
        {
            print("NOT Hit");

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

}
