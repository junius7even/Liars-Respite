using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void scene_change()
    {
        SceneManager.LoadScene(GameManager.NextSceneName);
    }
}
