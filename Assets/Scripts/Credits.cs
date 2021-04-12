using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    private Animator animator;
    private float RemainingTime = 20.0f;


    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }


    void Update()
    {
        RemainingTime -= Time.deltaTime;
        if (RemainingTime < 0)
        {
            Initiate.Fade("MainMenu", Color.black, 1.5f);
        }
    }
}
