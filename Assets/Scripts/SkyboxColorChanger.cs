using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//slightly adjusts the color of the skybox over time using randomly generated vectors
public class SkyboxColorChanger : MonoBehaviour
{
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Color mutator = new Vector4(RandomFloat(10000), RandomFloat(10000), RandomFloat(10000), 1f);
        cam.backgroundColor += (mutator * 4) * Time.deltaTime;

        //Debug.Log(mutator + "\t" + cam.backgroundColor);
    }

    //Random.Range() except it returns a random float between -0.5f and 0.5f (Works best with large values of max but not too large)
    float RandomFloat(float max) { return ((Random.Range(0, max) - (0.5f * max)) / max); }
}
