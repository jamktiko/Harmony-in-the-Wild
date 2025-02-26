using System.Collections;
using UnityEngine;

public class ShowLapFinishedEffect : MonoBehaviour
{
    private GameObject effect;

    private void Start()
    {
        effect = transform.GetChild(0).gameObject;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished += ShowEffect;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished -= ShowEffect;
    }

    private void ShowEffect()
    {
        effect.SetActive(true);
        effect.GetComponent<ParticleSystem>().Play();
    }

    private IEnumerator HideEffect()
    {
        yield return new WaitForSeconds(effect.GetComponent<ParticleSystem>().main.duration);
        effect.SetActive(false);
    }
}
