using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    [SerializeField] TMP_Text abilityUIText;
    // Start is called before the first frame update
    void Start()
    {
        Abilities=new List<AbilityData>() 
        {
            new AbilityData(0, "ChargeJump", PlayerManager.instance.abilityValues[2], 2), 
            new AbilityData(1, "Telegrab", PlayerManager.instance.abilityValues[6], 6), 
            new AbilityData(2, "Freeze", PlayerManager.instance.abilityValues[7], 7) 
        };
        currentAbilities = Abilities.Where(x => x.enabled == true).ToList();

        if (currentAbilities.Count>0)
        {
            
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
        if (Input.GetKeyDown(KeyCode.Tab)&&currentAbilities.Count!=0)
        {
            if (abilityIndex<currentAbilities.Count-1)
            {
                abilityIndex++;
                equippedAbility = currentAbilities[abilityIndex];
                abilityUIText.text = "Ability equipped: " + equippedAbility.name;
                abilityUIText.color = Color.black;
                StartCoroutine(DelayFadeTextToFullAlpha(2f, abilityUIText));
            }
            else
            {
                abilityIndex = 0;
                equippedAbility = currentAbilities[abilityIndex];
                abilityUIText.text = "Ability equipped: " + equippedAbility.name;
                abilityUIText.color = Color.black;
                StartCoroutine(DelayFadeTextToFullAlpha(2f, abilityUIText));
            }

        }
        else if (Input.GetKeyDown(KeyCode.Tab) && currentAbilities.Count == 0)
        {
            abilityUIText.text = "You haven't unlocked any abilites yet!";
            abilityUIText.color = Color.black;
            StartCoroutine(DelayFadeTextToFullAlpha(2f, abilityUIText));
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
