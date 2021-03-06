﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class InGameMenu : MonoBehaviour
{
    private Vector3 playervelocity = new Vector3(0, 0, 0);
    private float gravityscale;
    private float StoredLevelTime;

    private GameObject InGameMenuCanvas;
    private GameObject r, box;
    private Button LastCheckpointBtn, BackBtn, MainMenuBtn;


    /*  Tyler McPhee
     *      Called before the first frame update
     *      Finds the Player and In game menu GameObjects
     *      Finds and connects buttons to functions
     *      Hides the in game menu on start
     */
    void Start()
    {
        r = GameObject.Find("Player");
        box = GameObject.Find("grab box");

        InGameMenuCanvas = GameObject.Find("InGameMenuCanvas");

        LastCheckpointBtn = GameObject.Find("InGameMenuCanvas/RestartLastCheckpointBtn").GetComponent<Button>();
        LastCheckpointBtn.onClick.AddListener(RestartFromLastCheckpoint_Click);

        BackBtn = GameObject.Find("InGameMenuCanvas/BackBtn").GetComponent<Button>();
        BackBtn.onClick.AddListener(BackBtn_Click);

        MainMenuBtn = GameObject.Find("InGameMenuCanvas/MainMenuBtn").GetComponent<Button>();
        MainMenuBtn.onClick.AddListener(MainMenuBtn_Click);

        InGameMenuCanvas.SetActive(false);
    }


    /*  Tyler McPhee
     *      Disable Movment of player by disableing the object
     *      Saves player velocity, gravity, and LevelTime
     */
    public void FreezePlayer()
    {
        playervelocity = r.GetComponent<Rigidbody2D>().velocity;
        gravityscale = r.GetComponent<Rigidbody2D>().gravityScale;
        StoredLevelTime = r.GetComponent<Checkpoint>().getLevelTime();
        r.SetActive(false);
    }


    /*  Tyler McPhee
     *      Enables Movment of player by enableing the object
     *      Restores player velocity, gravity, and LevelTime
     */
    private void UnFreezePlayer()
    {
        r.SetActive(true);
        r.GetComponent<Rigidbody2D>().velocity = playervelocity;
        r.GetComponent<Rigidbody2D>().gravityScale = gravityscale;
        r.GetComponent<Checkpoint>().setLevelTime(StoredLevelTime);
    }


    /*  Tyler McPhee
     *      Sets the player to its last checkpoint
     */
    public void RestartFromLastCheckpoint_Click()
    {
        UnFreezePlayer();
        GetComponent<Checkpoint>().SetPlayerLastCheckpoint();

        if(box != null)
        {
            box.GetComponent<GrabObject>().ResetPosition();
        }

        InGameMenuCanvas.SetActive(false);
    }


    /*  Tyler McPhee
     *      On Menu Enter freese player and show menu
     */
    void OnEsc_Click()
    {
        FreezePlayer();
        InGameMenuCanvas.SetActive(true);
    }


    /*  Tyler McPhee
     *      Closes the Menu
     */
    public void BackBtn_Click()
    {
        UnFreezePlayer();
        InGameMenuCanvas.SetActive(false);
    }


    public void MainMenuBtn_Click()
    {
        //SceneManager.LoadScene("MainMenu");
        Initiate.Fade("MainMenu", Color.black, 1.5f);
    }


    /*  Tyler McPhee
     *      Update is called once per frame
     *      Checks if the esc key is pressed
     */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            OnEsc_Click();
        }
    }
}
