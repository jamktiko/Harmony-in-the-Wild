using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadAfterStuck : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene("Overworld");
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
