using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D r;
    public float magnitude;
    public float topSpeed;
    public bool infiniteJump;
    public AudioSource hitSound;
    public AudioSource jumpSound;

    private bool onGround = false;

    void Start()
    {
        /*  Tyler McPhee
         *  Sets the players postion to spawn on the first checkpoint
         */
        r.transform.position = r.GetComponent<Checkpoint>().GetLastCheckpointPosition();
    }


    /*  Andrew Greer
        - checks if player collides with ground or glitchwall
        - plays an impact sound if player collides with something  
    */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            onGround = true;
        }
        else onGround = false;

        if (collision.gameObject.tag.Equals("Glitchwall"))
        {
            //if the player collides with a glitchwall, applies a sizeable force to help push them through
            foreach (ContactPoint2D contact in collision.contacts)
            {
                r.AddForce(magnitude * 25 * -contact.normal, ForceMode2D.Impulse);
            }
        }

        if(collision.relativeVelocity.magnitude > topSpeed/2)
        {
            hitSound.Play();
        }
    }


    /*  Andrew Greer
     *   - called once every frame
     *   - handles jumping, movement, and player rotation
     */
    void Update()
    {
        //self-righting force; returns player to vertical by 0.75 degrees/frame
        if (r.rotation != 0)
        {
            r.MoveRotation(Mathf.Abs(r.rotation - 0.75f));
        }

        //applies force if player hasn't exceeded top speed
        if (Mathf.Abs(r.velocity.x) < topSpeed)
        {
            r.AddForce(new Vector2(Input.GetAxis("Horizontal") * magnitude * 1.75f, 1f));
        }

        //checks if either player is on the ground or infiniteJump glitch is active; plays a jumping sound
        if (Input.GetButtonDown("Jump"))
        {
            if(onGround || infiniteJump)
            {
                onGround = false;
                r.AddForce(new Vector2(1f, magnitude * 1.75f), ForceMode2D.Impulse);
                jumpSound.Play();
            }
        }

        //if player falls off the map, spawn them at the last checkpoint
        if(r.transform.position.y < -15)
        {
            r.GetComponent<Checkpoint>().SetPlayerLastCheckpoint();
        }
    }
}
