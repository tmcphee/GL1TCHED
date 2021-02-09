using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using playersave = PlayerSave.playersave;

public class MainMenu : MonoBehaviour
{
    private playersave psave;
    public GameObject Canvas;
    private GameObject MainPanel;
    private GameObject SettingsPanel;
    private GameObject VideoPanel;

    /* MAIN MENU
     * 
     */
    public void ContinueBtn_Clicked()
    {

    }

    public void NewGameBtn_Clicked()
    {
        psave.resetdata();
    }

    public void SettingsBtn_Clicked()
    {
        MainPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void QuitBtn_Clicked()
    {
        Application.Quit();
    }

    /* Settings MENU
     * 
     */
    public void SettingsBackBtn_Clicked()
    {
        MainPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }

    public void VideoBtn_Clicked()
    {
        SettingsPanel.SetActive(false);
        VideoPanel.SetActive(true);
    }

    public void SoundBtn_Clicked()
    {

    }


    /* Video MENU
     * 
     */
    public void VideoBackBtn_Clicked()
    {
        VideoPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    void Start()
    {
        MainPanel = GameObject.Find("MainPanel");
        SettingsPanel = GameObject.Find("SettingsPanel");
        VideoPanel = GameObject.Find("VideoPanel");

        psave = new playersave();
        if (!psave.SaveExists())
        {

            GameObject.Find("MainPanel/ContinueGameBtn").SetActive(false);
        }

        SettingsPanel.SetActive(false);
        VideoPanel.SetActive(false);

    }

}

