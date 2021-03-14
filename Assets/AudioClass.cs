using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Andrew Greer
 *  - gets the music audio source object and provides methods for playing and stopping music
 *      - the DontDestroyOnLoad on this object allows the music to continue playing when a new scene is loaded
 */

public class AudioClass : MonoBehaviour
{
    private AudioSource music;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject.transform);
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (!music.isPlaying)
        {
            music.Play();
        }

        return;
    }

    public void StopMusic()
    {
        music.Stop();
    }
}
