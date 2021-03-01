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
        if (psave.getLevel() == 0)
        {
            GameObject.Find("PatchText").GetComponent<Text>().text = pnotes.getNote(psave.getWorld(), psave.getLevel());
            GameObject.Find("Version").GetComponent<Text>().text = psave.getWorld() + "." + psave.getLevel();
           
            GameObject.Find("Canvas/LevelPanel").SetActive(false);
        }
        else
        {
            GameObject.Find("Canvas/WorldPanel").SetActive(false);
            GameObject.Find("LevelText").GetComponent<Text>().text = "Level " + psave.getLevel();
            float besttime = psave.getPreviousBestTime();
            if(besttime != -1f)
            {
                GameObject.Find("BestTime").GetComponent<Text>().text = "Best Time:  " + besttime;
            }
            
        }
        

        
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
