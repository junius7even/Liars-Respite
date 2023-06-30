using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using Unity.VisualScripting;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
   [Header("Dialogue UI")]
   [SerializeField] GameObject dialoguePanel;
   [SerializeField] private GameObject namePanel;
   [SerializeField] private GameObject portraitGameObject;
   [SerializeField] private GameObject portraitDarkGameObject;
   [SerializeField] private Animator sceneChangeAnimator;
   [SerializeField] private Animator musicAnimator;

   [SerializeField] private TextMeshProUGUI dialogueText;
   [SerializeField] private GameObject[] choices;
   [SerializeField] TextMeshProUGUI[] choicesText;

   [SerializeField]
   private Image portraitImage;
   [SerializeField]
   private Image portraitBackingImage;


   [SerializeField] private TextMeshProUGUI nameText;
   [SerializeField] private TextMeshProUGUI underNameText;

   private string portraitsPrefix = "Portraits/";
   private Story currentStory;
   private static DialogueManager instance;

   public static DialogueManager Instance
   {
      get
      {
         if (!instance)
            instance = new GameObject().AddComponent<DialogueManager>();
         return instance;
      }
   }
   
   public static bool dialogueIsPlaying;
   private bool shouldSceneChange = false;
   
   public bool GetDialogueState()
   {
      return dialogueIsPlaying;
   }
   private void Start()
   {
      dialoguePanel.SetActive(false);
      dialogueIsPlaying = false;
      portraitImage.sprite = null;
      portraitGameObject.SetActive(false);
      portraitDarkGameObject.SetActive(false);

      portraitBackingImage.sprite = null;
      namePanel.SetActive(false);
      nameText.text = "";
      underNameText.text = "";
   }

   private void Update()
   {
      // Return if dialogue isn't playing
      if (!dialogueIsPlaying)
      {
         return;
      }
         
      // If players pressed submit button
      if (Input.GetMouseButtonDown(0))
      {
         ContinueStory();
      }
   }

   private void Awake()
   {
      if (instance != null)
      {
         Debug.LogWarning("Found more than one Dialogue Manager in the scene");
      }

      instance = this;
   }

   public static DialogueManager GetInstance()
   {
      return instance;
   }

   public void EnterDialogueMode(TextAsset inkJSON)
   {
      Debug.Log("I've triggered dialogue");
      currentStory = new Story(inkJSON.text);
      dialogueIsPlaying = true;
      dialoguePanel.SetActive(true);
      ContinueStory();
   }

   private void ExitDialogueMode()
   {
      dialoguePanel.SetActive(false);
      dialogueIsPlaying = false;
      portraitImage.sprite = null;
      portraitGameObject.SetActive(false);
      portraitDarkGameObject.SetActive(false);

      portraitBackingImage.sprite = null;
      namePanel.SetActive(false);
      nameText.text = "";
      underNameText.text = "";
   }

   private void ContinueStory()
   {
      if (currentStory.canContinue)
      {
         currentStory.Continue();
         Debug.Log("Can continue!");
         // Debug.Log(currentStory.Continue());
         if (currentStory.currentTags[0] == "None")
         {
            portraitImage.sprite = null;
            portraitBackingImage.sprite = null;
            nameText.text = "";
            underNameText.text = "";
            namePanel.SetActive(false);
            portraitGameObject.SetActive(false);
            portraitDarkGameObject.SetActive(false);
         }
         else
         {
            portraitGameObject.SetActive(true);
            portraitDarkGameObject.SetActive(true);
            namePanel.SetActive(true);
            nameText.text = currentStory.currentTags[0];
            underNameText.text = currentStory.currentTags[0];
            Debug.Log(currentStory.currentTags[0]);
            portraitImage.sprite = Resources.Load<Sprite>(portraitsPrefix + currentStory.currentTags[0]);
            portraitBackingImage.sprite = Resources.Load<Sprite>(portraitsPrefix + currentStory.currentTags[0]);
         }

         if (currentStory.currentTags.Count > 1)
         {
            if (currentStory.currentTags[1] == "Evidence")
            {
               
            }
            else
            {
               SoundManager.Instance.PlayVoiceOver(currentStory.currentTags[0], currentStory.currentTags[1]);
            }
            if (currentStory.currentTags.Count > 2)
               shouldSceneChange = true;
         }
         dialogueText.text = currentStory.currentText;
      }
      else
      {
         ExitDialogueMode();
         if (shouldSceneChange)
         {
            sceneChangeAnimator.SetBool("FadeOut", true);
            musicAnimator.SetBool("StartMusicFade", true);
         }
      }
   }
}
