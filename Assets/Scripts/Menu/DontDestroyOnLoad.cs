using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{
    [SerializeField] GameObject pausemenuManager;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Manager");
        if (objects.Length>1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        pausemenuManager=transform.GetChild(5).gameObject;
        if (SceneManager.GetActiveScene().name.Contains("Overwold")|| SceneManager.GetActiveScene().name.Contains("Dungeon"))
        {
            pausemenuManager.SetActive(true);
        }
        else
        {
            pausemenuManager.SetActive(false);
        }
    }
}
