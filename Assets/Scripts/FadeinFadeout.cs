using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeinFadeout : MonoBehaviour
{
    [SerializeField]
    public CanvasGroup canvasGroup;
    [SerializeField]
    public bool fadein = false;
    [SerializeField]
    public bool fadeout = false;
    [SerializeField]
    public float TimeToFade;
    [Header("Ink JSON")][SerializeField] private TextAsset inkJSON;
    void Update()
    {
        // if (fadein)
        // {
        //     if (canvasGroup.alpha < 1)
        //     {
        //         canvasGroup.alpha += TimeToFade * Time.deltaTime;
        //         if (canvasGroup.alpha >= 1)
        //         {
        //             fadein = false;
        //             if (inkJSON != null)
        //                 TriggerDialogue();
        //         }
        //     }
        // }
        // else if (fadeout)
        // {
        //     if (canvasGroup.alpha > 0)
        //     {
        //         canvasGroup.alpha += TimeToFade * Time.deltaTime;
        //         if (canvasGroup.alpha >= 1)
        //             fadeout = false;
        //     }
        // }
    }

    public void fadeIn()
    {
        fadein = true;
    }

    public void fadeOut()
    {
        fadeout = true;
    }
    
    private void TriggerDialogue()
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
    }
}
