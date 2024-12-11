using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    public GameObject boat = null;
    public Rigidbody rb = null;

    public float speed = 10f;
    public float input = 0f;

    void Start()
    {
        rb = boat.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        input = Input.GetAxis("Vertical");

        rb.AddForce(transform.forward * input * Time.fixedDeltaTime * speed);
        rb.AddForce(-rb.velocity.normalized * rb.velocity.sqrMagnitude * Time.fixedDeltaTime * 100);
    }
}
