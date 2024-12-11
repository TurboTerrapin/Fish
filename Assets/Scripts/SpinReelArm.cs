using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinReelArm : MonoBehaviour
{

    public GameObject spinPoint = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.AngleAxis(200 * Time.time, transform.forward);



        transform.RotateAround(spinPoint.transform.position, transform.forward, 10);
    }
}
