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
    [SerializeField] private GameObject exitButton;

    [Header("Choices")]
    [SerializeField] private bool choiceAvailable;
    [SerializeField] private int currentChoiceIndex;
    [SerializeField] private GameObject[] choiceButtons;   

    [Header("Public Values for References")]
    public bool dialogueIsPlaying;

    // private variables, no need to show in the inspector

    private TextMeshProUGUI[] choicesText;
    private Story currentStory;
    private bool canStartDialogue = true;
    private GameObject questUI;

    public static DialogueManager instance;

    private const string speaker = "speaker";

    private void Awake()
    {
        // creating the instance for Dialogue Manager

        if(instance != null)
        {
            Debug.LogWarning("There is more than one Dialogue Manager in the scene!");
            Destroy(gameObject);
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

    private void Update()
    {
        if (dialogueIsPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ContinueDialogue();
            }

            else if (Input.GetKeyDown(KeyCode.X) && !currentStory.canContinue)
            {
                EndDialogue();
            }

            else if (Input.GetKeyDown(KeyCode.Return) && choiceAvailable)
            {
                MakeChoice(currentChoiceIndex);
            }

            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // if there is a choice available upper on the list, mark it as selected
                if(currentChoiceIndex > 0)
                {
                    ChangeCurrentChoice(currentChoiceIndex - 1);
                }
            }

            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // if there is a choice available down on the list, mark it as selected
                if (currentChoiceIndex < currentStory.currentChoices.Count - 1)
                {
                    ChangeCurrentChoice(currentChoiceIndex + 1);
                }
            }
        }
    }

    public void StartDialogue(TextAsset inkJSON)
    {
        if (canStartDialogue)
        {
            currentStory = new Story(inkJSON.text);
            dialogueIsPlaying = true;
            dialogueCanvas.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            ContinueDialogue();
        }
    }

    public void ContinueDialogue()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();

            HandleTags(currentStory.currentTags);

            DisplayChoices();

            if (!currentStory.canContinue)
            {
                exitButton.SetActive(true);
            }
        }
    }

    private void DisplayChoices()
    {
        // change bool for input tracking
        if(currentStory.currentChoices.Count <= 0)
        {
            choiceAvailable = false;
        }

        else
        {
            choiceAvailable = true;
        }

        List<Choice> currentChoices = currentStory.currentChoices;

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

        // set the first choice option as selected
        ChangeCurrentChoice(0);
    }

    private void ChangeCurrentChoice(int index)
    {
        // make last current choice button normal
        choiceButtons[currentChoiceIndex].GetComponent<Image>().color = Color.white;

        // make new current choice button with contrast color
        currentChoiceIndex = index;
        choiceButtons[index].GetComponent<Image>().color = new Color(255, 218, 142, 255);
    }

    public void MakeChoice(int choiceIndex)
    {
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
                case speaker:
                    if(tagValue == "Fox")
                    {
                        if(PlayerPrefs.GetString("foxName") != "" || PlayerPrefs.GetString("foxName") != null)
                        {
                            speakerText.text = PlayerPrefs.GetString("foxName");
                        }

                        else
                        {
                            speakerText.text = tagValue;
                        }
                    }

                    else
                    {
                        speakerText.text = tagValue;
                    }
                    break;

                case "showUI":
                    GameEventsManager.instance.questEvents.ShowQuestUI(int.Parse(tagValue));
                    break;

                case "hideUI":
                    GameEventsManager.instance.questEvents.HideQuestUI();
                    break;

                default:
                    Debug.LogWarning("No tag key set for " + tag);
                    break;
            }
        }
    }

    public void CloseDialogueView()
    {
        if (!currentStory.canContinue)
        {
            exitButton.SetActive(false);
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogueIsPlaying = false;
        dialogueText.text = "";

        dialogueCanvas.SetActive(false);

        StartCoroutine(DelayBetweenDialogues());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // delay between dialogues to prevent a bug from moving from one dialogue to another with the same character without player pressing any keys
    private IEnumerator DelayBetweenDialogues()
    {
        canStartDialogue = false;

        yield return new WaitForSeconds(3f);

        canStartDialogue = true;
    }
}
