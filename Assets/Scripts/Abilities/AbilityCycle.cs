using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityCycle : MonoBehaviour
{
    
    [SerializeField] Dictionary<int, string> CurrentAbilities;
    [SerializeField]
    [System.Serializable]
    public struct AbilityData
    {
        public int Abilityindex;
        public string name;
        public bool enabled;
        public int officialIndex;
        public bool currentlyActivated;
        public AbilityData(int ai,string n, bool e,int i)
        {
           Abilityindex = ai;
            name = n;
            enabled = e;
            officialIndex = i;
            currentlyActivated = false;
        }
    }
    [SerializeField] List<AbilityData> Abilities;
    [SerializeField] List<AbilityData> currentAbilities=new List<AbilityData>();
    [SerializeField] public AbilityData equippedAbility;
    [SerializeField] public int abilityIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        Abilities=new List<AbilityData>() 
        {
            new AbilityData(0, "ChargeJump", PlayerManager.instance.abilityValues[2], 2), 
            new AbilityData(1, "Telegrab", PlayerManager.instance.abilityValues[6], 6), 
            new AbilityData(2, "Freeze", PlayerManager.instance.abilityValues[7], 7) 
        };

        
        if (Abilities.Count>0)
        {
            currentAbilities = Abilities.Where(x => x.enabled == true).ToList();
            equippedAbility = currentAbilities[abilityIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        switchAbility();
    }
    void switchAbility() 
    {
        if (Input.mouseScrollDelta.y>0&&currentAbilities.Count!=0)
        {
            if (abilityIndex<currentAbilities.Count-1)
            {
                abilityIndex++;
                equippedAbility = currentAbilities[abilityIndex];
            }
            else
            {
                abilityIndex = 0;
                equippedAbility = currentAbilities[abilityIndex];

            }

        }
        if (Input.mouseScrollDelta.y < 0&& currentAbilities.Count != 0)
        {
            if (abilityIndex > 0)
            {
                abilityIndex--;
                equippedAbility = currentAbilities[abilityIndex];

            }
            else
            {
                abilityIndex = currentAbilities.Count - 1;
                equippedAbility = currentAbilities[abilityIndex];

            }
        }
    }
    void activateAbility() 
    {
        
    }
}
