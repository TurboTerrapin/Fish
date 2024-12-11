using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap : MonoBehaviour
{

    public bool playerCamMode = true;

    public Camera playerCam = null;
    public Camera hookCam = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(playerCamMode)
            {
                playerCamMode = false;

                playerCam.gameObject.SetActive(false);

                //Swap to hook cam
                hookCam.gameObject.SetActive(true);
            }
            else
            {
                playerCamMode = true;

                

                //Swap to hook cam
                hookCam.gameObject.SetActive(false);
                playerCam.gameObject.SetActive(true);
            }
        }
    }
}
