using UnityEngine;

public class TestController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitInput();
    }

    Vector2 yawInput;
    Vector2 pitchInput;
    Vector2 rollInput;

    float forwardAccel;
    float strafeAccel;
    float vertAccel;

    void InitInput()
    {
        yawInput = new Vector2();
        pitchInput = new Vector2();
        rollInput = new Vector2();

        forwardAccel = 0f;
        strafeAccel = 0f;
        vertAccel = 0f;

        velocity = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        UpdateRot();
        UpdateAccel();

        print(yawInput);
    }

    void UpdateInput()
    {
        #region Yaw Pitch Roll
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            yawInput.x -= Time.deltaTime;

            if (yawInput.x < -1f) yawInput.x = -1f;
        }
        else if(Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            yawInput.x += Time.deltaTime;

            if (yawInput.x > 1f) yawInput.x = 1f;
        }
        else
        {
            if(yawInput.x < 0f)
            {
                yawInput.x += Time.deltaTime;

                if(yawInput.x > 0f)
                    yawInput.x = 0f;
            }
            else if(yawInput.x > 0f)
            {
                yawInput.x -= Time.deltaTime;

                if(yawInput.x < 0f)
                    yawInput.x = 0f;
            }
        }

        if(Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            pitchInput.x -= Time.deltaTime;

            if (pitchInput.x < -1f) pitchInput.x = -1f;
        }
        else if(Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            pitchInput.x += Time.deltaTime;

            if (pitchInput.x > 1f) pitchInput.x = 1f;
        }
        else
        {
            if (pitchInput.x < 0f)
            {
                pitchInput.x += Time.deltaTime;

                if (pitchInput.x > 0f)
                    pitchInput.x = 0f;
            }
            else if (pitchInput.x > 0f)
            {
                pitchInput.x -= Time.deltaTime;

                if (pitchInput.x < 0f)
                    pitchInput.x = 0f;
            }
        }

        if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Q))
        {
            rollInput.x -= Time.deltaTime;

            if (rollInput.x < -1f) rollInput.x = -1f;
        }
        else if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
        {
            rollInput.x += Time.deltaTime;

            if (rollInput.x > 1f) rollInput.x = 1f;
        }
        else
        {
            if (rollInput.x < 0f)
            {
                rollInput.x += Time.deltaTime;

                if (rollInput.x > 0f)
                    rollInput.x = 0f;
            }
            else if (rollInput.x > 0f)
            {
                rollInput.x -= Time.deltaTime;

                if (rollInput.x < 0f)
                    rollInput.x = 0f;
            }
        }

        #endregion Yaw Pitch Roll

        #region Accel
        if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            forwardAccel += Time.deltaTime;

            if (forwardAccel > 1f) forwardAccel = 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
        {
            forwardAccel -= Time.deltaTime;

            if (forwardAccel < -1f) forwardAccel = -1f;
        }
        else
        {
            if (forwardAccel < 0f)
            {
                forwardAccel += Time.deltaTime;

                if (forwardAccel > 0f)
                    forwardAccel = 0f;
            }
            else if (forwardAccel > 0f)
            {
                forwardAccel -= Time.deltaTime;

                if (forwardAccel < 0f)
                    forwardAccel = 0f;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            strafeAccel -= Time.deltaTime;

            if (strafeAccel < -1f) strafeAccel = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            strafeAccel += Time.deltaTime;

            if (strafeAccel > 1f) strafeAccel = 1f;
        }
        else
        {
            if (strafeAccel < 0f)
            {
                strafeAccel += Time.deltaTime;

                if (strafeAccel > 0f)
                    strafeAccel = 0f;
            }
            else if (strafeAccel > 0f)
            {
                strafeAccel -= Time.deltaTime;

                if (strafeAccel < 0f)
                    strafeAccel = 0f;
            }
        }
        #endregion Accel
    }

    [SerializeField] float RotationSpeed = 5f;
    void UpdateRot()
    {
        Vector3 shipEulers = gameObject.transform.eulerAngles;

        shipEulers.x += pitchInput.x * RotationSpeed * Time.deltaTime;
        shipEulers.y += yawInput.x * RotationSpeed * Time.deltaTime;
        shipEulers.z += rollInput.x * RotationSpeed * Time.deltaTime;

        gameObject.transform.eulerAngles = shipEulers;
    }

    [SerializeField] float MoveSpeed = 5f;
    [SerializeField] float MoveSpeed_MAX = 5f;
    Vector3 velocity;
    Vector3 prevVelocity;
    void UpdateAccel()
    {
        velocity.x += strafeAccel * MoveSpeed * Time.deltaTime;
        // velocity.y += strafeAccel * MoveSpeed * Time.deltaTime;
        velocity.z += forwardAccel * MoveSpeed * Time.deltaTime;

        if (velocity.x > MoveSpeed_MAX) velocity.x = MoveSpeed_MAX;
        else if (velocity.x < -MoveSpeed_MAX) velocity.x = -MoveSpeed_MAX;

        // velocity.y = Mathf.Clamp(velocity.y, -MoveSpeed_MAX, MoveSpeed_MAX);

        if (velocity.z >  MoveSpeed_MAX) velocity.z = MoveSpeed_MAX;
        else if(velocity.z < -MoveSpeed_MAX) velocity.z = -MoveSpeed_MAX;

        Vector3 forwardVelocity = gameObject.transform.rotation * velocity;

        gameObject.transform.position += forwardVelocity;
    }

}
