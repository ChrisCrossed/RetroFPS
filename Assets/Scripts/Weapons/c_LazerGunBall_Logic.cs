using System.Collections;
using System.Collections.Generic;
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
    GameObject _PlayerObjectContainer;

    GameObject GO_LazerGun_Trigger;
    c_LazerGun_Trigger LazerGun_TriggerLogic;

    GameObject GO_WeaponCamera;
    GameObject GO_Player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _MeshRenderer = GetComponent<MeshRenderer>();
        _SphereCollider = GetComponent<SphereCollider>();
        _PlayerObjectContainer = GameObject.Find("PlayerObjectContainer");

        SetBallState(OrbState.Inactive);

        // Remove the LazerBall from being parented to the Player object now that we've started the game
        gameObject.transform.parent = _PlayerObjectContainer.transform;
        GO_LazerGun_Trigger = _PlayerObjectContainer.transform.Find("GO_LazerGun_Trigger").gameObject;
        LazerGun_TriggerLogic = GO_LazerGun_Trigger.GetComponent<c_LazerGun_Trigger>();

        GO_WeaponCamera = GameObject.Find("WeaponCamera").gameObject;
        GO_Player = GameObject.Find("Player").gameObject;
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

        float triggerWidth = 1;
        float triggerRaycastLength = 20;

        /// NOTE
        /// 
        /// This currently works, but I feel like I'm drinking crazy juice.
        /// I'm currently forgetting the difference between .forward and .eulerAngles for assigning the fwd dir of the trigger.
        /// I might have to revisit this in the future but it works for now.
        /// Where I'm confused: When assigning the fwd / pos of the trigger when no obj exists within raycast dist, the trigger pos is way off.
        /// Again, this works, but my brain is tired. It'll probably click when I'm less tired.

        // Messy, but combines the Horiz Euler of the player with the Vert Euler of the Camera to create the Forward Vector. DO NOT NORMALIZE.
        Vector3 triggerEuler = new Vector3(GO_WeaponCamera.transform.eulerAngles.x, GO_Player.transform.eulerAngles.y, 0f);

        // Midpoint Formula
        Vector3 fwdPoint = gunFrontTransform.position + (gunFrontTransform.forward * triggerRaycastLength);
        Vector3 triggerPosition = (gunFrontTransform.position + fwdPoint) / 2f;

        Vector3 triggerScale = new Vector3(triggerWidth, triggerWidth, triggerRaycastLength);

        RaycastHit _hit;
        LayerMask layerMask = LayerMask.GetMask("Default", "Geo");
        
        if (Physics.Raycast(gunFrontTransform.position, gunFrontTransform.forward, out _hit, triggerRaycastLength, layerMask))
        {
            print("Hit: " + _hit.collider.tag);

            triggerScale.z = Vector3.Distance(gunFrontTransform.position, _hit.point +(gunFrontTransform.forward * 0.5f * triggerWidth));
            triggerPosition = (gunFrontTransform.position + _hit.point) / 2f;
        }


        LazerGun_TriggerLogic.InitOrbTrigger(triggerPosition, triggerEuler, triggerScale);

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
    public void ChargeOrb()
    {
        print("CHARGE");

        MoveSpeed = MoveSpeed_MAX * OrbChargeMoveSpeedMult;

        if(ChargeUpPerc < 1f)
        {
            ChargeUpPerc += Time.deltaTime / ChargeUpPerc_MaxTime;

            ChargeUpPerc = Mathf.Clamp(ChargeUpPerc, 0f, 1f);

            if(ChargeUpPerc == 1f) WillExplode = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ENEMY_OBJ enemyObj = other.GetComponent<ENEMY_OBJ>();

        if (enemyObj)
        {
            enemyObj.Damage(77);
        }
    }

}
