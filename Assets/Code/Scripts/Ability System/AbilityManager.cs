using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

    public Dictionary<Abilities, bool> AbilityStatuses = new Dictionary<Abilities, bool>();
    [SerializeField] private GameObject _abilityPartsChild;

    private Dictionary<Abilities, IAbility> _abilities;
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more than one Ability Manager.");
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }


        _abilities = new Dictionary<Abilities, IAbility>();

        LoadAbilityData();
    }
    private void Start()
    {
    }
    private void Update()
    {
        KeepAbilityPartsAtPlayer();
    }

    public void ActivateAbilityIfUnlocked(Abilities abilityType)
    {
        //Debug.Log($"Tried activating ability: {abilityType}");
        if (_abilities.ContainsKey(abilityType))
        {
            if (_abilityStatuses.TryGetValue(abilityType, out bool isUnlocked) && isUnlocked)
            {
                _abilities[abilityType].Activate();
            }
            else
            {
                // Debug.Log($"Abilities cannot be activated right now or {abilityType} is not unlocked.");
            }
        }
    }

    public void RegisterAbility(Abilities abilityType, IAbility ability)
    {
        if (!_abilities.ContainsKey(abilityType))
        {
            _abilities.Add(abilityType, ability);
            //Debug.Log($"Registered ability: {abilityType}");
        }
        else
        {
            //Debug.Log($"Ability {abilityType} is already registered.");
        }
    }

    //Method to enable an ability
    public void UnlockAbility(Abilities abilityType)
    {
        if (_abilityStatuses.ContainsKey(abilityType))
        {
            _abilityStatuses[abilityType] = true;

            Debug.Log($"Ability {abilityType} has been unlocked.");
        }
        else
        {
            Debug.LogError($"Attempted to unlock an unrecognized ability: {abilityType}");
        }
    }

    // Method to lock an ability again if needed (most likely since you haven't yet completed the corresponding quest)
    public void LockAbility(Abilities abilityType)
    {
        if (_abilityStatuses.ContainsKey(abilityType))
        {
            _abilityStatuses[abilityType] = false;

            Debug.Log($"Ability {abilityType} has been locked again. Corresponding dungeon was not yet completed.");
        }
    }

    public string CollectAbilityDataForSaving()
    {
        string data = JsonConvert.SerializeObject(_abilityStatuses);

        //Debug.Log("data: " + data);
        return data;
    }

    public void LoadAbilityData()
    {
        _abilityStatuses = SaveManager.instance.GetLoadedAbilityDictionary();
    }

    void KeepAbilityPartsAtPlayer()
    {
        if (FoxMovement.instance != null && FoxMovement.instance.gameObject.transform.position != null)
        {
            _abilityPartsChild.transform.position = FoxMovement.instance.gameObject.transform.position;
        }
    }
}
