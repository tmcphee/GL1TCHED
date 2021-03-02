using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
        //enemy.transform.LookAt(player.transform);

        if(Vector3.Distance(enemy.transform.position, player.transform.position) >= 5)
        {
            //enemy.transform.position += transform.forward * 2;
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, player.position, 5);
            //enemy.AddForce(new Vector2(1f, 2f));
        }
    }
}
