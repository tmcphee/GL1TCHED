using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using playersave = PlayerSave.playersave;

public class MainMenu : MonoBehaviour
{
    private playersave psave;
    public GameObject MenuCanvas;
    public GameObject Continue_BTN;
    public GameObject NewGame_BTN;
    public GameObject Settings_BTN;
    public GameObject Quit_BTN;


    public void ContinueBtn_Clicked()
    {

    }

    public void NewGameBtn_Clicked()
    {

    }

    public void SettingsBtn_Clicked()
    {

    }

    public void QuitBtn_Clicked()
    {
        Debug.Log("here");
        Application.Quit();
    }

    void Start()
    {
        psave = new playersave();
        if (!psave.SaveExists())
        {

            Continue_BTN.SetActive(false);
        }

    }

}

