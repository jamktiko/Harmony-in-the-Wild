using UnityEngine;
using UnityEngine.Serialization;

public class MenuOnLevelLoaded : MonoBehaviour
{
    [FormerlySerializedAs("pauseMenuPanel")] [SerializeField] private GameObject _pauseMenuPanel;
    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            _pauseMenuPanel.SetActive(false);
        }
    }
}
