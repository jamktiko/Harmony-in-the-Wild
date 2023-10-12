using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishDungeon : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int gainedAbilityIndex;
    [SerializeField] private string goToScene;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ReturnToOverworld());
        }
    }

    private IEnumerator ReturnToOverworld()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length + 0.5f);

        SceneManager.LoadScene(goToScene);
    }
}
