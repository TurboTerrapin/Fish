using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{

    public float viewAngle = 90;

    public List<GameObject> boids = new List<GameObject>();

    public GameObject target = null;
    public GameObject hook = null;
    public GameObject rodTip = null;

    private Rigidbody rb = null;

    public float separationRadius = 0.5f;
    public float detectionRadius = 2f;
    public float maxSeparationForce = 5f;
    public float maxCohesionForce = 3f;
    public float maxAlignmentForce = 10f;

    private int boidCount = 0;

    public float maxSpeed = 10f;
    public float minSpeed = 0f;
    public float speed = 5f;

    public float maxSteer = 5f;

    public float timerInterval = 3f;
    public float timer = 0f;
    public Quaternion newRotation = new Quaternion();


    public float maxDeltaAngle = 10f;
    public float oldYaw = 0f;
    public float oldPitch = 0f;

    public bool targetMode = false;
    public bool hooked = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        speed = (maxSpeed + minSpeed) / 2;
        rb.velocity = transform.forward * speed;
        oldYaw = transform.rotation.eulerAngles.y;
        oldPitch = transform.rotation.eulerAngles.x;

        rodTip = FindObjectOfType<RodCastingController>().rodTip;
        hook = FindObjectOfType<Hook>().gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward * 10);
        timer += Time.fixedDeltaTime;

        if (timer > timerInterval)
        {
            NewSpeedAndDirection();
            timerInterval = Random.Range(0f, 3f);
            timer = 0f;
        }

        if (hooked)
        {
            //Get the vector that points from the player to the fish
            Vector3 dir = rodTip.transform.position - hook.transform.position;

            float angle = Vector3.Angle(transform.forward, dir);

            //Debug.Log(angle);


            if(angle < 90)
            {
                dir = rodTip.transform.position - transform.position;

                newRotation = LookAt(dir);
            }


            transform.position = hook.transform.position;

        }
        else if (targetMode)
        {
            //Get the vector that points from the fish to the target
            Vector3 dir = transform.position - target.transform.position;

            newRotation = LookAt(dir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, maxSteer);

        if (Vector3.Angle(hook.transform.position - transform.position, transform.forward) > viewAngle)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, hook.transform.position - transform.position, out hit, 10f))
            {

                Debug.DrawRay(transform.position, transform.position - hook.transform.position, Color.magenta);

                if (hit.collider.gameObject.CompareTag("Hook"))
                {
                    target.transform.position = hook.transform.position;
                    targetMode = true;
                }
            }
            else
            {
                targetMode = false;
            }
        }

        CheckAllBoids();

        if (transform.position.y > 0)
        {
            rb.AddForce(-Vector3.up * 9.81f * 50 * Time.fixedDeltaTime);
        }
        else if(hooked)
        {
            hook.GetComponent<Rigidbody>().AddForce(transform.forward * speed * speed * Time.fixedDeltaTime);
        }
        else
        {
            rb.AddForce(transform.forward * speed * speed * Time.fixedDeltaTime);
            rb.AddForce(-rb.velocity.normalized * rb.velocity.sqrMagnitude * Time.fixedDeltaTime * 100);
        }
    }


    Quaternion LookAt(Vector3 dir)
    {
        //A homemade, quaternion based LookAt function
        //Calculate y/hypotenuse
        float newPitchAngle = dir.y / dir.magnitude;
        //Calculate z/y
        float newYawAngle = dir.z / dir.x;

        //Calculate the vertical angle of the direction vector
        newPitchAngle = Mathf.Asin(newPitchAngle);
        //Convert to degrees
        newPitchAngle *= 180 / Mathf.PI;

        //Calculate the angle between the direction and the x unit vector
        newYawAngle = Mathf.Atan(newYawAngle);
        newYawAngle *= -180 / Mathf.PI;
        newYawAngle -= 90;

        //Since tangent is bounded to -90 to 90 degrees, we have to correct for when x<0
        if (dir.x < 0)
        {
            newYawAngle += 180;
        }

        //Create new quaternions and apply the rotation to them
        Quaternion newPitch = Quaternion.AngleAxis(newPitchAngle, Vector3.right);
        Quaternion newYaw = Quaternion.AngleAxis(newYawAngle, Vector3.up);

        //Apply the new rotation values to the old values
        oldPitch = newPitchAngle;
        oldYaw = newYawAngle;

        //Apply the new rotation
        return (newYaw * newPitch);
    }

    void NewSpeedAndDirection()
    {

        speed = Random.Range(minSpeed, maxSpeed);
        if (hooked)
        {
            if(speed > 1 && speed < 4)
            {
                speed = 4;
            }

            if (speed >= maxSpeed - 3f)
            {
                speed = maxSpeed * 5f;
            }
        }
        else
        {
            if (speed > 1 && speed < 5)
            {
                speed = 0;
            }
            if (speed >= maxSpeed - .5f)
            {
                speed = maxSpeed * 10f;
            }
        }
        //speed = 0;
        if (!targetMode)
        {
            Quaternion newYaw = Quaternion.identity;
            Quaternion newPitch = Quaternion.identity;

            float newYawAngle = Random.Range(-maxDeltaAngle, maxDeltaAngle);
            float newPitchAngle = Random.Range(-maxDeltaAngle, maxDeltaAngle);

            Vector2 angle = new Vector2(newYawAngle, newPitchAngle);
            if (angle.magnitude > maxDeltaAngle)
            {
                angle.Normalize();
                angle *= maxDeltaAngle;
            }

            oldYaw += angle.x;
            oldPitch += angle.y;

            newYaw = Quaternion.AngleAxis(oldYaw, Vector3.up);
            newPitch = Quaternion.AngleAxis(oldPitch, Vector3.right);

            newRotation = newYaw * newPitch;
        }
    }

    void CheckAllBoids()
    {
        Vector3 center = Vector3.zero;
        Vector3 heading = Vector3.zero;
        foreach(GameObject boid in boids)
        {
            Color color = Color.green;
            
            
            if (Vector3.Angle(boid.transform.position - transform.position, transform.forward) > viewAngle)
            {
                color = Color.red;
            }
            else
            {
                Separation(boid);

                center += boid.transform.position;
                heading += boid.transform.forward.normalized;
                boidCount += 1;
            }
            Debug.DrawLine(boid.transform.position, transform.position, color);
        }
        if (boidCount > 0)
        { 
            center /= boidCount;
            heading /= boidCount;
            Cohesion(center);
            Alignment(heading);
        }
        boidCount = 0;
    }

    private void Separation(GameObject boid)
    {
        Vector3 dist = transform.position - boid.transform.position;

        float magnitude = dist.magnitude;
        dist.Normalize();
        //dist *= (1 /  magnitude) * separationRadius;
        /*
        magnitude /= separationRadius;
        if(magnitude > 1)
        {
            magnitude = 1;
        }

        magnitude = Mathf.Cos(magnitude);

        if(magnitude < 0)
        {
            magnitude *= -1;
        }
        */

        magnitude *= maxSeparationForce;

        dist *= magnitude;

        /*
        if(dist.magnitude > 5)
        {
            dist.Normalize();
            dist *= 5;
        }
        */

        //Debug.Log(magnitude);

        rb.AddForce(dist * 50 * Time.fixedDeltaTime);
    }


    private void Cohesion(Vector3 center)
    {
        Vector3 dir = center - transform.position;

        
        float magnitude = dir.magnitude;
        dir.Normalize();
        /*
        magnitude /= separationRadius;
        if (magnitude > 1)
        {
            magnitude = 1;
        }

        magnitude = Mathf.Cos(magnitude);

        if (magnitude < 0)
        {
            magnitude *= 0;
        }
        */
        magnitude *= maxCohesionForce;
        
        dir *= magnitude;

        rb.AddForce(dir * 50 * Time.fixedDeltaTime);
    }

    private void Alignment(Vector3 heading)
    {
        heading *= maxAlignmentForce;

        rb.AddForce(heading * 50 * Time.fixedDeltaTime);
    }    
    public void ClearNeighbors()
    {
        foreach (GameObject boid in boids)
        {
            boid.GetComponent<Boid>().boids.Remove(gameObject);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Boid"))
        {
            boids.Add(other.gameObject);
        }
        if(other.gameObject.CompareTag("Hook"))
        {
            if (!other.gameObject.GetComponent<Hook>().fishOn)
            {
                other.gameObject.GetComponent<Hook>().SetFish(gameObject);
                hooked = true;
            }
            

        }

    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Boid"))
        {
            boids.Remove(other.gameObject);
        }
    }

}
