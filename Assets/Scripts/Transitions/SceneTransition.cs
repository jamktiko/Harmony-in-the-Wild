using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private string goToScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(goToScene);
        }
    }
    public void GoToScene() 
    {
        SceneManager.LoadScene(goToScene);
    }
}
