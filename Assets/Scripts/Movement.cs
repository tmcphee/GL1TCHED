﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Movement : MonoBehaviour
{
    
    public float magnitude;
    public float topSpeed;
    public float topclimbspeed;
    public AudioSource hitSound;
    public AudioSource jumpSound;
    public AudioSource enemySound;

    //public flags
    public bool canRotate;
    public bool canMove = true;
    public bool infiniteJump;
    public bool screenWarp;

    //private flags
    private bool onGround = false;
    private bool onClimbable = false;    
    private bool isWrappingX = false;
    private bool isWrappingY = false;

    private Rigidbody2D r;
    private GameObject dummyModel;
    private GameObject sprite;
    private Animator anim;
    private SpriteRenderer sr;
    private float hAxis;
    Renderer[] renderers;
    



    void Start()
    {
        r = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        dummyModel = GameObject.FindGameObjectWithTag("dummy_mesh");

        sprite = GameObject.FindGameObjectWithTag("Player_2D");
        try
        {
            anim = sprite.GetComponent<Animator>();
            sr = sprite.GetComponent<SpriteRenderer>();
            Debug.Log("Passed");
            Debug.Log(anim.GetBool("idle"));
        }
        catch (Exception e)
        {
            anim = null;
            Debug.Log("Failed");

        }

            /*  Tyler McPhee
             *    - Sets the players postion to spawn on the first checkpoint
             *      
             *  Troy Walther
             *    - Store character renderer(s) in array
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
         *      Checks if the player collided with an enemy
         *      if true respawn enemy
         */
        if (collision.gameObject.CompareTag("Enemy"))
        {
            r.GetComponent<Checkpoint>().SetPlayerLastCheckpoint();
            enemySound.Play();
            collision.collider.GetComponent<Enemy>().ResetPosition();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }

        if (collision.gameObject.CompareTag("Glitchwall"))
        {
            //if the player collides with a glitchwall, applies a sizeable force to help push them through (UNUSED)
            foreach (ContactPoint2D contact in collision.contacts)
            {
                r.AddForce(magnitude * Time.deltaTime * 25 * -contact.normal, ForceMode2D.Impulse);
            }
        }

        if(collision.relativeVelocity.magnitude * Time.deltaTime * 15f > 4.25f)
        {
            hitSound.Play();
        }
    }


    /*  Tyler McPhee
     *      When the player enters an object that has trigger enabled
     */
    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.CompareTag("Spike"))
        {
            r.GetComponent<Checkpoint>().SetPlayerLastCheckpoint();
        }
    }


    /*  Tyler McPhee
     *      When the player is in an object that has trigger enabled
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
     *      When the player leaves an object that has trigger enabled
     */
    void OnTriggerExit2D(Collider2D Collider)
    {
        //Check if the object has climbable in its tag. Stop the player from climbing
        if (Collider.CompareTag("Climbable"))
        {
            onClimbable = false;
        }
    }


    // Andrew Greer: additional check to prevent a bug where jumping would stop working if the player collided with a non-ground object
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !onGround && Mathf.Abs(collision.relativeVelocity.y) < 0.1)
        {
            onGround = true;
        }
    }


    /*  Andrew Greer:
     *   - called once every frame
     *   - handles jumping, movement, and player rotation
     *   
     *   Troy Walther:
     *   - rotation lock for certain levels
     *   - Fixed control after reaching max speed
     */
    void Update()
    {
        // storing Horizontal axis as it would otherwise be a very frequent function call
        hAxis = Input.GetAxis("Horizontal");

        /* Tyler McPhee
         *      Check if the player is allowed to move
         *      in not exit
         */
        if (!canMove)
        {
            return;
        }

        //self-righting force; returns player to vertical by 0.75 degrees/frame
        if (r.rotation != 0)
        {
            r.MoveRotation(Mathf.Abs(r.rotation - 0.75f));
        }
        if (canRotate == false) 
        {
            r.rotation = 0;
        }

        //applies force over time if player hasn't exceeded top speed (or an instantaneous force if the player wants to change direction)
        if (Mathf.Abs(r.velocity.x) < topSpeed)
        {
            r.AddForce(new Vector2((hAxis * magnitude  * 120000f) * Time.deltaTime, 0f));
        } 
        if (hAxis != 0 && Mathf.Sign(r.velocity.x) != Mathf.Sign(hAxis))
        {
            r.AddForce(new Vector2(hAxis * magnitude * 120000f * Time.deltaTime, 0f), ForceMode2D.Impulse);
        }


        /*  Tyler McPhee
         *      Apply force for climbable objects
         *      Recycled and modified code from Andrew
         */
        if (Mathf.Abs(r.velocity.y) < topclimbspeed && onClimbable && (Input.GetAxis("Vertical") != 0))
        {
            r.AddForce(new Vector2(1f, (Input.GetAxis("Vertical") * magnitude * 125000f) * Time.deltaTime));
            r.velocity = new Vector3(0, r.velocity.y, 0);
            onGround = false;
        }

        //checks if either player is on the ground or infiniteJump glitch is active; plays a jumping sound; plays jumping animation if using the v2 character model
        if (Input.GetButtonDown("Jump"))
        {
            if (onGround || infiniteJump)
            {
                r.AddForce(new Vector2(0f, magnitude * 225f), ForceMode2D.Impulse);
                if(dummyModel != null)
                {
                    dummyModel.GetComponentInChildren<dummyAnimations>().PlayJumpingAnimation();
                }
                jumpSound.Play();
                onGround = false;
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

        if (anim != null)
        {
            animate();
        }
    }


    /*Troy Walther
     *  - Check if character is on screen
     *  - Return: True if character is on screen, false if character is off screen
     */
    private bool onScreen() 
    {
        //loop through all children components
        if (renderers == null) 
        {
            return false;
        }
        foreach (Renderer renderer in renderers)
        {
            if (renderer == null) 
            {
                return false;
            }
        }
        foreach (Renderer renderer in renderers)
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
     * - Teleport character to different region of screen
     */
    private void wrapScreen() 
    {
        //if the character is already wrapping don't do it again
        if (isWrappingX && isWrappingY) 
        {
            return;
        }

        //get main camera Viewport coordinate (0,0 - 1,1) and get current transform position
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


    // Andrew Greer: exposed method for other scripts to check if player is on the ground (UNUSED)
    public bool isOnGround()
    {
        return onGround;
    }

    /*Troy Walther
     * -  Basic function to set values of animator transitions
     */
    private void animate() {

        //Run or Idle
        if (r.velocity.x > 1 || r.velocity.x < -1)
        {
            sr.flipX = (r.velocity.x < 0);
            anim.SetBool("idle", false);
        }
        else 
        {
            anim.SetBool("idle", true);
        }

        anim.SetBool("Jumping", onGround ? false : true);
    
        return;
    }


}
