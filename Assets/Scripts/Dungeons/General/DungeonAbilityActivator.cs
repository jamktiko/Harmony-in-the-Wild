using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonAbilityActivator : MonoBehaviour
{
    [Header("Config")]
    [Tooltip("Index of the major ability gained in this dungeon. If set to -1, no major abilities are activated.")]
    [SerializeField] private int enabledAbility;

    private void Start()
    {
        if(enabledAbility < 0)
        {
            for (int i = 0; i < PlayerManager.instance.abilityValues.Count; i++)
            {
                // enable the major ability gained in this dungeon
                if (i == enabledAbility)
                {
                    PlayerManager.instance.abilityValues[i] = true;
                }

                // enable the minor abilities (passive abilities)
                else if (i == 1 || i == 3 || i == 4 || i == 5)
                {
                    PlayerManager.instance.abilityValues[i] = true;
                }

                // disable all the other major abilities
                else
                {
                    PlayerManager.instance.abilityValues[i] = false;
                }
            }

            return;
        }

        for(int i = 0; i < PlayerManager.instance.abilityValues.Count; i++)
        {
            // enable the major ability gained in this dungeon
            if(i == enabledAbility)
            {
                PlayerManager.instance.abilityValues[i] = true;
            }

            // enable the minor abilities (passive abilities)
            else if (i == 1 || i == 3 || i == 4 || i == 5)
            {
                PlayerManager.instance.abilityValues[i] = true;
            }

            // disable all the other major abilities
            else
            {
                PlayerManager.instance.abilityValues[i] = false;
            }
        }
    }
}
