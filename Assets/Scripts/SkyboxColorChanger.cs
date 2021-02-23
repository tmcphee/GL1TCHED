using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Troy Walther
 *  -slightly adjusts the color of the skybox over time between color ranges, if no Colors specified color change is random
 */
public class SkyboxColorChanger : MonoBehaviour
{
    Camera cam;
    public Vector4[] colors;
    public float changeSpeed;
    private bool rand;
    private Color mutator;
    private bool mutating = false;
    private Vector4 colorOriginal, colorGoal;


    // Start is called before the first frame update
    void Start()
    {

        // if there are no colors supplied make the skybox random

        if ((colors.Length == 0))   //For some reason there is a weird type casting issue if you don't add == 0?
        {
            mutator = new Vector4(RandomFloat(10000), RandomFloat(10000), RandomFloat(10000), 1f);
            rand = true;
        }
        else 
        {
            rand = false;
        }

        //Set a random colour for the initial background colour from the array of colours
        int element1 = Random.Range(0, colors.Length);
        colorOriginal = colors[element1];

        //Store the camera in a variable
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        //If no colour values are passed, randomly mutate the background colour
        if (rand)
        {
            cam.backgroundColor += (mutator * 4) * Time.deltaTime;
            return;
        }

        //if not mutating call function to begin mutating, otherwise modify color original color at specific speed 
        if (!mutating)
        {
            mutate();
        }
        else 
        {
            colorOriginal = Vector4.MoveTowards(colorOriginal, colorGoal, changeSpeed * Time.deltaTime);
        }

        //if the distance between the goal value and original value has become 0, set mutating to false
        if ( (Vector4.Distance(colorOriginal, colorGoal) == 0))
        {
            mutating = false;
        }

        //set Camera background color to colorOriginal
        cam.backgroundColor = colorOriginal;
        
    }

    //Random.Range() except it returns a random float between -0.5f and 0.5f (Works best with large values of max but not too large)
    float RandomFloat(float max) { return ((Random.Range(0, max) - (0.5f * max)) / max); }


    /* Troy Walther 
     *  -set the value of the color to mutate towards and set mutating to true 
     *
    */
    private void mutate() 
    {
        int randElement = Random.Range(0, colors.Length);

        colorGoal = colors[randElement];
        mutating = true;

    }

}
