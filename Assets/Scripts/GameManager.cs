using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    public static int ChapterNumber = 0; // 0 is cold open, rest is divided between acts
    public static string NextSceneName;
    private static GameManager _instance;
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
            SetNextSceneName("Party Room");
    }

    public void SetNextSceneName(string nextSceneName)
    {
        NextSceneName = nextSceneName;
    }
}