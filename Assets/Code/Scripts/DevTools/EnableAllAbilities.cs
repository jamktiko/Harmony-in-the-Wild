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

        foreach (Abilities abilities in AbilityManager.Instance.AbilityStatuses.Keys)
        {
            AbilityManager.Instance.AbilityStatuses[abilities] = true;
        }

        SaveManager.Instance.SaveGame();
    }
}
