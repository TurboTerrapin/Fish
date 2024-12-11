using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FishCaughtTracker : MonoBehaviour
{

    public static FishCaughtTracker Instance { get; private set; }

    public TextMeshProUGUI fishCounter = null;

    public int numFish = 0;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        fishCounter.text = numFish.ToString();
    }
}
