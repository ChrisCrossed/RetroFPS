using UnityEngine;

public class c_LazerGunBall_Logic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void FireOrb()
    {
        perc = 0f;
        PercDir = false;

        ChargeUpPerc = 0f;
        WillExplode = false;

        // Set Transform Position to front of gun

        // Set scale appropriately (And begin scaling up to max size)

        // Enable Visibility if Disabled
    }

    // Update is called once per frame
    void Update()
    {
        MovementUpdate();
        MaterialUpdate();
    }

    static float MoveSpeed_MAX = 3.0f;
    static float MoveSpeed = 3.0f;
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

    float OrbChargeMoveSpeedMult = 0.5f;
    float ChargeUpPerc = 0f;
    float ChargeUpPerc_MaxTime = 1f;
    bool WillExplode = false;
    public void ChargeOrb()
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
