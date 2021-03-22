﻿using System.Collections;
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

        /*  Tyler McPhee
         *  If the level is the first in the world then show the world panel
         *  else show the level panel
         */
        if (psave.getLevel() == 0)
        {
            GameObject.Find("PatchText").GetComponent<Text>().text = pnotes.getNote(psave.getWorld(), psave.getLevel());
            GameObject.Find("Version").GetComponent<Text>().text = psave.getWorld() + "." + psave.getLevel();
           
            GameObject.Find("Canvas/LevelPanel").SetActive(false);
        }
        else
        {
            GameObject.Find("Canvas/WorldPanel").SetActive(false);
            GameObject.Find("LevelText").GetComponent<Text>().text = "Level " + (psave.getLevel() + 1);
            string besttime = psave.parseTime(psave.getPreviousBestTime(), 3);
            if(besttime != "NA")
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

        //Scene Name, Colour to fade in, length of fade time
        Initiate.Fade(scene, Color.black, 1.5f);
        if (!SceneManager.GetSceneByName(scene).IsValid())
        {
            Initiate.Fade("MainMenu", Color.black, 1.5f);
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
