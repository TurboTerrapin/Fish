using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{

    public List<GameObject> fish = new List<GameObject>();

    public GameObject fishPrefab = null;

    public Hook hook = null;
    public bool justLanded = false;
    public bool canLand = true;
    public int lastMovedFish = 0;

    void Start()
    {
        //hook = FindObjectOfType<Hook>();
    }


    void Update()
    {
        if (!hook.landed)
        {
            canLand = true;
        }
        else if (hook.landed && !justLanded && canLand)
        {
            justLanded = true;
        }
        else if(justLanded)
        {
            SpawnFish();
            canLand = false;
            justLanded = false;
        }
    }

    void SpawnFish()
    {
        
        int num = Random.Range(5, 10);

        for (int i = 0; i < num; i++)
        {

            float posX = hook.transform.position.x;
            float posY = hook.transform.position.y;
            float posZ = hook.transform.position.z;

            posX = Random.Range(posX - 10, posX + 10);
            posY = Random.Range(posY - 20, posY + 10);
            posZ = Random.Range(posZ - 10, posZ + 10);

            if (posY > 0)
            {
                posY = 0;
            }

            Vector3 newPos = new Vector3(posX, posY, posZ);
            if (fish.Count <= 30)
            {
                fish.Add(Instantiate<GameObject>(fishPrefab, newPos, Random.rotation));
            }
            else
            {
                lastMovedFish++;
                if(lastMovedFish >= 30)
                {
                    lastMovedFish = 0;
                }
                fish[lastMovedFish].gameObject.transform.position = newPos;
                fish[lastMovedFish].gameObject.transform.rotation = Random.rotation;
            }


        }

    }


}
