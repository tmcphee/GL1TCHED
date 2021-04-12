using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuClose : MonoBehaviour
{
    private GameObject r;

    /*  Tyler McPhee
     *      Find and store the Player Object
     */
    void Start()
    {
        r = GameObject.Find("Player");
    }

    /*  Tyler McPhee
     *      IF the ESC key is pressed close the menu and unfreeze the player using the BackBtn Click
     */
    void Update()
    {
        if (gameObject.active)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameObject.SetActive(false);
                r.GetComponent<InGameMenu>().BackBtn_Click();
            }
        }
    }
}
