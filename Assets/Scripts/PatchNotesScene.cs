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


    void Start()
    {
        psave = new playersave();
        pnotes = new patchnotes();


        if (psave.getWorld() == 1 && psave.getLevel() == 2)
        {
            Debug.Log("HERE");
            GotoCredits();
            return;
        }

        /*  Tyler McPhee
         *      If the level is the first in the world then show the world panel
         *      else show the level panel
         */
        if (psave.getLevel() == 0)
        {
            //Set text on Text labels
            GameObject.Find("PatchText").GetComponent<Text>().text = pnotes.getNote(psave.getWorld(), psave.getLevel());
            GameObject.Find("Version").GetComponent<Text>().text = psave.getWorld() + "." + psave.getLevel();
           
            //Display level panel
            GameObject.Find("Canvas/LevelPanel").SetActive(false);
        }
        else
        {
            //Display world panel
            GameObject.Find("Canvas/WorldPanel").SetActive(false);

            //Set text on Text labels
            GameObject.Find("LevelText").GetComponent<Text>().text = "Level " + (psave.getLevel() + 1);

            //Find best time then display the text on the text label
            string besttime = psave.ParseTime(psave.getPreviousBestTime(), 3);
            if(besttime != "NA")
            {
                GameObject.Find("BestTime").GetComponent<Text>().text = "Best Time:  " + besttime;
            }
        }
    }


    /*  Tyler McPhee
     *      Loads the next level if the level exists
     *      If not go back to the Main Menu
     */
    private void loadnextlevel()
    {
        string scene = "World " + psave.getWorld() + " - Level " + psave.getLevel();

        //Scene Name, Colour to fade in, length of fade time
        Initiate.Fade(scene, Color.black, 1.5f);
        if (!SceneManager.GetSceneByName(scene).IsValid())
        {
            Initiate.Fade("MainMenu", Color.black, 1.5f);
        }
    }


    /*  Tyler McPhee
     *      On continue button click Load the next level
     */
    public void ContinueBtn_Clicked()
    {
        loadnextlevel();
    }


    /*  Tyler McPhee
     *      Load the Credits
     */
    public void GotoCredits()
    {
        SceneManager.LoadScene("Credits");
        //Initiate.Fade("Credits", Color.black, 1.5f);
    }

    /*  Tyler McPhee
     *      On quit button click Load the Main Menu
     */
    public void QuitBtn_Clicked()
    {
        //SceneManager.LoadScene("MainMenu");
        Initiate.Fade("MainMenu", Color.black, 1.5f);
    }
}
