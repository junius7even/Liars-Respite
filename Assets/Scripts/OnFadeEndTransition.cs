using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnFadeEndTransition : MonoBehaviour
{
    private void SceneChange()
    {
        Debug.Log(GameManager.NextSceneName);
        SceneManager.LoadScene(GameManager.NextSceneName);
    }
}
