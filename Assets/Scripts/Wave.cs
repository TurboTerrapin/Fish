using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    public float amplitude;
    public float frequency;
    public float speedOfWave;


    public Wave()
    {
        amplitude = 0f;
        frequency = 0f;
        speedOfWave = 0f;
    }

    public Wave(float amplitude, float frequency, float speedOfWave)
    {
        this.amplitude = amplitude;
        this.frequency = frequency;
        this.speedOfWave = speedOfWave;
    }


    public float calculateVertexPosition(Vector3 pos)
    {
        float yPos = Mathf.Sin(frequency * pos.x + frequency * pos.z + Time.time) * amplitude;


        return yPos;
    }


}
