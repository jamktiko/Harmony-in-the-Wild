using System.Collections;
using UnityEngine;

public class ShowLapFinishedEffect : MonoBehaviour
{
    private GameObject _effect;

    private void Start()
    {
        _effect = transform.GetChild(0).gameObject;
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapFinished += ShowEffect;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapFinished -= ShowEffect;
    }

    private void ShowEffect()
    {
        _effect.SetActive(true);
        _effect.GetComponent<ParticleSystem>().Play();
    }

    private IEnumerator HideEffect()
    {
        yield return new WaitForSeconds(_effect.GetComponent<ParticleSystem>().main.duration);
        _effect.SetActive(false);
    }
}
