using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float bounciness;

    private Rigidbody2D player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

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
                Debug.Log(bounceVector);
            }
        }
    }
}
