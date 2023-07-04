using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySoundOnStart : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    [SerializeField] private AudioSource musicSource;
    // Start is called before the first frame update
    // called first
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        musicSource.clip = clip;
        musicSource.Play();

    }
    
    // called third
    void Start()
    {
        Debug.Log("Start");
        musicSource.clip = clip;
        musicSource.Play();
    }

    // called when the game is terminated
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
