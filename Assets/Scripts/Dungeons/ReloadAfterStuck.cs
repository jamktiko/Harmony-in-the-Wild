
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
    }
}
