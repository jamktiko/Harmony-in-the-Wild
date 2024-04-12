using System.Collections;
using UnityEngine;

public class DungeonAbilityActivator : MonoBehaviour
{
    [Header("Config")]
    [Tooltip("Index of the major ability gained in this dungeon. If set to -1, no major abilities are activated.")]
    [SerializeField] private int enabledAbility;

    private void Start()
    {
        StartCoroutine(ActivateAbilities());
    }

    private IEnumerator ActivateAbilities()
    {
        yield return new WaitForSeconds(1f);

        // if enabled ability index is set to -1, enable only minor abilities
        //note: replace with AbilityManager stuff
        //if (enabledAbility < 0)
        //{
        //    for (int i = 0; i < PlayerManager.instance.hasAbilityValues.Count; i++)
        //    {
        //        // enable the minor abilities (passive abilities)
        //        if (i == 1 || i == 3 || i == 4 || i == 5)
        //        {
        //            PlayerManager.instance.hasAbilityValues[i] = true;
        //        }

        //        // disable all the other major abilities
        //        else
        //        {
        //            PlayerManager.instance.hasAbilityValues[i] = false;
        //        }
        //    }

        //}

        //else
        //{
        //    for (int i = 0; i < PlayerManager.instance.hasAbilityValues.Count; i++)
        //    {
        //        // enable the major ability gained in this dungeon
        //        if (i == enabledAbility)
        //        {
        //            PlayerManager.instance.hasAbilityValues[i] = true;
        //        }

        //        // enable the minor abilities (passive abilities)
        //        else if (i == 1 || i == 3 || i == 4 || i == 5)
        //        {
        //            PlayerManager.instance.hasAbilityValues[i] = true;
        //        }

        //        // disable all the other major abilities
        //        else
        //        {
        //            PlayerManager.instance.hasAbilityValues[i] = false;
        //        }
        //    }
        //}
    }
}

