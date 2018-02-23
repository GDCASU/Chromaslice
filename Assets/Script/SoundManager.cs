using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class SoundManager : MonoBehaviour
{
    static SoundManager instance_;
    public SoundManager SM = null;
    const float MaxBackgroundMucisVolume = 1f;
    const float MaxSoundEffectsVolume = 1f;
    static float CurrentBGMVolume = 1f;
    static float CurrentSFXVolume = 1f;
    static bool isMuted = false;

    List<AudioSource> sfxSources;
    AudioSource backgroundMusic;

    void Awake()
    {
        //checking if there is already an instance of SoundManager
         if(SM == null)
         {
            //sets instance to this if there is none
             SM = this;
         }
         else if(SM != this)
         {
            //destroys this to ensure only one instance
             Destroy(gameObject);
         }

        DontDestroyOnLoad(gameObject);

    }

    public static SoundManager GetInstance()
    {
        if(!instance_)
        {
            GameObject soundManager = new GameObject("SoundManager");
            instance_ = soundManager.AddComponent();
            instance_.Initialize();
        }

        return instance_;
    }

    //initializing the varaibles
    void Initialize()
    {
        backgroundMusic = gameObject.AddComponent();
        backgroundMusic.loop = true;
        backgroundMusic.playOnAwake = false;
        backgroundMusic.volume = GetBGMVolume();
        DontDestroyOnLoad(gameObject);
    }

    //Helper methods for volume
    static float GetBGMVolume()
    {
        return isMuted ? 0f : MaxBackgroundMucisVolume * CurrentBGMVolume;
    }

    public static float GetSFXVolume()
    {
        return isMuted ? 0f : MaxSoundEffectsVolume * CurrentSFXVolume;
    }
    

    //fading background music
    void FadeBGMOut (float fadeDuration)
    {
        SoundManager SM = GetInstance();

        float delay = 0f;
        float nextVolume = 0f;

        if(SM.backgroundMusic.clip == null)
        {
            Debug.LogError("Could not fade Nackground Music, no audio clip");
        }

        StartCoroutine(FadeBGM(nextVolume, delay, fadeDuration));
    }

    //fade in
    void FadeBGMIn(AudioClip clip, float delay, float fadeDuration)
    {
        SoundManager SM = GetInstance();
        SM.backgroundMusic.clip = clip;
        SM.backgroundMusic.Play();

        float nextVolume = GetBGMVolume();

        StartCoroutine(FadeBGM(nextVolume, delay, fadeDuration));
    }

    //fades te sound for background music
    IEnumerator FadeBGM(float fadeToVolume, float delay, float fadeDuration)
    {
        yield return new WaitForSeconds(delay);

        SoundManager SM = GetInstance();

        float elapsed = 0f;

        while (fadeDuration > 0)
        {
            float t = (elapsed / fadeDuration);
            float volume = Mathf.Lerp(0f, fadeToVolume * CurrentBGMVolume, t);

            SM.backgroundMusic.volume = volume;

            elapsed += Time.deltaTime;
            yield return 0;
        }
    }

    //plays background music
    public static void PlayBGM(AudioClip clip, bool fade, float fadeDuration)
    {
        SoundManager SM = GetInstance();

        if(fade)
        {
            //fade out then fade in
            if(SM.backgroundMusic.isPlaying)
            {
                SM.FadeBGMOut(fadeDuration / 2);
                SM.FadeBGMIn(clip, fadeDuration / 2, fadeDuration / 2);
            }
            else
            {
                //fade in only because nothing is playing
                float delay = 0f;
                SM.FadeBGMIn(clip, delay, fadeDuration);
            }
        }
        else
        {
            SM.backgroundMusic.volume = GetBGMVolume();
            SM.backgroundMusic.clip = clip;
            SM.backgroundMusic.Play();
        }
    }

    //stops background music
    public static void StopBGM(bool fade, float fadeDuration)
    {
        SoundManager SM = GetInstance();

        //if music is playing, fades out, then switch and fade in
        if (SM.backgroundMusic.isPlaying)
        {
            if (fade)
            {
                SM.FadeBGMOut(fadeDuration);
            }
            else
            {
                SM.backgroundMusic.Stop();
            }
        }
    }

    //ges the sound effect
    AudioSource GetSFXSource()
    {
        //set up a new sfx source for each new clip
        AudioSource sfxSource = gameObject.AddComponent();

        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = GetSFXVolume();

        if(sfxSources == null)
        {
            //initialize list if there is none
            sfxSources = new List<AudioSource>();
        }

        sfxSources.Add(sfxSource);

        return sfxSource;
    }

    //waits for full duration of clips before removing it
    IEnumerator RemoveSFXSource(AudioSource sfxSource)
    {
        yield return new WaitForSeconds(sfxSource.clip.length);
        sfxSources.Remove(sfxSource);
        Destroy(sfxSource);
    }

    //same as RemoveSFXSource but for a fixed length
    IEnumerator RemoveSFXSourceFixedLength(AudioSource sfxSource, float length)
    {
        yield return new WaitForSeconds(length);
        sfxSources.Remove(sfxSource);
        Destroy(sfxSource);
    }

    //plays the sound effects
    public static void PlaySFX(AudioClip clip)
    {
        SoundManager SM = GetInstance();
        AudioSource source = SM.GetSFXSource();
        source.volume = GetSFXVolume();
        source.clip = clip;
        source.Play();

        SM.StartCoroutine(SM.RemoveSFXSource(source));
    }

    //same as PlaySFX but adjusts pitch slightly for small variation
    public static void PlaySFXRandomized(AudioClip clip)
    {
        SoundManager SM = GetInstance();
        AudioSource source = SM.GetSFXSource();
        source.volume = GetSFXVolume();
        source.clip = clip;
        source.pitch = Random.Range(0.85f, 1.2f);
        source.Play();

        SM.StartCoroutine(SM.RemoveSFXSource(source));
    }

    //same as PlaySFX but cuts off clip at certain point
    public static void PlaySFXFixedDuration(AudioClip clip, float duration, float volumeMultiplier = 1.0f)
    {
        SoundManager SM = GetInstance();
        AudioSource source = SM.GetSFXSource();
        source.volume = GetSFXVolume() * volumeMultiplier;
        source.clip = clip;
        source.loop = true;
        source.Play();

        SM.StartCoroutine(SM.RemoveSFXSourceFixedLength(source, duration));
    }

    //mutes sounds
    public static void DisableSoundImmediate()
    {
        SoundManager SM = GetInstance();
        SM.StopAllCoroutines();

        if(SM.sfxSources != null)
        {
            foreach(AudioSource source in SM.sfxSources)
            {
                source.volume = 0;
            }
        }
        SM.backgroundMusic.volume = 0f;
        isMuted = true;
    }

    //re-enable all audio sources
    public static void EnableSoundImmediate()
    {
        SoundManager SM = GetInstance();

        if(SM.sfxSources != null)
        {
            foreach(AudioSource source in SM.sfxSources)
            {
                source.volume = GetSFXVolume();
            }
        }
        SM.backgroundMusic.volume = GetBGMVolume();
        isMuted = false;
    }

    //sets overall volume (global volume)
    public static void SetGlobalVolume(float newVolume)
    {
        CurrentBGMVolume = newVolume;
        CurrentSFXVolume = newVolume;
        AdjustSoundImmediate();
    }

    //sets only sfx volume
    public static void SetSFXVolume(float newVolume)
    {
        CurrentSFXVolume = newVolume;
        AdjustSoundImmediate();
    }

    //sets only bgm volume
    public static void SetBGMVolume(float newVolume)
    {
        CurrentBGMVolume = newVolume;
        AdjustSoundImmediate();
    }

    //update sources to reflect new volume level
    public static void AdjustSoundImmediate()
    {
        SoundManager SM = GetInstance();

        //loops through a list of sources to set volume
        if(SM.sfxSources != null)
        {
            foreach(AudioSource source in SM.sfxSources)
            {
                source.volume = GetSFXVolume();
            }
        }
        SM.backgroundMusic.volume = GetBGMVolume();
    }
}
