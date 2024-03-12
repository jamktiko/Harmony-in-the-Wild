using UnityEngine;

public class AbilityTester : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            AbilityManager.instance.TryActivateAbility();
        }
    }
}
