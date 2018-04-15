using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class SoundManager : MonoBehaviour
{
    public static SoundManager singleton;

    public Sound[] sounds;

    private Sound currentLevelMusic;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;

        else if (singleton != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Create each sound
        foreach(Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.soundClip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
        }
    }

    // Plays level music
    public void PlayLevelMusic(string name)
    {
        if(currentLevelMusic != null)
        {
            currentLevelMusic.audioSource.Stop();
        }
        else
        {
            currentLevelMusic = System.Array.Find(sounds, sound => sound.name == name);
        }

        currentLevelMusic.audioSource.Play();
    }

}

[System.Serializable]
public class Sound
{
    public AudioClip soundClip;
    public string name;
    public bool loop;
    [Range(0, 1)] public float volume;
    [Range(0, 2)] public float pitch = 1.0f;
    [HideInInspector] public AudioSource audioSource;
}
