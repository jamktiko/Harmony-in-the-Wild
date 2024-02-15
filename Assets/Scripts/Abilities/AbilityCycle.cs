using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AbilityCycle : MonoBehaviour
{
    
    [SerializeField] Dictionary<int, string> currentAbilitiesDictionary; // Note from Flavio: Doesn't appear to be used anymore? Replaced by currentAbilitiesList?
    [SerializeField]
    [System.Serializable]
    public struct AbilityData
    {
        public int abilityIndex;
        public string name;
        public bool isEnabled;
        public int officialIndex;
        public bool isActivated;
        public AbilityData(int ai,string n, bool e,int i)
        {
           abilityIndex = ai;
            name = n;
            isEnabled = e;
            officialIndex = i;
            isActivated = false;
        }
        
    }
    
    [SerializeField] List<AbilityData> abilitiesList;
    [SerializeField] List<AbilityData> currentAbilitiesList = new List<AbilityData>();
    [SerializeField] public AbilityData equippedAbility;
    [SerializeField] public int abilityIndex = 0;
    [SerializeField] TMP_Text abilityUIText;
    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(MakeList());
       
    }
    public IEnumerator MakeList()
    {
        yield return new WaitForSeconds(2f);
        abilitiesList = new List<AbilityData>()
        {
            new AbilityData(0, "ChargeJump", PlayerManager.instance.abilityValues[2], 2),
            new AbilityData(1, "Telegrab", PlayerManager.instance.abilityValues[6], 6),
            new AbilityData(2, "Freeze", PlayerManager.instance.abilityValues[7], 7)
        };
        currentAbilitiesList = abilitiesList.Where(x => x.isEnabled == true).ToList();

        if (currentAbilitiesList.Count > 0)
        {

            equippedAbility = currentAbilitiesList[abilityIndex];
        }

    }

        // Update is called once per frame
        void Update()
        {
            SwitchAbility();
        }

    void SwitchAbility() 
    {
        if (Input.GetKeyDown(KeyCode.Tab)&&currentAbilitiesList.Count!=0)
        {
            if (abilityIndex<currentAbilitiesList.Count-1)
            {
                abilityIndex++;
                equippedAbility = currentAbilitiesList[abilityIndex];
                abilityUIText.text = "Ability equipped: " + equippedAbility.name;
                abilityUIText.color = Color.black;
                StartCoroutine(DelayFadeTextToFullAlpha(2f, abilityUIText));
            }
            else
            {
                abilityIndex = 0;
                equippedAbility = currentAbilitiesList[abilityIndex];
                abilityUIText.text = "Ability equipped: " + equippedAbility.name;
                abilityUIText.color = Color.black;
                StartCoroutine(DelayFadeTextToFullAlpha(2f, abilityUIText));
            }

        }
        else if (Input.GetKeyDown(KeyCode.Tab) && currentAbilitiesList.Count == 0)
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
