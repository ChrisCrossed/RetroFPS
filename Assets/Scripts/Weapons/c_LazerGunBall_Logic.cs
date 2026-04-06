using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

enum OrbState
{
    Inactive,
    ChargingUp,
    Active
}

public class c_LazerGunBall_Logic : MonoBehaviour
{
    public bool IsActive {get; private set;}
    MeshRenderer _MeshRenderer;
    SphereCollider _SphereCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _MeshRenderer = GetComponent<MeshRenderer>();
        _SphereCollider = GetComponent<SphereCollider>();

        SetBallState(OrbState.Inactive);

        // Remove the LazerBall from being parented to the Player object now that we've started the game
        gameObject.transform.parent = null;
    }

    void SetBallState(OrbState orbState)
    {
        IsActive = (orbState == OrbState.Active);

        _MeshRenderer.enabled = (orbState != OrbState.Inactive);
        _SphereCollider.enabled = (orbState != OrbState.Inactive);
    }

    public void FireOrb(Transform startingTransform)
    {
        perc = 0f;
        PercDir = false;

        ChargeUpPerc = 0f;
        WillExplode = false;

        // Set Transform Position to front of gun
        gameObject.transform.position = startingTransform.position;
        gameObject.transform.rotation = startingTransform.rotation;

        // Enable Visibility if Disabled
        SetBallState(OrbState.ChargingUp);

        // Set scale appropriately (And begin scaling up to max size)
        StartCoroutine( ScaleOrb( startingTransform ) );
    }

    void DisperseLazers()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(IsActive)
        {
            MovementUpdate();
            MaterialUpdate();
        }
    }

    static float MoveSpeed_MAX = 3.0f;
    static float MoveSpeed_SLOW = 1.5f;
    float MoveSpeed = 3.0f;
    void MovementUpdate()
    {
        gameObject.transform.position += gameObject.transform.forward * Time.deltaTime * MoveSpeed;
    }

    float perc;
    float MaterialTransitionTimer = 0.65f;
    bool PercDir;
    float percMin = 0.35f;
    float percMax = 1.0f;
    void MaterialUpdate()
    {
        float timerIncrement = Time.deltaTime / MaterialTransitionTimer;
        if (!PercDir) timerIncrement *= -1f;

        perc += timerIncrement;

        
        if (perc < percMin || perc > percMax)
        {
            perc = Mathf.Clamp(perc, percMin, percMax);
            PercDir = !PercDir;
        }

        Material[] matList = gameObject.GetComponent<MeshRenderer>().materials;
        // matList[0].color = new Color(matList[0].color.r, matList[0].color.g, matList[0].color.b, 1f);
        matList[1].color = new Color(matList[1].color.r, matList[1].color.g, matList[1].color.b, perc);

        gameObject.GetComponent<MeshRenderer>().materials = matList;
    }

    float OrbScaleSize_Start = 0.1f;
    float OrbScaleSize_Max = 1f;
    float OrbScale_Time = 1.5f;
    IEnumerator ScaleOrb(Transform gunFrontTransform)
    {
        Vector3 orbScale;
        
        float time = 0f;

        while (time < OrbScale_Time)
        {
            time += Time.deltaTime / OrbScale_Time;
            if(time > OrbScale_Time) time = OrbScale_Time;

            float scale = Mathf.Lerp(OrbScaleSize_Start, OrbScaleSize_Max, time / OrbScale_Time);

            orbScale = new Vector3(scale, scale, scale);

            gameObject.transform.localScale = orbScale;
            gameObject.transform.rotation = gunFrontTransform.rotation;
            // gameObject.transform.position = gunFrontTransform.position + (gameObject.transform.forward * scale);
            gameObject.transform.position = gunFrontTransform.position;

            yield return new WaitForEndOfFrame();
        }

        print("Started");

        SetBallState(OrbState.Active);

        StartCoroutine( DestroyOrbTimer() );

        yield return true;
    }

    float OrbTimer;
    IEnumerator DestroyOrbTimer()
    {
        OrbTimer = 10.0f;

        while (OrbTimer > 0f)
        {
            OrbTimer -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        SetBallState(OrbState.Inactive);

        yield return null;
    }

    float OrbChargeMoveSpeedMult = 0.5f;
    float ChargeUpPerc = 0f;
    float ChargeUpPerc_MaxTime = 1f;
    bool WillExplode = false;
    void ChargeOrb()
    {
        MoveSpeed = MoveSpeed_MAX * OrbChargeMoveSpeedMult;

        if(ChargeUpPerc < 1f)
        {
            ChargeUpPerc += Time.deltaTime / ChargeUpPerc_MaxTime;

            ChargeUpPerc = Mathf.Clamp(ChargeUpPerc, 0f, 1f);

            if(ChargeUpPerc == 1f) WillExplode = true;
        }
    }

}
