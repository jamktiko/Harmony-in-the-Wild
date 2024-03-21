using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    private Dictionary<Abilities, bool> abilityStatuses = new Dictionary<Abilities, bool>();

    public bool CanActivateAbilities { get; set; } = false;

    private IAbility currentAbility;

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one Ability Manager.");
            Destroy(gameObject);
        }
        instance = this; ;

        currentAbility = GetComponent<IAbility>();

        InitializeAbilities();
    }

    private void Update()
    {
        CanActivateAbilitiesFlip();
    }

    private void InitializeAbilities()
    {
        // Initialize all abilities as false (disabled) by default
        foreach (Abilities ability in System.Enum.GetValues(typeof(Abilities)))
        {
            abilityStatuses[ability] = false;
            Debug.Log("ability: " + ability + ". Status: " + abilityStatuses);
        }
    }

    // This is for testing purposes.
    // Eventually the condition will be tied into the QuestManager & QuestRewards
    private void CanActivateAbilitiesFlip()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            CanActivateAbilities = !CanActivateAbilities;
            Debug.Log("CanActivateAbilities boolean = " + CanActivateAbilities);
        }
    }

    public void TryActivateAbility()
    {
        if (CanActivateAbilities)
        {
            currentAbility.Activate();
        }
        else
        {
            Debug.Log("Abilities cannot be activated right now.");
        }
    }
}
