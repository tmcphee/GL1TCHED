using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClass : MonoBehaviour
{
    private AudioSource music;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject.transform);
        music = GameObject.Find("Music Source").GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (music.isPlaying) return;
        music.Play();
    }

    public void StopMusic()
    {
        music.Stop();
    }
}
