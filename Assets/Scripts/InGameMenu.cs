using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{

    public GameObject InGameMenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        InGameMenuCanvas.SetActive(false);
    }

    public void RestartFromLastCheckpoint_Click()
    {
        this.GetComponent<Checkpoint>().SetPlayerLastCheckpoint();
        InGameMenuCanvas.SetActive(false);
    }

    void OnEsc_Click()
    {
        InGameMenuCanvas.SetActive(true);
    }

    public void BackBtn_Click()
    {
        InGameMenuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            OnEsc_Click();
        }
    }
}
