using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    Color c;

    void Start()
    {
        c = GetComponent<Light>().color;
    }

    void Update()
    {
        //generates a random vector and slightly varies the background colour from fram eto fram
        Color mutator = new Vector4(RandomFloat(10000f), RandomFloat(10000f), RandomFloat(10000f), 1f);
        c += mutator * Time.deltaTime;
    }

    //Random.Range() except it returns a random float between -0.5f and 0.5f (Works best with large values of max)
    float RandomFloat(float max) { return ((Random.Range(0, max) - (0.5f * max)) / max); }
}