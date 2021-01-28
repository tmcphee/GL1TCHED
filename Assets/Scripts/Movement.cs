using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D r;
    public float magnitude;
    public float topSpeed;
    private Vector2 v;

    // Update is called once per frame
    void Update()
    {
        v = new Vector2(Input.GetAxis("Horizontal") * magnitude, Mathf.Abs(Input.GetAxis("Vertical") * 2 * magnitude));

        if (r.rotation != 0)
        {
            r.MoveRotation(Mathf.Abs(r.rotation - 1f) + Random.Range(0, (r.velocity.x/topSpeed)*5));
        }

        if (Mathf.Abs(r.velocity.x) < topSpeed)
        {
            r.AddForce(v, ForceMode2D.Impulse);
        }
    }
}
