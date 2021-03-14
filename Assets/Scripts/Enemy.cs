using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float topSpeed;
    public float detectionDistance;

    private Rigidbody2D player;
    private Rigidbody2D enemy;
    private Vector3 originalPos;

    
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        enemy = GetComponent<Rigidbody2D>();
        originalPos = enemy.transform.position;
    }

    /* Andrew Greer 
        - for every frame, if the player is within the enemy's detection radius, it applies a force
          on the enemy towards the player if the top speed isn't exceeded
        - also includes recycled movement code for self-righting in case the enemy falls over
    */
    void Update()
    {
        float distance = Vector3.Distance(enemy.transform.position, player.transform.position);

        //self-righting force; returns enemy to vertical by 0.75 degrees/frame
        if (enemy.rotation != 0)
        {
            enemy.MoveRotation(Mathf.Abs(enemy.rotation - 0.75f));
        }

        //if player is within sight of the enemy and the enemy isn't going too fast
        if (distance <= detectionDistance && Mathf.Abs(enemy.velocity.x) <= topSpeed)
        {
            float sign;

            //multiplies the force by -1 if the player is to the left of the enemy
            if (player.transform.position[0] < enemy.transform.position[0])
            {
                sign = -1f;
            }
            else sign = 1f;

            //generates the enemy movement force
            Vector2 forceVector = new Vector2((sign * 900f) + (0.5f / distance) * sign * 2200f, 0f);
            enemy.AddForce(forceVector);
        }
    }


    /* Andrew Greer
       - convenient method for resetting the enemy position (i.e. level restart or respawning) */
    public void ResetPosition()
    {
        enemy.transform.position = originalPos;
        enemy.velocity = new Vector2(0f, 0f);
    }
}
