using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySoundOnStart : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayAmbience("Ambience", SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
