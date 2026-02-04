using System.Net;
using UnityEngine;

public class c_Logo_Level10Games : MonoBehaviour
{
    GameObject whiteBackdropObject;
    GameObject endPoint;
    float endpointX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        whiteBackdropObject = transform.Find("WhiteBackdrop").gameObject;
        endPoint = whiteBackdropObject.transform.Find("EndPoint").gameObject;
        endpointX = endPoint.transform.position.x;

        MoveSpeed = 13000f / 2.5f;
    }

    bool active;
    float MoveSpeed;
    float waitTime = 1.0f;
    // Update is called once per frame
    void Update()
    {
        if(waitTime > 0f)
        {
            waitTime -= Time.deltaTime;

            if(waitTime < 0f)
            {
                waitTime = 0f;
                active = true;
            }
        }

        if (active)
        {
            Vector3 pos = whiteBackdropObject.transform.position;
            if (pos.x > -endpointX)
            {
                pos.x -= Time.deltaTime * MoveSpeed;

                if (pos.x < -endpointX)
                    pos.x = -endpointX;

                whiteBackdropObject.transform.position = pos;
                print(pos.x);
            }
            else active = false;
        }
    }
}
