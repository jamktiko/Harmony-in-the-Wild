using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class DevToolButtons : MonoBehaviour
{
#if DEBUG
    [FormerlySerializedAs("pageParent")] [SerializeField] private Transform _pageParent;

    private void Update()
    {
        if (PlayerInputHandler.Instance.DebugDevToolsInput.WasPressedThisFrame())
        {
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeInHierarchy);
        }
    }

    public void ToggleVisiblePage(int pageIndex)
    {
        for (int i = 0; i < _pageParent.childCount; i++)
        {
            if (i == pageIndex)
            {
                _pageParent.GetChild(i).gameObject.SetActive(true);
            }

            else
            {
                _pageParent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void SaveAbilityChanges()
    {
        SaveManager.Instance.SaveGame();
    }

    public void ResetAbilities()
    {

        foreach (Abilities abilities in AbilityManager.Instance.AbilityStatuses.Keys)
        {
            AbilityManager.Instance.AbilityStatuses[abilities] = false;
        }

        SaveManager.Instance.SaveGame();
    }

    public void DeleteSaveFile()
    {
        File.Delete(Application.persistentDataPath + "/GameData.json");
        Debug.LogError("The save file has been deleted. Please restart the game to avoid any errors.");
    }
#endif
}
