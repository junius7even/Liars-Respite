using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnFadeEndTransition : MonoBehaviour
{
    public Animator animator;
    // Update is called once per frame

    private void SceneChange()
    {
        SceneManager.LoadScene(GameManager.NextSceneName);

    }
}
