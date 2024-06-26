using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOnLevelLoaded : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            pauseMenuPanel.SetActive(false);
        }
    }
}
