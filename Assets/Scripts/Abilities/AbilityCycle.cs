using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using System;
public class AbilityCycle : MonoBehaviour
{
    public static AbilityCycle instance;

    [SerializeField] private TMP_Text abilityUIText;
    public Dictionary<Abilities, bool> activeAbilities = new Dictionary<Abilities, bool>();
    private List<Abilities> abilityKeys;
    private Abilities selectedAbility = Abilities.None;
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
        if (PlayerInputHandler.instance.AbilityToggleInput.WasPressedThisFrame())
        {
            //activeAbilities.TryGetValue(selectedAbility, out bool isSelected);
            //Debug.Log("1. Selected ability is: " + selectedAbility + " and it is: " + isSelected);
            activeAbilities[selectedAbility] = false;

            int currentIndex = abilityKeys.IndexOf(selectedAbility);

            if (currentIndex != -1)
            {
                bool isSelectedUnlocked = AbilityManager.instance.abilityStatuses[selectedAbility];
                if (isSelectedUnlocked)
                {
                    //this modulo thing wraps back to 0 when it reaches the end of a list
                    currentIndex = (currentIndex + 1) % abilityKeys.Count;
                    selectedAbility = abilityKeys[currentIndex];
                    activeAbilities[selectedAbility] = true;

                    abilityUIText.text = "Selected Ability: " + selectedAbility;
                    abilityUIText.color = Color.black;
                    StartCoroutine(DelayFadeTextToFullAlpha(2f, abilityUIText));

                    //activeAbilities.TryGetValue(selectedAbility, out bool isSelected2);
                    //Debug.Log("2. Selected ability is: " + selectedAbility + " and it is: " + isSelected2);
                }
                else
                {
                    abilityUIText.text = "You haven't unlocked that ability yet.";
                    abilityUIText.color = Color.black;
                    StartCoroutine(DelayFadeTextToFullAlpha(2f, abilityUIText));
                }
            }
            else
            {
                Debug.LogError($"Switching abilities failed. currentIndex: {currentIndex}, selectedAbility: {selectedAbility}");
            }
        }
    }
    public IEnumerator DelayFadeTextToFullAlpha(float t, TMP_Text i)
    {
        yield return new WaitForSeconds(1);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}