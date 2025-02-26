using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private const string speaker = "speaker";

    [Header("Dialogue Canvas")]
    [SerializeField] private GameObject dialogueCanvas;

    [Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerText;
    //[SerializeField] private GameObject exitButton;

    [Header("Choices")]
    [SerializeField] private bool isChoiceAvailable;
    [SerializeField] private int currentChoiceIndex = 0;
    [SerializeField] private GameObject[] choiceButtons;

    [Header("Public Values for References")]
    public bool isDialoguePlaying = false;
    public bool canStartDialogue = true;

    [Header("Ink Globals")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    // private variables, no need to show in the inspector
    private DialogueVariableObserver dialogueVariables;
    private Story currentStory;
    private TextMeshProUGUI[] choicesText;
    private bool canInteractWith = true;   // boolean to detect whether you can use the input; not interactable if for example pause menu is opened
    private Coroutine dialogueCooldown = null;

    private void Awake()
    {
        // creating the instance for Dialogue Manager

        if(DialogueManager.instance != null)
        {
            Debug.LogWarning("There is more than one Dialogue Manager in the scene!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;

        }
    }

    private void Start()
    {
        Invoke(nameof(InitializeDialogueVariables),0.3f);

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

    private void OnEnable()
    {
        GameEventsManager.instance.playerEvents.OnToggleInputActions += ToggleInteractability;
        SceneManager.sceneLoaded += ResetInteractibilityOnSceneChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playerEvents.OnToggleInputActions -= ToggleInteractability;
        SceneManager.sceneLoaded -= ResetInteractibilityOnSceneChange;
    }

    private void Update()
    {
        if (isDialoguePlaying && canInteractWith)
        {
            // make the selected choice
            if (PlayerInputHandler.instance.SelectInput.WasPressedThisFrame() && isChoiceAvailable)
            {
                MakeChoice(currentChoiceIndex);
            }

            else if (PlayerInputHandler.instance.DialogueUpInput.WasPressedThisFrame() && isChoiceAvailable)
            {
                // if there is a choice available upper on the list, mark it as selected
                if (currentChoiceIndex > 0)
                {
                    ChangeCurrentChoice(currentChoiceIndex - 1);
                }
            }

            else if (PlayerInputHandler.instance.DialogueDownInput.WasPressedThisFrame() && isChoiceAvailable)
            {
                // if there is a choice available down on the list, mark it as selected
                if (currentChoiceIndex < currentStory.currentChoices.Count - 1)
                {
                    ChangeCurrentChoice(currentChoiceIndex + 1);
                }
            }

            // if there is still more dialogue to show, continue to the next section
            else if (PlayerInputHandler.instance.DialogueInput.WasPressedThisFrame() && currentStory.canContinue && !isChoiceAvailable)
            {
                ContinueDialogue();
            }

            // if there is no more dialogue to show, end the dialogue
            else if (PlayerInputHandler.instance.DialogueInput.WasPressedThisFrame() && !currentStory.canContinue && !isChoiceAvailable)
            {
                EndDialogue();
            }
        }
    }

    public void StartDialogue(TextAsset inkJSON)
    {
        if (!isDialoguePlaying && canStartDialogue)
        {
            Debug.Log("Start dialogue.");

            GameEventsManager.instance.dialogueEvents.StartDialogue();

            if (FoxMovement.instance.IsInWater())
            {
                FoxMovement.instance.playerAnimator.ReadyToSwim = false;
                FoxMovement.instance.playerAnimator.State = FoxAnimationState.Swimming;
            }
            else
                FoxMovement.instance.SetDefaultAnimatorValues();
            FoxMovement.instance.isSprinting=false;
            FoxMovement.instance.horizontalInput = 0;
            FoxMovement.instance.verticalInput = 0;

            currentStory = new Story(inkJSON.text);
            isDialoguePlaying = true;
            dialogueCanvas.SetActive(true);

            ContinueDialogue();
        }

        else
        {
            Debug.Log("Can't start dialogue right now, system on cooldown. You might be trying to talk to the same character too quickly.");
        }
    }

    public void ContinueDialogue()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();

            HandleTags(currentStory.currentTags);

            DisplayChoices();
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

                case "variableChange":
                    dialogueVariables.ChangeVariable(tagValue);
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
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogueVariables.CallVariableChangeEvent();

        GameEventsManager.instance.dialogueEvents.EndDialogue();

        SaveManager.instance.SaveGame();

        isDialoguePlaying = false;
        dialogueText.text = "";

        dialogueCanvas.SetActive(false);

        if (dialogueCooldown == null)
        {
            Debug.Log("Starting a dialogue delay");
            dialogueCooldown = StartCoroutine(DelayBetweenDialogues());
        }

        else
        {
            Debug.Log("Dialogue already on cooldown");
        }
    }

    private void InitializeDialogueVariables()
    {
        dialogueVariables = new DialogueVariableObserver();
    }

    // delay between dialogues to prevent a bug from moving from one dialogue to another with the same character without player pressing any keys
    private IEnumerator DelayBetweenDialogues()
    {
        Debug.Log("Dialogue delay started.");
        canStartDialogue = false;

        yield return new WaitForSeconds(1.5f);

        canStartDialogue = true;
        dialogueCooldown = null;

        Debug.Log("Dialogue delay ended.");
    }

    public string CollectDialogueVariableDataForSaving()
    {
        if(dialogueVariables != null)
        {
            string dataToJSON = dialogueVariables.ConvertVariablesToString();

            return dataToJSON;
        }

        else
        {
            return "";
        }
    }

    private void ToggleInteractability(bool enableInteractions)
    {
        canInteractWith = enableInteractions;

        if (canInteractWith)
        {
            canStartDialogue = true;
            dialogueCooldown = null;
        }

        Debug.Log("Dialogue interactability: " + enableInteractions);
    }

    private void ResetInteractibilityOnSceneChange(Scene scene, LoadSceneMode mode)
    {
        if(dialogueCooldown != null)
        {
            StopCoroutine(dialogueCooldown);
        }

        dialogueCooldown = null;
        canStartDialogue = true;
        canInteractWith = true;
    }
}