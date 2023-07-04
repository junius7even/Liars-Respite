using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    public static int ChapterNumber = 0; // 0 is cold open, rest is divided between acts

    private const string storyPrefix = "StorySequence/";
    public static int CurrentStorySequence = 0;
    public static string NextSceneName;
    private static GameManager _instance;
    public GameObject objectToActivate;

    public static bool sceneChange;
    // Start is called before the first frame update
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
                Debug.Log("GameManager is null");
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance)
            Destroy(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Update()
    {
        HandleSceneTransitions();
    }

    private void HandleSceneTransitions()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            SetNextSceneName("ColdOpen");
        else if (SceneManager.GetActiveScene().name == "ColdOpen")
            SetNextSceneName("PartyRoom");
        else if (SceneManager.GetActiveScene().name == "PartyRoom")
            SetNextSceneName("LockedRoom");
        else if (SceneManager.GetActiveScene().name == "LockedRoom")
            SetNextSceneName("FutureUpdate");
    }

    public void SetActiveObject()
    {
       objectToActivate.SetActive(true);
    }

    public void SetNextSceneName(string nextSceneName)
    {
        NextSceneName = nextSceneName;
    }

    public void LoadNextScene()
    {        
        AudioClip clip = Resources.Load<AudioClip>($"Sound/Music/{NextSceneName}/Ambience");
        SoundManager.Instance.PlayAmbience("Ambience", NextSceneName);
        SoundManager.Instance.PlayMusic(clip);
        SceneManager.LoadScene(NextSceneName);
        Debug.Log("NextSceneName: " + NextSceneName);
        CurrentStorySequence = 0;
        TextAsset storySequence = Resources.Load<TextAsset>(storyPrefix + SceneManager.GetActiveScene().name + "/" + CurrentStorySequence); //"StorySequence/PartyRoom/0"
        DialogueManager.Instance.EnterDialogueMode(storySequence);
        SoundManager.Instance.PlayAmbience("Ambience", NextSceneName);
    }

    public void TriggerStorySequence()
    {
        TextAsset storySequence = Resources.Load<TextAsset>(storyPrefix + SceneManager.GetActiveScene().name + "/" + CurrentStorySequence); //"StorySequence/PartyRoom/0"
        CurrentStorySequence++;
        Debug.Log(SceneManager.GetActiveScene().name);
        DialogueManager.GetInstance().EnterDialogueMode(storySequence);
    }
}
