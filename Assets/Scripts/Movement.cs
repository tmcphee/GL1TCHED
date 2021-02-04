﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D r;
    public float magnitude;
    public float topSpeed;
    public bool infiniteJump;
    private bool onGround = false;

    void Start()
    {
        r.transform.position = r.GetComponent<Checkpoint>().GetLastCheckpointPosition();
    }

    //checks if player is touching the ground object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            onGround = true;
        }
        else onGround = false;
    }

    // Update is called once per frame
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

        //checks if either player is on the ground or infiniteJump is active
        if (Input.GetButtonDown("Jump"))
        {
            if(onGround || infiniteJump)
            {
                onGround = false;
                r.AddForce(new Vector2(1f, magnitude * 1.75f), ForceMode2D.Impulse);
            }
        }
    }
}