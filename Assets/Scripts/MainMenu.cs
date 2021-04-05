using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using playersave = PlayerSave.playersave;

public class MainMenu : MonoBehaviour
{
    private playersave psave;
    public GameObject Canvas;
    private GameObject MainPanel;
    private GameObject SettingsPanel;
    private GameObject VideoPanel;
    private GameObject SoundPanel;

    /*  Tyler McPhee
     *      Loads the values from the Player Save File and updates the value in the settings menu
     */
    private void initilize_settings()
    {
        Application.targetFrameRate = 144;

        //Gets the Resolution dropdown and sets its value to the resolution matiching the saved resolution
        Dropdown Resdrop = GameObject.Find("VideoPanel/ResolutionDropdown").GetComponent<Dropdown>();
        Resdrop.value = Resdrop.options.FindIndex(option => option.text == psave.getResolution());

        //Gets the VSYNC toggle and sets its value to the VSYNC bool matiching the saved VSYNC state
        Toggle VSYNCtog = GameObject.Find("VideoPanel/VSYNC_Toggle").GetComponent<Toggle>();
        VSYNCtog.isOn = psave.getVsync();

        //Gets the Fullscreen toggle and sets its value to the Fullscreen bool matiching the saved Fullscreen state
        Toggle Fullscreentog = GameObject.Find("VideoPanel/Fullscreen_Toggle").GetComponent<Toggle>();
        Fullscreentog.isOn = psave.getFullscreen();
    }


    private void loadscene()
    {
        //SceneManager.LoadScene("PatchNotes");
        Initiate.Fade("PatchNotes", Color.black, 1.5f);
    }


    /* MAIN MENU
     * 
     */
    public void ContinueBtn_Clicked()
    {
        loadscene();
    }


    public void NewGameBtn_Clicked()
    {
        psave.resetdata();
        loadscene();
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
        SettingsPanel.SetActive(false);
        SoundPanel.SetActive(true);
    }


    /*
     *Sound Menu
     */
    public void SoundBackBtn_Clicked()
    {
        SoundPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }


    /* Video MENU
     * 
     */
    public void VideoBackBtn_Clicked()
    {
        VideoPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }


    /*  Tyler McPhee
     *      Saves the specified resulution selected in the dropdown to disk when changed
     */
    public void Resolution_Dropdown_OnValueChanged(Dropdown change)
    {
        psave.setResolution("" + change.captionText.text);
    }


    public void VSYNC_Toggle_OnValueChanged()
    {
        Toggle vtog = GameObject.Find("VideoPanel/VSYNC_Toggle").GetComponent<Toggle>();
        psave.setVsync(vtog.isOn);
    }


    public void Fullscreen_Toggle_OnValueChanged()
    {
        Toggle vtog = GameObject.Find("VideoPanel/Fullscreen_Toggle").GetComponent<Toggle>();
        psave.setFullscreen(vtog.isOn);
    }


    void Start()
    {
        MainPanel = GameObject.Find("MainPanel");
        SettingsPanel = GameObject.Find("SettingsPanel");
        VideoPanel = GameObject.Find("VideoPanel");
        SoundPanel = GameObject.Find("SoundPanel");

        psave = new playersave();
        if (!psave.SaveExists())
        {

            GameObject.Find("MainPanel/ContinueGameBtn").SetActive(false);
        }

        initilize_settings();

        /* Andrew Greer - plays music when the game is loaded */
        GameObject.Find("VolumeController").GetComponent<AudioClass>().PlayMusic();

        SettingsPanel.SetActive(false);
        VideoPanel.SetActive(false);
        SoundPanel.SetActive(false);

    }
}

