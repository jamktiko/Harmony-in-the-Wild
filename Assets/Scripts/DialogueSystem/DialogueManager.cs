using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private const string speaker = "speaker";

    [Header("Dialogue Canvas")]
    [SerializeField] private GameObject dialogueCanvas;

    [Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private GameObject exitButton;

    [Header("Choices")]
    [SerializeField] private bool isChoiceAvailable;
    [SerializeField] private int currentChoiceIndex = 0;
    [SerializeField] private GameObject[] choiceButtons;

    [Header("Public Values for References")]
    public bool isDialoguePlaying;

    [Header("Ink Globals")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    // private variables, no need to show in the inspector
    private DialogueVariableObserver dialogueVariables;
    private Story currentStory;
    private TextMeshProUGUI[] choicesText;
    private bool canStartDialogue = true;

    private void Awake()
    {
        // creating the instance for Dialogue Manager

        if(instance != null)
        {
            Debug.LogWarning("There is more than one Dialogue Manager in the scene!");
            Destroy(gameObject);
        }

        instance = this;

        dialogueVariables = new DialogueVariableObserver(loadGlobalsJSON);
    }

    private void Start()
    {
        dialogueCanvas.SetActive(false);
        isDialoguePlaying = false;

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
        if (isDialoguePlaying)
        {
            // make the selected choice
            if (Input.GetKeyDown(KeyCode.Return) && isChoiceAvailable)
            {
                MakeChoice(currentChoiceIndex);
            }

            else if (Input.GetKeyDown(KeyCode.UpArrow) && isChoiceAvailable)
            {
                // if there is a choice available upper on the list, mark it as selected
                if (currentChoiceIndex > 0)
                {
                    ChangeCurrentChoice(currentChoiceIndex - 1);
                }
            }

            else if (Input.GetKeyDown(KeyCode.DownArrow) && isChoiceAvailable)
            {
                // if there is a choice available down on the list, mark it as selected
                if (currentChoiceIndex < currentStory.currentChoices.Count - 1)
                {
                    ChangeCurrentChoice(currentChoiceIndex + 1);
                }
            }

            // if there is still more dialogue to show, continue to the next section
            else if (Input.GetKeyDown(KeyCode.Space) && currentStory.canContinue && !isChoiceAvailable)
            {
                ContinueDialogue();
            }

            // if there is no more dialogue to show, end the dialogue
            else if (Input.GetKeyDown(KeyCode.Space) && !currentStory.canContinue && !isChoiceAvailable)
            {
                EndDialogue();
            }
        }
    }

    public void StartDialogue(TextAsset inkJSON)
    {
        if (canStartDialogue)
        {
            Debug.Log("Start dialogue.");

            GameEventsManager.instance.dialogueEvents.StartDialogue();

            currentStory = new Story(inkJSON.text);
            isDialoguePlaying = true;
            dialogueCanvas.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            ContinueDialogue();

            // start listening the variable changes in the current story
            dialogueVariables.StartListening(currentStory);
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

            else
            {
                exitButton.SetActive(false);
            }
        }
    }

    private void DisplayChoices()
    {
        // change bool for input tracking and hide the choice buttons
        if(currentStory.currentChoices.Count <= 0)
        {
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                choiceButtons[i].SetActive(false);
            }

            isChoiceAvailable = false;
            return;
        }

        else
        {
            isChoiceAvailable = true;
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
        choiceButtons[currentChoiceIndex].GetComponent<Image>().color = new Color32(255, 218, 142, 255);

        Debug.Log("Choice color changed.");
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
        GameEventsManager.instance.dialogueEvents.EndDialogue();

        // stop listening the dialogue variable changes in the current story
        dialogueVariables.StopListening(currentStory);

        isDialoguePlaying = false;
        dialogueText.text = "";

        exitButton.SetActive(false);
        dialogueCanvas.SetActive(false);

        StartCoroutine(DelayBetweenDialogues());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public Ink.Runtime.Object GetDialogueVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;

        dialogueVariables.variables.TryGetValue(variableName, out variableValue);

        if(variableValue == null)
        {
            Debug.Log("Ink Variables was found to be null: " + variableName);
        }

        //else
        //{
        //    Debug.Log("Fetched value is: " + variableValue);
        //}

        return variableValue;
    }

    // delay between dialogues to prevent a bug from moving from one dialogue to another with the same character without player pressing any keys
    private IEnumerator DelayBetweenDialogues()
    {
        canStartDialogue = false;

        yield return new WaitForSeconds(3f);

        canStartDialogue = true;
    }
    private void OnLevelWasLoaded(int level)
    {
        //instance = this;
        //dialogueCanvas = transform.GetChild(0).gameObject;
    }
}
