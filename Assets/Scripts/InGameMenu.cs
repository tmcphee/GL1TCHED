using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    private Vector3 playervelocity = new Vector3(0, 0, 0);
    private float gravityscale;

    public GameObject InGameMenuCanvas;
    public GameObject r;

    //Disable Movment of player by disableing the object
    //Saves player velocity and gravity
    private void freezeplayer()
    {
        playervelocity = r.GetComponent<Rigidbody2D>().velocity;
        gravityscale = r.GetComponent<Rigidbody2D>().gravityScale;
        r.SetActive(false);
    }

    //Enables Movment of player by enableing the object
    //Restores player velocity and gravity
    private void unfreezeplayer()
    {
        r.SetActive(true);
        r.GetComponent<Rigidbody2D>().velocity = playervelocity;
        r.GetComponent<Rigidbody2D>().gravityScale = gravityscale;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        InGameMenuCanvas.SetActive(false);
    }

    //Sets the player to its last checkpoint
    public void RestartFromLastCheckpoint_Click()
    {
        unfreezeplayer();
        this.GetComponent<Checkpoint>().SetPlayerLastCheckpoint();
        InGameMenuCanvas.SetActive(false);
    }

    //On Menu Enter freese player and show menu
    //On Menu Enter freese player and show menu
    void OnEsc_Click()
    {
        freezeplayer();
        InGameMenuCanvas.SetActive(true);
    }

    //closes the Menu
    public void BackBtn_Click()
    {
        unfreezeplayer();
        InGameMenuCanvas.SetActive(false);
    }

    public void mainMenuBtn_Click()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            OnEsc_Click();
        }
    }
}
