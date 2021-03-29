using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectScript : MonoBehaviour
{
    void Update()
    {
        /* Tyler McPhee
         * Used for object cleanup
         * When the object is out of the screen then detroy it
         */
        if (transform.position.y < -50)
        {
            Destroy(this.gameObject);
        }
    }
}
