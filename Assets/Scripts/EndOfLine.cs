using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLine : MonoBehaviour
{
    public static EndOfLine Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
