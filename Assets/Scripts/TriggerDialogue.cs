using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    private DialogueHolder _dialogueHolder;
    void LateUpdate()
    {
        if (DialogueManager.dialogueIsPlaying)
            return;
        
        int layerMask = LayerMask.GetMask("Interactable");
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        // Specifying layer that the ray cast hits
        if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log("hit the thing: " + hit.collider.name);
            _dialogueHolder = hit.collider.GetComponentInParent<DialogueHolder>();
            // If a dialogue holder is found on the racyasted object, then activate the popup
            if (_dialogueHolder)
            {
                _dialogueHolder.ShowDialoguePopup();
            }

            if (Input.GetMouseButtonDown(0))
            {
                DialogueManager.Instance.EnterDialogueMode(_dialogueHolder.inkJSON);
            }
        }
    }
}
