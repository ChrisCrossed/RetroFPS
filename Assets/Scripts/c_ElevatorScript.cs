using UnityEngine;
using UnityEngine.SceneManagement;

public class c_ElevatorScript : MonoBehaviour
{
    [SerializeField] float DoorClosedPercent = .15f;
    float f_DoorsOpenPerc; // 100% = Open, 0% = Closed
    bool f_DoorsOpen;
    [SerializeField] float DoorMoveTimeInSeconds = 0.25f;

    GameObject DoorLeftObject;
    GameObject DoorRightObject;
    
    GameObject ElevatorModel;
    GameObject DoorColliderObject;
    Collider DoorCollider;

    bool[] UnlockedFloors = new bool[10];
    GameObject ElevatorButtonMenu;
    GameObject[] ElevatorButtons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // This is when the door starts as closed. I know my naming logic is wrong but this works for now.
        f_DoorsOpen = true;
        f_DoorsOpenPerc = 1f;
        flip = f_DoorsOpen;

        ElevatorModel = transform.Find("Model").gameObject;

        DoorLeftObject = ElevatorModel.transform.Find("DoorLeft_Obj").gameObject;
        DoorRightObject = ElevatorModel.transform.Find("DoorRight_Obj").gameObject;
        DoorColliderObject = transform.Find("DoorCollider").gameObject;
        DoorCollider = DoorColliderObject.GetComponent<Collider>();

        ElevatorButtonMenu = ElevatorModel.transform.Find("ButtonMenu").gameObject;

        ElevatorButtons = new GameObject[10];
        for (int i = 0; i < 10; i++)
            ElevatorButtons[i] = ElevatorButtonMenu.transform.Find("mdl_Button_" + i).gameObject;

        for (int i = 0; i < UnlockedFloors.Length; i++)
        {
            UnlockedFloors[i] = false;
            ElevatorButtons[i].GetComponent<c_ElevatorButton>().SetLockedFloorState( UnlockedFloors[i] );
        }

        UnlockFloorNumber(1);
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

        if(Input.GetKeyDown(KeyCode.O))
        {
            UnlockFloorNumber(5);
            GoToFloorNumber(5);
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

    public void UnlockFloorNumber(int floorNum)
    {
        if (floorNum < 0 || floorNum > UnlockedFloors.Length)
            return;

        if(!UnlockedFloors[floorNum])
            UnlockedFloors[floorNum] = true;

        ElevatorButtons[floorNum].GetComponent<c_ElevatorButton>().SetLockedFloorState(UnlockedFloors[floorNum]);
    }

    public void GoToFloorNumber(int floorNum)
    {
        if (floorNum < 0 || floorNum > UnlockedFloors.Length)
            return;

        if (UnlockedFloors[floorNum])
        {
            SceneManager.LoadScene("Level_" + floorNum);
        }
    }
}
