using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    public Rigidbody hookRB;

    public GameObject currentFish = null;

    public bool landed = false;
    public bool fishOn = false;

    // Start is called before the first frame update
    void Start()
    {
        hookRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y > 0)
        {
            hookRB.AddForce(Vector3.up * -9.81f * 50 * Time.fixedDeltaTime);
        }
        else
        {
            if(!landed)
            {
                hookRB.velocity = Vector3.zero;
                landed = true;
            }
            else
            {
                hookRB.AddForce(-hookRB.velocity.normalized * hookRB.velocity.sqrMagnitude * Time.fixedDeltaTime * 50);
            }
        }
    }

    public void SetFish(GameObject fish)
    {
        fishOn = true;
        currentFish = fish;
    }

    public void DeleteFish()
    {
        fishOn = false;

        if (currentFish)
        {
            FishCaughtTracker.Instance.numFish++;
            currentFish.GetComponent<SphereCollider>().radius = 0.01f;
            currentFish.GetComponent<Boid>().ClearNeighbors();
            Destroy(currentFish);
        }

    }
}
