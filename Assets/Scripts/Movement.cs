using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D r;
    public float magnitude;
    public float topSpeed;

    // Update is called once per frame
    void Update()
    {
        if (r.rotation != 0)
        {
            r.MoveRotation(Mathf.Abs(r.rotation - 0.5f));
        }

        if (Mathf.Abs(r.velocity.x) < topSpeed)
        {
            r.AddForce(new Vector2(Input.GetAxis("Horizontal") * magnitude * 1.75f, 1f));
        }

        if (Input.GetButtonDown("Jump"))
        {
            r.AddForce(new Vector2(1f, magnitude * 2.5f), ForceMode2D.Impulse);
        }
    }
}
