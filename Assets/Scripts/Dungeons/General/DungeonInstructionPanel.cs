using UnityEngine;

public class DungeonInstructionPanel : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Invoke(nameof(HideInstructionPanel), 0.1f);
        }
    }

    private void HideInstructionPanel()
    {
        gameObject.SetActive(false);
    }
}
