using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/* Troy Walher
 *  change the value of an audio mixers group
*/  
public class volumeSlide : MonoBehaviour
{
    // variables to store which mixer, mixerGroup and Slider
    public AudioMixer mixer;
    public AudioMixerGroup mixGroup;

    public Slider slider;

    //When the menu is loaded, set audio mixer value to value saved in preferences 
    void Start()
    {
        Debug.Log(mixGroup.ToString() + ": " + Mathf.Log10(PlayerPrefs.GetFloat(mixGroup.ToString(), 1f)) * 20);
        mixer.SetFloat(mixGroup.ToString(), Mathf.Log10(PlayerPrefs.GetFloat(mixGroup.ToString(), 1f)) * 20 );
    }

    //Called from a slider to lower the volume of a mixer group
    public void SetLevel (float slideValue)
    {
        mixer.SetFloat(mixGroup.ToString(), Mathf.Log10(slideValue) * 20);
        PlayerPrefs.SetFloat(mixGroup.ToString(), slideValue);
    }

    // When the slider object becomes visible, set it's value to the saved value from player preferences
    void OnEnable()
    {
        slider.value = PlayerPrefs.GetFloat(mixGroup.ToString(), 1f);
    }

}
