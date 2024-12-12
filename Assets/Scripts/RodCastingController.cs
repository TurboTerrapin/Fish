using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodCastingController : MonoBehaviour
{

    public GameObject hook = null;
    public GameObject mainCamera = null;
    public GameObject rodTip = null;


    public float force = 0f;
    public bool growing = true;
    public bool isReeling = false;
    public bool justReturned = false;
    public bool lockedBail = true;

    public Line lineController = null;

    public float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 dir = rodTip.transform.position - hook.transform.position;
        
        if (Input.GetMouseButton(0))
        {
            if (isReeling)
            {
                Reel();
            }
            else
            {
                WindUp();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (justReturned)
            {
                justReturned = false;
            }
            else if (!isReeling)
            {
                Cast();
                lineController.Cast();
            }
        }
        else if (hook.GetComponent<Hook>().landed && !lockedBail && dir.magnitude > lineController.length + 1.5)
        {
            lineController.AddPointToEnd();
        }

        if (Input.GetMouseButton(1) && timer > 0.2)
        {
            Release();
            timer = 0f;
        }
        timer += Time.fixedDeltaTime;
    }


    void WindUp()
    {

        if (growing)
        {
            force += 20f * Time.fixedDeltaTime;
        }
        else
        {
            force -= 20f * Time.fixedDeltaTime;
        }

        if(force < 0f)
        {
            growing = true;
        }
        if(force > 50f)
        {
            growing = false;
        }
    }

    void Cast()
    {

        hook.transform.position = rodTip.transform.position;
        hook.SetActive(true);

        hook.GetComponent<Rigidbody>().AddForce(mainCamera.transform.forward * force, ForceMode.Impulse);
        isReeling = true;
        lockedBail = false;
    }

    void Reel()
    {
        lockedBail = true;
        if ((transform.position - hook.transform.position).magnitude < 5)
        {
            hook.SetActive(false);
            //Hook has been returned to player
            isReeling = false;
            justReturned = true;
            //Reset the hook bools
            hook.GetComponent<Hook>().landed = false;
            hook.GetComponent<Hook>().DeleteFish();
            //Get rid of any points on the fishing line
            lineController.Clear();
            //Reset the hooks velocity and position
            hook.GetComponent<Rigidbody>().velocity = Vector3.zero;
            hook.transform.localPosition = new Vector3(0, 3, 0);
        }
        else
        {
            Vector3 dir = rodTip.transform.position - hook.transform.position;
            if (dir.magnitude >= lineController.length)
            {
                hook.GetComponent<Rigidbody>().AddForce(dir * 50 * Time.fixedDeltaTime);
            }
            
            if (hook.GetComponent<Hook>().landed && timer > 0.2)
            {
                lineController.RemovePointFromEnd();
                timer = 0f;
            }
        }

    }

    void Release()
    {
        lineController.AddPointToEnd();
        lockedBail = false;
    }

}
