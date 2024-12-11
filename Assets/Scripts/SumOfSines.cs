using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumOfSines : MonoBehaviour
{


    List<Wave> waves = new List<Wave>();

    public int numWaves = 4;

    public float maxAmplitude = 2f;
    public float maxFrequency = 2f;
    public float maxSpeed = 2f;

    public GameObject temp = null;
    public GameObject water = null;
    Mesh waterMesh = null;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numWaves; i++)
        {
            float amplitude = Random.Range(0, maxAmplitude);
            float frequency = Random.Range(0, maxFrequency);
            float speed = Random.Range(0, maxSpeed);

            Wave wave = new Wave(amplitude, frequency, speed);

            waves.Add(wave);
        }
        MeshFilter waterMeshFilter = water.GetComponent<MeshFilter>();
        waterMesh = waterMeshFilter.sharedMesh;
        //Debug.Log(waves[0] + " " + waves[1] + " " + waves[2] + " " + waves[3]);


    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < numWaves; i++)
        {
            float newY = waves[i].calculateVertexPosition(temp.transform.position);

            //Debug.Log(newY);
            Vector3 tempVec = new Vector3(0, newY, 0);
            temp.transform.position += tempVec;
        }

        //MeshRenderer waterMesh = water.GetComponent<MeshRenderer>();
        //Mesh waterMesh = water.GetComponent<Mesh>();
        

        for (int i = 0; i < waterMesh.vertices.Length; i++)
        {
            for (int j = 0; j < numWaves; j++)
            {
                float newY = waves[i].calculateVertexPosition(waterMesh.vertices[i]);

                //Debug.Log(newY);
                Vector3 tempVec = new Vector3(0, newY, 0);
                waterMesh.vertices[i] += tempVec;
            }
        }



    }
}
