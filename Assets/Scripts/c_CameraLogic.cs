using UnityEngine;

public class c_CameraLogic : MonoBehaviour
{
    GameObject CameraPosObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CameraPosObj = GameObject.Find("CameraPos").gameObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = CameraPosObj.transform.position;
        transform.rotation = CameraPosObj.transform.rotation;
    }
}
