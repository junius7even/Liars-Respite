using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;

public class DialogueHolder : MonoBehaviour
{
    public GameObject dialoguePopup;
    public TextAsset InkJSON;
    
    // Start is called before the first frame update
    private void Update()
    {
        DisableDialoguePopup();
    }
    public void ShowDialoguePopup()
    {
        dialoguePopup.SetActive(true);
    }

    public void DisableDialoguePopup()
    {
        dialoguePopup.SetActive(false);
    }
}
