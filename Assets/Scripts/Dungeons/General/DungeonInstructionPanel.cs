using UnityEngine;
using UnityEngine.VFX;

public class DungeonInstructionPanel : MonoBehaviour
{
    [SerializeField] private VisualEffect dungeonEntranceVFX;
    private int onDungeonStartID;

    private void Awake()
    {
        onDungeonStartID = Shader.PropertyToID("OnDungeonStart");

        // Check if the dungeonEntranceVFX is not assigned in the inspector
        if (dungeonEntranceVFX == null)
        {
            // Find the DungeonEntrance VFX by its name in the hierarchy
            var dungeonEntranceObject = GameObject.Find("DungeonEntrance");
            if (dungeonEntranceObject != null)
            {
                dungeonEntranceVFX = dungeonEntranceObject.GetComponent<VisualEffect>();

                if (dungeonEntranceVFX == null)
                {
                    Debug.LogWarning("VisualEffect component not found on DungeonEntrance object!");
                }
            }
            else
            {
                Debug.LogError("DungeonEntrance object not found in the hierarchy!");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Invoke(nameof(HideInstructionPanel), 0.1f);
            dungeonEntranceVFX.SendEvent("OnDungeonStart");
        }
    }

    private void HideInstructionPanel()
    {
        gameObject.SetActive(false);
    }
}
