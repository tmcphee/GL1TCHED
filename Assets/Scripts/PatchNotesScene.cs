using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using playersave = PlayerSave.playersave;
using patchnotes = PatchNotesLog.notes;

public class PatchNotesScene : MonoBehaviour
{
    private playersave psave;
    private patchnotes pnotes;

    // Start is called before the first frame update
    void Start()
    {
        psave = new playersave();
        pnotes = new patchnotes();

        GameObject.Find("PatchText").GetComponent<Text>().text = pnotes.getNote(psave.getWorld(), psave.getLevel());
        GameObject.Find("Version").GetComponent<Text>().text = psave.getWorld() + "." + psave.getLevel();
    }

    /*  Tyler McPhee
     *  Loads the next level if the level exists
     *  If not go back to the Main Menu
     */
    private void loadnextlevel()
    {
        string scene = "World " + psave.getWorld() + " - Level " + psave.getLevel();

        SceneManager.LoadScene(scene);
        if (!SceneManager.GetSceneByName(scene).IsValid())
        {
            SceneManager.LoadScene("MainMenu");
        }

    }

    /*  Tyler McPhee
     *  On continue button click load the next level
     */
    public void ContinueBtn_Clicked()
    {
        loadnextlevel();
    }

    /*  Tyler McPhee
     *  On quit button click load the Main Menu
     */
    public void QuitBtn_Clicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
