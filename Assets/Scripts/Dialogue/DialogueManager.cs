using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
   [Header("Dialogue UI")]
   [SerializeField] GameObject dialoguePanel;
   [SerializeField] private GameObject namePanel;
   [SerializeField] private GameObject portraitsGameObject;
   [SerializeField] public Animator sceneChangeAnimator;
   [SerializeField] private Animator musicAnimator;

   [SerializeField] private TextMeshProUGUI dialogueText;
   [SerializeField] private GameObject[] choices;
   [SerializeField] private Animator portraitsAnimator;
   [SerializeField] private Animator dialogueBoxAnimator;
   [SerializeField] public TextMeshProUGUI[] choicesText;
   [SerializeField] private GameObject inputField;

   [SerializeField]
   private Image portraitImage;
   [SerializeField]
   private Image portraitBackingImage;

   [SerializeField] GameObject noteObject;


   [SerializeField] private TextMeshProUGUI nameText;
   [SerializeField] private TextMeshProUGUI underNameText;

   private string portraitsPrefix = "Portraits/";
   private Story currentStory;
   private static DialogueManager instance;
   private string currentSpeaker = "";
   private string nextSpeaker;
   private string nextStoryLine;
   private bool displayingChoices;
   private bool editingTextField;

   [SerializeField] private GameObject dresser;
   
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
   public bool shouldSceneChange = false;
   
   public bool GetDialogueState()
   {
      return dialogueIsPlaying;
   }
   private void Start()
   {
      dialoguePanel.SetActive(true);
      foreach (GameObject choice in choices)
         choice.SetActive(false);
      portraitBackingImage.sprite = null;
      namePanel.SetActive(true);
      nameText.text = "";
      underNameText.text = "";
   }

   private void LateUpdate()
   {
      Debug.Log("Dialogue playing" + dialogueIsPlaying);
      bool animationFinished = (((portraitsAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)|| !portraitsGameObject.activeInHierarchy)&&
                                (dialogueBoxAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1));
      // Return if dialogue isn't playing
      if (!dialogueIsPlaying)
      {
         return;
      }
      
      string currentClipName = dialogueBoxAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

      Debug.Log("AnimationFinished: " + animationFinished);
      Debug.Log(portraitsAnimator.GetBool("FadeOut") && dialogueBoxAnimator.GetBool("FadeOut"));
      // If the dialoguepanel had finished it's fadein/out animation
      if (animationFinished)
      {
         // If the panel finished fading out, display the the next line by fading in
         if (dialogueBoxAnimator.GetBool("FadeOut") && currentClipName == "TextBoxDefault")
         {
            Debug.Log("I'm faded out my friend");
            SetDialogueAnimatorFadeIn(true);
            displayDialoguePanelWithFadeIn();
         }
         else if(dialogueBoxAnimator.GetBool("FadeIn") && currentClipName == "TextBoxExisting") // If the panel finished fading in, it will continue to exist
            SetDialogueAnimatorFadeIn(false);
         if (Input.GetKeyDown(KeyCode.Mouse0) && currentClipName == "TextBoxExisting"  && currentClipName != "TextBoxDefault" && !displayingChoices && !editingTextField)
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

   private void DisplayChoices()
   {
      List<Choice> currentChoices = currentStory.currentChoices;
      if (currentChoices.Count > choices.Length)
      {
         // Defensive check to make sure our UI can support the number of choices coming in.
         Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
      }
      Debug.Log(currentChoices.Count);
      int index = 0;
      // enable and initialize the choices up to the amount of current choices for this line of dialogue
      foreach (Choice choice in currentChoices)
      {
         choices[index].gameObject.SetActive(true);
         choicesText[index].text = choice.text;
         index++;
      }
      if (index != 0)
         displayingChoices = true;

      for (int i = index; i < choices.Length; i++)
      {
         choices[i].gameObject.SetActive(false);
      }

      StartCoroutine(SelectFirstChoice());
   }
   
   private IEnumerator SelectFirstChoice()
   {
      // Event system requires we clear it first, then wait
      // For at least one frame before we set the current selected object
         EventSystem.current.SetSelectedGameObject(null);
         yield return new WaitForEndOfFrame();
   }

   private void SetDialogueAnimatorFadeOut(bool value)
   {
      portraitsAnimator.SetBool("FadeOut", value);
      dialogueBoxAnimator.SetBool("FadeOut", value);
   }
   private void SetDialogueAnimatorFadeIn(bool value)
   {
      portraitsAnimator.SetBool("FadeIn", value);
      dialogueBoxAnimator.SetBool("FadeIn", value);
   }
   
   public void EnterDialogueMode(TextAsset inkJSON)
   {
      Debug.Log("Story: " + inkJSON.name);
      currentStory = new Story(inkJSON.text);
      dialogueIsPlaying = true;
      ContinueStory();
      SetDialogueAnimatorFadeIn(true);
      SetDialogueAnimatorFadeOut(false);
      Debug.Log("I've triggered dialogue");
   }

   public void ExitDialogueMode()
   {
      inputField.SetActive(false);
      currentSpeaker = "";
      dialogueIsPlaying = false;
      editingTextField = false;
      SetDialogueAnimatorFadeOut(true);
      SetDialogueAnimatorFadeIn(false);
      if (shouldSceneChange)
      {
         sceneChangeAnimator.SetBool("FadeOut", true);
         musicAnimator.SetBool("StartMusicFade", true);
      }
   }

   private void ContinueStory()
   {
      if (currentStory.canContinue)
      {
         if (currentStory.currentTags.Count > 0)
            currentSpeaker = currentStory.currentTags[0];
         nextStoryLine = currentStory.Continue();
         if (currentSpeaker == "")
            currentSpeaker = currentStory.currentTags[0];
         // Debug.Log(currentSpeaker);
         // Continue the dialogue only after the animation has played
         nextSpeaker = currentStory.currentTags[0];
         
         // Play fadeout when the speaker is swapped
         if (currentSpeaker != nextSpeaker)
         {
            SetDialogueAnimatorFadeIn(false);
            SetDialogueAnimatorFadeOut(true);
         }
         else // Display dialogue as normal if the same person continues to speak
            displayDialoguePanel();
      }
      else
      {
         ExitDialogueMode();
      }
   }

   private void displayDialoguePanelWithFadeIn()
   {
      SetDialogueAnimatorFadeOut(false);
      displayDialoguePanel();
      SetDialogueAnimatorFadeIn(true);
   }

   private void displayDialoguePanel()
   {
      //Debug.Log("Can continue!");
      dialogueText.text = nextStoryLine;
      // Debug.Log(currentStory.Continue());
      if (nextSpeaker == "None" || nextSpeaker == "AdvanceStory" || nextSpeaker == "SceneChange" || nextSpeaker == "Code" || nextSpeaker == "Dresser2")
      {
         portraitImage.sprite = null;
         portraitBackingImage.sprite = null;
         nameText.text = "";
         underNameText.text = "";
         namePanel.SetActive(false);
         portraitsGameObject.SetActive(false);
         if (nextSpeaker == "AdvanceStory")
            GameManager.Instance.TriggerStorySequence();
         if (nextSpeaker == "SceneChange")
         {
            shouldSceneChange = true;
            //Debug.Log("I've been sceneChanged magic");
            //Debug.Log(shouldSceneChange);
         }
         if (nextSpeaker == "Code")
         {
            editingTextField = true;
            inputField.SetActive(true);
         }
         
      }
      else
      {
         if (nextSpeaker == "note")
         {
            Debug.Log("Note triggred");
            noteObject.SetActive(true);
         }
         namePanel.SetActive(true);
         nameText.text = currentStory.currentTags[0];
         underNameText.text = currentStory.currentTags[0];
         if (nameText.text == "Rudolph2" || nameText.text == "Vivian2" || nameText.text == "Connor2")
         {
            string placeholder = nameText.text.Substring(0, nameText.text.Length - 1);
            nameText.text = placeholder;
            underNameText.text = placeholder;
         }
         Debug.Log(currentStory.currentTags[0]);
         portraitImage.sprite = Resources.Load<Sprite>(portraitsPrefix + currentStory.currentTags[0]);
         portraitBackingImage.sprite = Resources.Load<Sprite>(portraitsPrefix + currentStory.currentTags[0]);
         portraitsGameObject.SetActive(true);
      }

      if (currentStory.currentTags.Count > 1)
      {
         if (currentStory.currentTags[1] == "Evidence")
         {
               
         }
         else if (currentStory.currentTags[1] == "Dresser2")
         {
            dresser.GetComponent<DialogueHolder>().inkJSON = Resources.Load<TextAsset>("StorySequence/LockedRoom/Dresser2");
         }
         else
         {
            SoundManager.Instance.PlayVoiceOver(currentStory.currentTags[0], currentStory.currentTags[1]);
         }
         if (currentStory.currentTags.Count > 2)
            shouldSceneChange = true;
      }
      DisplayChoices();
   }

   public void ReadStringInput(string s)
   {
      if (s == "8348")
      {
         dialogueText.text = "The lock opened.";
         ExitDialogueMode();
         EnterDialogueMode(Resources.Load<TextAsset>("StorySequence/LockedRoom/0"));
      }
      else
      {
         dialogueText.text = "That's incorrect!";
      }
   }

   public void MakeChoice(int choiceIndex)
   {
      currentStory.ChooseChoiceIndex(choiceIndex);
      displayingChoices = false;
      ContinueStory();
   }
}
