using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Canvas")]
    [SerializeField] private GameObject dialogueCanvas;

    [Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private GameObject continueButton;

    [Header("Quest Choices")]
    [SerializeField] private GameObject[] choiceButtons;

    [Header("Public Values for References")]
    public bool dialogueIsPlaying;

    //[Header("Debugging")]


    // private variables, no need to show in the inspector

    private TextMeshProUGUI[] choicesText;
    private Story currentStory;

    private static DialogueManager instance;

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        // creating the instance for Dialogue Manager

        if(instance != null)
        {
            Debug.LogWarning("There is more than one Dialogue Manager in the scene!");
        }

        instance = this;
    }

    private void Start()
    {
        dialogueCanvas.SetActive(false);
        dialogueIsPlaying = false;

        // initializing choice button texts

        choicesText = new TextMeshProUGUI[choiceButtons.Length];

        for(int i = 0; i < choiceButtons.Length; i++)
        {
            choicesText[i] = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            choiceButtons[i].SetActive(false);
        }
    }

    public void StartDialogue(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialogueCanvas.SetActive(true);

        ContinueDialogue();
    }

    public void ContinueDialogue()
    {
        if (currentStory.canContinue)
        {
            Debug.Log("Dialogue about to continue.");

            dialogueText.text = currentStory.Continue();

            HandleTags(currentStory.currentTags);

            DisplayChoices();
        }

        else
        {
            EndDialogue();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        // hide continue button if there are choices available, otherwise show the button

        if(currentChoices.Count > 0)
        {
            continueButton.SetActive(false);
        }

        else
        {
            continueButton.SetActive(true);
        }

        // check if the UI can hold all the written choice options

        if(currentChoices.Count > choiceButtons.Length)
        {
            Debug.LogWarning("There are more choices written than the UI can hold!");
        }

        // set each choice to show the correct text; if not all the slots are used, hide the extra ones

        int index = 0;

        foreach(Choice choice in currentChoices)
        {
            choiceButtons[index].SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for(int i = index; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        Debug.Log("Made a choice.");
        currentStory.ChooseChoiceIndex(choiceIndex);

        ContinueDialogue();
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach(string tag in currentTags)
        {
            string[] splitTag = tag.Split(":");

            if(splitTag.Length != 2)
            {
                Debug.LogWarning("Tag could not be appropriately parsed: " + tag);
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case "speakerName":
                    speakerText.text = tagValue;
                    break;

                default:
                    Debug.LogWarning("No tag key set for " + tag);
                    break;
            }
        }
    }

    private void EndDialogue()
    {
        Debug.Log("Dialogue ending.");
        dialogueIsPlaying = false;
        dialogueText.text = "";

        dialogueCanvas.SetActive(false);
    }
}
