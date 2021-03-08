using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D r;
    public float magnitude;
    public float topSpeed;
    public float topclimbspeed;
    public bool infiniteJump;
    public bool screenWarp;
    public AudioSource hitSound;
    public AudioSource jumpSound;

    private bool onGround = false;
    private bool onClimbable = false;

    private bool isWrappingX = false;
    private bool isWrappingY = false;
    

    Renderer[] renderers;


    void Start()
    {
        r = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        /*  Tyler McPhee
         *      -Sets the players postion to spawn on the first checkpoint
         *  Troy Walther
         *      -Store character renderer(s) in array
         */
        r.transform.position = r.GetComponent<Checkpoint>().GetLastCheckpointPosition();

        renderers = GetComponentsInChildren<Renderer>();

    }


    /*  Andrew Greer
        - checks if player collides with ground or glitchwall
        - plays an impact sound if player collides with something  
    */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*  Tyler McPhee
         *  Checks if the player collided with an enemy
         *  if true respawn enemy
         */
        if (collision.gameObject.tag == "Enemy")
        {
            r.GetComponent<Checkpoint>().SetPlayerLastCheckpoint();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
        else onGround = false;

        if (collision.gameObject.CompareTag("Glitchwall"))
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

    /*  Tyler McPhee
     *  When the player enters an object that has trigger enabled
     */
    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.CompareTag("Spike"))
        {
            r.GetComponent<Checkpoint>().SetPlayerLastCheckpoint();
        }
    }

    /*  Tyler McPhee
     *  When the player is in an object that has trigger enabled
     */
    void OnTriggerStay2D(Collider2D Collider)
    { 
        //Check if the object has climbable in its tag. If so allow the player to climb
        if(Collider.CompareTag("Climbable"))
        {
            onClimbable = true;
        }
    
    }

    /*  Tyler McPhee
     *  When the player leaves an object that has trigger enabled
     */
    void OnTriggerExit2D(Collider2D Collider)
    {
        //Check if the object has climbable in its tag. Stop the player from climbing
        if (Collider.CompareTag("Climbable"))
        {
            onClimbable = false;
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

        /*  Tyler McPhee
         *  Apply force for climbable objects
         *  Recycled and modified code from Andrew
         */
        if (Mathf.Abs(r.velocity.y) < topclimbspeed && onClimbable)
        {
            r.AddForce(new Vector2(1f, Input.GetAxis("Vertical") * magnitude * 1.25f));
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
        if(r.transform.position.y < -50)
        {
            r.GetComponent<Checkpoint>().SetPlayerLastCheckpoint();
        }

        //if player is off screen and screen warp is enabled, wrap player to opposite side of screen
        if ((!onScreen()) && screenWarp)
        {
            wrapScreen();
        }
        else 
        {
            isWrappingY = false;
            isWrappingX = false;
        }
    }

    /*Troy Walther
     * -Check if character is on screen
     * -Return: True if character is on screen, false if character is off screen
     */
    private bool onScreen() 
    {
        //loop through all children components
        foreach (var renderer in renderers)
        {
            // If at least one render is visible, return true
            if (renderer.isVisible)
            {
                return true;
            }
        }
        // Otherwise, the object is invisible
        return false;
    }

    /*Troy Walther
     * -Teleport character to different region of screen
     */
    private void wrapScreen() 
    {
        //if the character is already wrapping don't do it again
        if (isWrappingX && isWrappingY) 
        {
            return;
        }

        //get main camera Veiwport coordinate (0,0 - 1,1) and get current transform position
        Camera cam = Camera.main;
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
        Vector3 newPos  = transform.position;

        //if the character is outside the screen on the X plane, inverse the X value, and set WrappingY = true
        if (!isWrappingX && (viewPos.x > 1 || viewPos.x < 0)) 
        {
            newPos.x *= -1;
            isWrappingX = true;
        }

        //if the character is outside the screen on the Y plane, inverse the Y value, and set WrappingY = true
        if (!isWrappingY && (viewPos.y > 1 || viewPos.y < 0))
        {
            newPos.y *= -1;
            isWrappingY = true;
        }

        //apply the transformation, if you haven't wrapped, nothing will happen here
        transform.position = newPos;
    }
}
