using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCycle : MonoBehaviour
{
    public static AbilityCycle Instance;

    [SerializeField] private TMP_Text _abilityUIText;
    [SerializeField] private Image _abilityBackground;
    public Dictionary<Abilities, bool> ActiveAbilities = new Dictionary<Abilities, bool>();
    private List<Abilities> _abilityKeys;
    public Abilities SelectedAbility = Abilities.None;

    private bool _canInteractWith = true;   // boolean to detect whether you can use the input; not interactable if for example pause menu is opened

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more than one AbilityCycle.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    void Start()
    {
        InitializeAbilities();
        _abilityKeys = new List<Abilities>(ActiveAbilities.Keys);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.PlayerEvents.OnToggleInputActions += ToggleInteractability;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.PlayerEvents.OnToggleInputActions -= ToggleInteractability;
    }

    void Update()
    {
        SwitchAbility();
    }
    public void InitializeAbilities()
    {
        ActiveAbilities.Add(Abilities.None, true);
        ActiveAbilities.Add(Abilities.ChargeJumping, false);
        ActiveAbilities.Add(Abilities.TeleGrabbing, false);
        ActiveAbilities.Add(Abilities.Freezing, false);
    }
    private void SwitchAbility()
    {
        if (PlayerInputHandler.Instance.AbilityToggleInput.WasPressedThisFrame() && _canInteractWith)
        {
            ActiveAbilities.TryGetValue(SelectedAbility, out bool isSelected);
            //Debug.Log("1. Selected ability is: " + selectedAbility + " and it is: " + isSelected);
            ActiveAbilities[SelectedAbility] = false;

            int currentIndex = _abilityKeys.IndexOf(SelectedAbility);

            if (currentIndex != -1)
            {
                //this modulo thing wraps back to 0 when it reaches the end of a list
                currentIndex = (currentIndex + 1) % _abilityKeys.Count;
                SelectedAbility = _abilityKeys[currentIndex];

                bool isSelectedUnlocked = AbilityManager.Instance.AbilityStatuses[SelectedAbility];
                if (isSelectedUnlocked)
                {
                    ActiveAbilities[SelectedAbility] = true;

                    _abilityUIText.text = "Selected Ability: " + SelectedAbility;
                    _abilityUIText.color = Color.black;
                    _abilityBackground.color = Color.white;
                    StartCoroutine(DelayFadeTextToFullAlpha(2f, _abilityUIText, _abilityBackground));

                    //activeAbilities.TryGetValue(selectedAbility, out bool isSelected2);
                    //Debug.Log("2. Selected ability is: " + selectedAbility + " and it is: " + isSelected2);
                }
                else
                {
                    _abilityUIText.text = "You haven't unlocked that ability yet.";
                    _abilityUIText.color = Color.black;
                    _abilityBackground.color = Color.white;
                    StartCoroutine(DelayFadeTextToFullAlpha(2f, _abilityUIText, _abilityBackground));
                }
            }
            else
            {
                Debug.LogError($"Switching abilities failed. currentIndex: {currentIndex}, selectedAbility: {SelectedAbility}");
            }
        }
    }
    public IEnumerator DelayFadeTextToFullAlpha(float t, TMP_Text i, Image p)
    {
        yield return new WaitForSeconds(1);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        p.color = new Color(p.color.r, p.color.g, p.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            p.color = new Color(p.color.r, p.color.g, p.color.b, p.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    private void ToggleInteractability(bool enableInteractions)
    {
        _canInteractWith = enableInteractions;
    }
}