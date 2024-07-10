using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadAfterStuck : MonoBehaviour
{
    private void Update()
    {
        if (PlayerInputHandler.instance.DebugReloadCurrentSceneInput.WasPressedThisFrame())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (PlayerInputHandler.instance.DebugReloadOverworldInput.WasPressedThisFrame())
        {
            SceneManager.LoadScene("Overworld");
        }

        if (PlayerInputHandler.instance.DebugReloadMainMenuInput.WasPressedThisFrame())
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
