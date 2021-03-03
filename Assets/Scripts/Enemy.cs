using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float topSpeed;
    public float detectionDistance;

    private Rigidbody2D player;
    private Rigidbody2D enemy;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        enemy = GameObject.Find("Enemy").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(enemy.transform.position, player.transform.position);

        //self-righting force; returns enemy to vertical by 0.75 degrees/frame
        if (enemy.rotation != 0)
        {
            enemy.MoveRotation(Mathf.Abs(enemy.rotation - 0.75f));
        }

        if (distance <= detectionDistance && Mathf.Abs(enemy.velocity.x) <= topSpeed)
        {
            float angle = DegreesToRadians(Vector3.SignedAngle(enemy.transform.position, player.transform.position, player.transform.position - enemy.transform.position));

            float sign;
            if (player.transform.position[0] < enemy.transform.position[0])
            {
                sign = -1f;
            }
            else sign = 1f;

            Vector2 forceVector = new Vector2((sign * 900f) + (0.5f / distance) * sign * 2200f, 0f);
            Debug.Log((angle, forceVector));
            enemy.AddForce(forceVector);
        }
    }

    float DegreesToRadians(float angle) { return angle * (Mathf.PI / 180); }
}
