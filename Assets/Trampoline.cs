using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Andrew Greer:
 *   - handles bouncing off of a trampoline object
 */
public class Trampoline : MonoBehaviour
{
    public float bounciness;
    private Rigidbody2D player;

    // finds the player object
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }


    // if the player collides, apply a force opposite to the collision, multiplied by bounciness and impact velocity
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("player hit");
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 bounceVector = new Vector2(bounciness * collision.relativeVelocity.magnitude * Time.deltaTime * -contact.normal[0],
                                                   bounciness * collision.relativeVelocity.magnitude * Time.deltaTime * -contact.normal[1]);

                player.AddForce(bounceVector, ForceMode2D.Impulse);
            }
        }
    }
}
