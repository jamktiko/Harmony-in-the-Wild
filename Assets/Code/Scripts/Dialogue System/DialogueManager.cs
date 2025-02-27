using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private const string _speaker = "speaker";

    [FormerlySerializedAs("dialogueCanvas")]
    [Header("Dialogue Canvas")]
    [SerializeField] private GameObject _dialogueCanvas;

    [FormerlySerializedAs("dialogueText")]
    [Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [FormerlySerializedAs("speakerText")] [SerializeField] private TextMeshProUGUI _speakerText;
    //[SerializeField] private GameObject exitButton;

    [FormerlySerializedAs("isChoiceAvailable")]
    [Header("Choices")]
    [SerializeField] private bool _isChoiceAvailable;
    [FormerlySerializedAs("currentChoiceIndex")] [SerializeField] private int _currentChoiceIndex = 0;
    [FormerlySerializedAs("choiceButtons")] [SerializeField] private GameObject[] _choiceButtons;

    [FormerlySerializedAs("isDialoguePlaying")] [Header("Public Values for References")]
    public bool IsDialoguePlaying = false;
    [FormerlySerializedAs("canStartDialogue")] public bool CanStartDialogue = true;

    [FormerlySerializedAs("loadGlobalsJSON")]
    [Header("Ink Globals")]
    [SerializeField] private TextAsset _loadGlobalsJson;

    // private variables, no need to show in the inspector
    private DialogueVariableObserver _dialogueVariables;
    private Story _currentStory;
    private TextMeshProUGUI[] _choicesText;
    private bool _canInteractWith = true;   // boolean to detect whether you can use the input; not interactable if for example pause menu is opened
    private Coroutine _dialogueCooldown = null;

    private void Awake()
    {
        // creating the instance for Dialogue Manager

        if (DialogueManager.Instance != null)
        {
            Debug.LogWarning("There is more than one Dialogue Manager in the scene!");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

        }
    }

    private void Start()
    {
        Invoke(nameof(InitializeDialogueVariables), 0.3f);

        _dialogueCanvas.SetActive(false);
        IsDialoguePlaying = false;

        // initializing choice button texts
        _choicesText = new TextMeshProUGUI[_choiceButtons.Length];

        for (int i = 0; i < _choiceButtons.Length; i++)
        {
            _choicesText[i] = _choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            _choiceButtons[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.PlayerEvents.OnToggleInputActions += ToggleInteractability;
        SceneManager.sceneLoaded += ResetInteractibilityOnSceneChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.PlayerEvents.OnToggleInputActions -= ToggleInteractability;
        SceneManager.sceneLoaded -= ResetInteractibilityOnSceneChange;
    }

    private void Update()
    {
        if (IsDialoguePlaying && _canInteractWith)
        {
            // make the selected choice
            if (PlayerInputHandler.Instance.SelectInput.WasPressedThisFrame() && _isChoiceAvailable)
            {
                MakeChoice(_currentChoiceIndex);
            }

            else if (PlayerInputHandler.Instance.DialogueUpInput.WasPressedThisFrame() && _isChoiceAvailable)
            {
                // if there is a choice available upper on the list, mark it as selected
                if (_currentChoiceIndex > 0)
                {
                    ChangeCurrentChoice(_currentChoiceIndex - 1);
                }
            }

            else if (PlayerInputHandler.Instance.DialogueDownInput.WasPressedThisFrame() && _isChoiceAvailable)
            {
                // if there is a choice available down on the list, mark it as selected
                if (_currentChoiceIndex < _currentStory.currentChoices.Count - 1)
                {
                    ChangeCurrentChoice(_currentChoiceIndex + 1);
                }
            }

            // if there is still more dialogue to show, continue to the next section
            else if (PlayerInputHandler.Instance.DialogueInput.WasPressedThisFrame() && _currentStory.canContinue && !_isChoiceAvailable)
            {
                ContinueDialogue();
            }

            // if there is no more dialogue to show, end the dialogue
            else if (PlayerInputHandler.Instance.DialogueInput.WasPressedThisFrame() && !_currentStory.canContinue && !_isChoiceAvailable)
            {
                EndDialogue();
            }
        }
    }

    public void StartDialogue(TextAsset inkJson)
    {
        if (!IsDialoguePlaying && CanStartDialogue)
        {
            Debug.Log("Start dialogue.");

            GameEventsManager.instance.DialogueEvents.StartDialogue();

            if (FoxMovement.Instance.IsInWater())
            {
                FoxMovement.Instance.playerAnimator.ReadyToSwim = false;
                FoxMovement.Instance.playerAnimator.State = FoxAnimationState.Swimming;
            }
            else
                FoxMovement.Instance.SetDefaultAnimatorValues();
            FoxMovement.Instance.IsSprinting = false;
            FoxMovement.Instance.HorizontalInput = 0;
            FoxMovement.Instance.VerticalInput = 0;

            _currentStory = new Story(inkJson.text);
            IsDialoguePlaying = true;
            _dialogueCanvas.SetActive(true);

            ContinueDialogue();
        }

        else
        {
            Debug.Log("Can't start dialogue right now, system on cooldown. You might be trying to talk to the same character too quickly.");
        }
    }

    public void ContinueDialogue()
    {
        if (_currentStory.canContinue)
        {
            _dialogueText.text = _currentStory.Continue();

            HandleTags(_currentStory.currentTags);

            DisplayChoices();
        }
    }

    private void DisplayChoices()
    {
        // change bool for input tracking and hide the choice buttons
        if (_currentStory.currentChoices.Count <= 0)
        {
            for (int i = 0; i < _choiceButtons.Length; i++)
            {
                _choiceButtons[i].SetActive(false);
            }

            _isChoiceAvailable = false;
            return;
        }

        else
        {
            _isChoiceAvailable = true;
        }

        List<Choice> currentChoices = _currentStory.currentChoices;

        // check if the UI can hold all the written choice options

        if (currentChoices.Count > _choiceButtons.Length)
        {
            Debug.LogWarning("There are more choices written than the UI can hold!");
        }

        // set each choice to show the correct text; if not all the slots are used, hide the extra ones

        int index = 0;

        foreach (Choice choice in currentChoices)
        {
            _choiceButtons[index].SetActive(true);
            _choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < _choiceButtons.Length; i++)
        {
            _choiceButtons[i].SetActive(false);
        }

        // set the first choice option as selected
        ChangeCurrentChoice(0);
    }

    private void ChangeCurrentChoice(int index)
    {
        // make last current choice button normal
        _choiceButtons[_currentChoiceIndex].GetComponent<Image>().color = Color.white;

        // make new current choice button with contrast color
        _currentChoiceIndex = index;
        _choiceButtons[_currentChoiceIndex].GetComponent<Image>().color = new Color32(255, 218, 142, 255);

        Debug.Log("Choice color changed.");
    }

    public void MakeChoice(int choiceIndex)
    {
        _currentStory.ChooseChoiceIndex(choiceIndex);

        ContinueDialogue();
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(":");

            if (splitTag.Length != 2)
            {
                Debug.LogWarning("Tag could not be appropriately parsed: " + tag);
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case _speaker:
                    if (tagValue == "Fox")
                    {
                        if (PlayerPrefs.GetString("foxName") != "" || PlayerPrefs.GetString("foxName") != null)
                        {
                            _speakerText.text = PlayerPrefs.GetString("foxName");
                        }

                        else
                        {
                            _speakerText.text = tagValue;
                        }
                    }
                    else
                    {
                        _speakerText.text = tagValue;
                    }
                    break;

                case "variableChange":
                    _dialogueVariables.ChangeVariable(tagValue);
                    break;

                default:
                    Debug.LogWarning("No tag key set for " + tag);
                    break;
            }
        }
    }

    public void CloseDialogueView()
    {
        if (!_currentStory.canContinue)
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        _dialogueVariables.CallVariableChangeEvent();

        GameEventsManager.instance.DialogueEvents.EndDialogue();

        SaveManager.Instance.SaveGame();

        IsDialoguePlaying = false;
        _dialogueText.text = "";

        _dialogueCanvas.SetActive(false);

        if (_dialogueCooldown == null)
        {
            Debug.Log("Starting a dialogue delay");
            _dialogueCooldown = StartCoroutine(DelayBetweenDialogues());
        }

        else
        {
            Debug.Log("Dialogue already on cooldown");
        }
    }

    private void InitializeDialogueVariables()
    {
        _dialogueVariables = new DialogueVariableObserver();
    }

    // delay between dialogues to prevent a bug from moving from one dialogue to another with the same character without player pressing any keys
    private IEnumerator DelayBetweenDialogues()
    {
        Debug.Log("Dialogue delay started.");
        CanStartDialogue = false;

        yield return new WaitForSeconds(1.5f);

        CanStartDialogue = true;
        _dialogueCooldown = null;

        Debug.Log("Dialogue delay ended.");
    }

    public string CollectDialogueVariableDataForSaving()
    {
        if (_dialogueVariables != null)
        {
            string dataToJson = _dialogueVariables.ConvertVariablesToString();

            return dataToJson;
        }

        else
        {
            return "";
        }
    }

    private void ToggleInteractability(bool enableInteractions)
    {
        _canInteractWith = enableInteractions;

        if (_canInteractWith)
        {
            CanStartDialogue = true;
            _dialogueCooldown = null;
        }

        Debug.Log("Dialogue interactability: " + enableInteractions);
    }

    private void ResetInteractibilityOnSceneChange(Scene scene, LoadSceneMode mode)
    {
        if (_dialogueCooldown != null)
        {
            StopCoroutine(_dialogueCooldown);
        }

        _dialogueCooldown = null;
        CanStartDialogue = true;
        _canInteractWith = true;
    }
}