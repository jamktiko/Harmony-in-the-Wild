using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AbilityCycle : MonoBehaviour
{
    public static AbilityCycle instance;

    [SerializeField] private TMP_Text abilityUIText;
    [SerializeField] private Image abilityBackground;
    public Dictionary<Abilities, bool> activeAbilities = new Dictionary<Abilities, bool>();
    private List<Abilities> abilityKeys;
    public Abilities selectedAbility = Abilities.None;

    private bool canInteractWith = true;   // boolean to detect whether you can use the input; not interactable if for example pause menu is opened

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("There is more than one AbilityCycle.");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    void Start()
    {
        InitializeAbilities();
        abilityKeys = new List<Abilities>(activeAbilities.Keys);
    }

    private void OnEnable()
    {
        GameEventsManager.instance.playerEvents.OnToggleInputActions += ToggleInteractability;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playerEvents.OnToggleInputActions -= ToggleInteractability;
    }

    void Update()
    {
        SwitchAbility();
    }
    public void InitializeAbilities()
    {
        activeAbilities.Add(Abilities.None, true);
        activeAbilities.Add(Abilities.ChargeJumping, false);
        activeAbilities.Add(Abilities.TeleGrabbing, false);
        activeAbilities.Add(Abilities.Freezing, false);
    }
    private void SwitchAbility()
    {
        if (PlayerInputHandler.instance.AbilityToggleInput.WasPressedThisFrame() && canInteractWith)
        {
            activeAbilities.TryGetValue(selectedAbility, out bool isSelected);
            //Debug.Log("1. Selected ability is: " + selectedAbility + " and it is: " + isSelected);
            activeAbilities[selectedAbility] = false;

            int currentIndex = abilityKeys.IndexOf(selectedAbility);

            if (currentIndex != -1)
            {
                //this modulo thing wraps back to 0 when it reaches the end of a list
                currentIndex = (currentIndex + 1) % abilityKeys.Count;
                selectedAbility = abilityKeys[currentIndex];

                bool isSelectedUnlocked = AbilityManager.instance.abilityStatuses[selectedAbility];
                if (isSelectedUnlocked)
                {
                    activeAbilities[selectedAbility] = true;

                    abilityUIText.text = "Selected Ability: " + selectedAbility;
                    abilityUIText.color = Color.black;
                    abilityBackground.color = Color.white;
                    StartCoroutine(DelayFadeTextToFullAlpha(2f, abilityUIText, abilityBackground));

                    //activeAbilities.TryGetValue(selectedAbility, out bool isSelected2);
                    //Debug.Log("2. Selected ability is: " + selectedAbility + " and it is: " + isSelected2);
                }
                else
                {
                    abilityUIText.text = "You haven't unlocked that ability yet.";
                    abilityUIText.color = Color.black;
                    abilityBackground.color = Color.white;
                    StartCoroutine(DelayFadeTextToFullAlpha(2f, abilityUIText, abilityBackground));
                }
            }
            else
            {
                Debug.LogError($"Switching abilities failed. currentIndex: {currentIndex}, selectedAbility: {selectedAbility}");
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
        canInteractWith = enableInteractions;
    }
}