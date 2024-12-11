using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodRotation : MonoBehaviour
{
    public GameObject hand = null;
    public float mousePos = 0f;
    public float prevMousePos = 0f;

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.GetAxis("Mouse X");

        prevMousePos += mousePos;

        prevMousePos = Mathf.Clamp(prevMousePos, -90f, 90f);

        hand.transform.localRotation = Quaternion.AngleAxis(prevMousePos, -transform.right);
    }
}
