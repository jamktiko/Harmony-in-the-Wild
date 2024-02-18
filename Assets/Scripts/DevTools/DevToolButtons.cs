using UnityEngine;
using System.IO;

public class DevToolButtons : MonoBehaviour
{
    [SerializeField] private Transform pageParent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeInHierarchy);
        }
    }

    public void ToggleVisiblePage(int pageIndex)
    {
        for(int i = 0; i < pageParent.childCount; i++)
        {
            if(i == pageIndex)
            {
                pageParent.GetChild(i).gameObject.SetActive(true);
            }

            else
            {
                pageParent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void SaveAbilityChanges()
    {
        SaveManager.instance.SaveGame();
    }

    public void ResetAbilities()
    {
        for (int i = 0; i < 8; i++)
        {
            PlayerManager.instance.hasAbilityValues[i] = false;
        }

        SaveManager.instance.SaveGame();
    }

    public void DeleteSaveFile()
    {
        File.Delete(Application.persistentDataPath + "/gameData.dat");
        Debug.LogError("The save file has been deleted. Please restart the game to avoid any errors.");
    }
}
