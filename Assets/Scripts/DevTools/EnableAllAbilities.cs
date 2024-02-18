using System.Collections;
using UnityEngine;

public class EnableAllAbilities : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(AbilityEnableDelay());
    }

    private IEnumerator AbilityEnableDelay()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 8; i++)
        {
            PlayerManager.instance.abilityValues[i] = true;
        }

        SaveManager.instance.SaveGame();
    }
}
