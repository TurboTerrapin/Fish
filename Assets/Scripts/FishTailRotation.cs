using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTailRotation : MonoBehaviour
{

    public GameObject fishTail = null;
    public GameObject fishFin = null;

    public GameObject bodyTailRotPt = null;
    public GameObject tailFinRotPt = null;


    public float maxTailAngle = 0f;
    public float tailAngle = 0f;
    public float maxFinAngle = 0f;
    public float finAngle = 0f;
    

    float tailChangeRate = 0f;
    float finChangeRate = 0f;

    public Boid boid = null;

    // Update is called once per frame
    void Update()
    {
        float tailCorrection = maxTailAngle / 90;
        float finCorrection = maxFinAngle / 90;
        tailAngle = Mathf.Sin(Time.time * boid.speed) * 180 / Mathf.PI * tailCorrection;
        finAngle = Mathf.Sin(Time.time * boid.speed) * 180 / Mathf.PI * finCorrection;


        if(tailAngle > maxTailAngle)
        {
            tailAngle = maxTailAngle;
        }
        else if (tailAngle < -maxTailAngle)
        {
            tailAngle = -maxTailAngle;
        }

        if(finAngle > maxFinAngle)
        {
            finAngle = maxFinAngle;
        }
        else if (finAngle < -maxFinAngle)
        {
            finAngle = -maxFinAngle;
        }


        tailChangeRate = (tailAngle - fishTail.transform.localEulerAngles.y);
        finChangeRate = (finAngle - fishFin.transform.localEulerAngles.y);


        fishTail.transform.RotateAround(bodyTailRotPt.transform.position, transform.up, tailChangeRate);
        fishFin.transform.RotateAround(tailFinRotPt.transform.position, transform.up, finChangeRate);
    }
}
