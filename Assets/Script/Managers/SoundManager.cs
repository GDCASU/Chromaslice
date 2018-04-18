using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class SoundManager : MonoBehaviour
{
    public static SoundManager singleton;
    public Sound[] sounds;

    private List<Sound> currentLevelMusic;

    // Set manager singleton
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

    void Start()
    {
        currentLevelMusic = new List<Sound>();

        // Play theme song by default
        ResetLevelMusic("ChromasliceTheme");
    }

    // Clears all the old scene music and plays curent scene music. You can also choose to fade
    public void ResetLevelMusic(string name = "", float fadeDuration = 0.0f)
    {
        StartCoroutine(FadeAudio(name, fadeDuration));
    }

    // For ambient looping sounds
    public void AddSoundToLevel(string name)
    {
        Sound sound = System.Array.Find(sounds, s => s.name == name);
        if (sound != null)
        {
            sound.audioSource.Play();
            currentLevelMusic.Add(sound);
        }
    }

    // Plays an instanced sound without adding to list
    public void PlaySoundInstance(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);

        if (s != null && !s.audioSource.isPlaying)
            s.audioSource.Play();
    }

    // Play a sound without adding to list
    public void PlaySound(string name, float pitch = 1.0f)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.audioSource.pitch = pitch;
            s.audioSource.Play();
        }
    }

    // Fade all the sounds in the scene out
    IEnumerator FadeAudio(string name, float fadeDuration)
    {
        // Fade volume out
        if (fadeDuration > 0)
        {
            while (currentLevelMusic.Count > 0 && currentLevelMusic[0].audioSource.volume > 0)
            {
                foreach (Sound s in currentLevelMusic)
                    s.audioSource.volume -= s.volume * Time.deltaTime / fadeDuration;
                yield return new WaitForEndOfFrame();
            }
        }

        // Restore volume
        foreach (Sound s in currentLevelMusic)
        {
            s.audioSource.Stop();
            s.audioSource.volume = s.volume;
        }

        currentLevelMusic.Clear();

        // Play the new sound
        Sound sound = System.Array.Find(sounds, s => s.name == name);
        if (sound != null)
        {
            sound.audioSource.Play();
            currentLevelMusic.Add(sound);
        }
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
