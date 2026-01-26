using UnityEngine;

public class c_ElevatorScript : MonoBehaviour
{
    [SerializeField] float DoorClosedPercent = .15f;
    float f_DoorsOpenPerc; // 100% = Open, 0% = Closed
    bool f_DoorsOpen;
    [SerializeField] float DoorMoveTimeInSeconds = 0.25f;

    GameObject DoorLeftObject;
    GameObject DoorRightObject;
    
    GameObject DoorColliderObject;
    Collider DoorCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // This is when the door starts as closed. I know my naming logic is wrong but this works for now.
        f_DoorsOpen = true;
        f_DoorsOpenPerc = 1f;
        flip = f_DoorsOpen;

        DoorLeftObject = transform.Find("Model").Find("DoorLeft_Obj").gameObject;
        DoorRightObject = transform.Find("Model").Find("DoorRight_Obj").gameObject;
        DoorColliderObject = transform.Find("DoorCollider").gameObject;
        DoorCollider = DoorColliderObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    bool flip;
    void Update()
    {
        Update_DoorStatus();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            flip = !flip;

            if (flip) OpenDoors();
            else CloseDoors();
        }
    }

    void Update_DoorStatus()
    {
        if(f_DoorsOpen && f_DoorsOpenPerc < 1f)
        {
            f_DoorsOpenPerc += Time.deltaTime * (1f / DoorMoveTimeInSeconds);

            if (f_DoorsOpenPerc > 1f) f_DoorsOpenPerc = 1f;

            SetDoorPercentage(f_DoorsOpenPerc);
        }
        else if(!f_DoorsOpen && f_DoorsOpenPerc > 0f)
        {
            f_DoorsOpenPerc -= Time.deltaTime * (1f / DoorMoveTimeInSeconds);

            if (f_DoorsOpenPerc < 0f) f_DoorsOpenPerc = 0f;

            SetDoorPercentage(f_DoorsOpenPerc);
        }
    }

    void SetDoorPercentage(float perc)
    {
        // y = .85x + .15
        float doorPerc = ((1f - DoorClosedPercent) * perc) + (DoorClosedPercent);

        DoorLeftObject.transform.localScale = new Vector3(doorPerc, 1f, 1f);
        DoorRightObject.transform.localScale = new Vector3(doorPerc, 1f, 1f);
    }

    public void OpenDoors()
    {
        f_DoorsOpen = true;
        DoorCollider.isTrigger = false;
    }

    public void CloseDoors()
    {
        f_DoorsOpen = false;

        DoorCollider.isTrigger = true;
    }
}
