using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public GameObject boat = null;
    public GameObject player = null;

    public Vector2 mouseMove = Vector2.zero;

    public float mouseSensitivity = 100f;

    public Vector2 prevPos = Vector2.zero;
    public Vector2 boatPrevPos = Vector2.zero;

    public bool boatMode = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Gets mouse input
        mouseMove = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //Increases the sensitivity to movement
        mouseMove *= mouseSensitivity * Time.deltaTime;

        prevPos.y = Mathf.Clamp(prevPos.y, -90f, 90f);

        prevPos.y -= mouseMove.y;
        transform.localRotation = Quaternion.AngleAxis(prevPos.y, Vector3.right);


        if (boatMode)
        {
            boatPrevPos.x += mouseMove.x;

            boat.transform.rotation = Quaternion.AngleAxis(boatPrevPos.x, Vector3.up);
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            boatMode = true;
        }
        if (Input.GetKey(KeyCode.X))
        {
            boatMode = false;
        }
        else if(Input.GetKey(KeyCode.C))
        {
            prevPos.x += mouseMove.x;

            player.transform.localRotation = Quaternion.AngleAxis(prevPos.x, Vector3.up);
        }
    }
}
