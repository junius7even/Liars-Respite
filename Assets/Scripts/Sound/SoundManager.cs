using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _musicSource, _effectsSource, _vaSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.clip = clip;
        _musicSource.Play();
    }
    
    public void PlayEffect(AudioClip clip)
    {
        
        _effectsSource.PlayOneShot(clip);
    }

    public void PlayVoiceOver(string character, string clipName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Sound/VA_s/{character}/{clipName}");
        Debug.Log($"Sound/VA_s/{character}/{clipName}");
        Debug.Log(clip);
        _vaSource.clip = clip;
        _vaSource.Play();
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }
}
