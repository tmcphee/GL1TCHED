using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using playersave = PlayerSave.playersave;

public class PatchNotesScene : MonoBehaviour
{
    private playersave psave;

    // Start is called before the first frame update
    void Start()
    {
        psave = new playersave();
    }

    public void ContinueBtn_Clicked()
    {
        SceneManager.LoadScene("World " + psave.getWorld() + " - Level " + psave.getLevel());
    }

    public void QuitBtn_Clicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
