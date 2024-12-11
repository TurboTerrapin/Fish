using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHookCamera : MonoBehaviour
{

    public Vector2 mouseMove = Vector2.zero;
    public float mouseSensitivity = 100f;
    public Vector2 prevPos = Vector2.zero;
    // Update is called once per frame
    void LateUpdate()
    {
        //Gets mouse input
        mouseMove = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //Increases the sensitivity to movement
        mouseMove *= mouseSensitivity * Time.deltaTime;


        prevPos.y -= mouseMove.y;
        prevPos.x += mouseMove.x;

        transform.rotation = Quaternion.AngleAxis(prevPos.y, Vector3.right) * Quaternion.AngleAxis(prevPos.x, Vector3.up);
    }
}
