using UnityEngine;

public class TempActivateChargedJump : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.J))
        {
            AbilityManager.instance.EnableAbility(Abilities.ChargeJumping);
        }
    }
}
