using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ZoneTransition : MonoBehaviour
{
    [Header("Result")]
    [SerializeField] private UnityEvent onTriggerEnterEvent;

    private bool canTriggerAudioChange = true;

    private void Awake()
    {
        GameEventsManager.instance.uiEvents.OnUseUnstuckButton += DisableAudioChangeForAWhile;
        GameEventsManager.instance.cinematicsEvents.OnStartCinematics += DisableAudioChangeForCinematics;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.uiEvents.OnUseUnstuckButton -= DisableAudioChangeForAWhile;
        GameEventsManager.instance.cinematicsEvents.OnStartCinematics -= DisableAudioChangeForCinematics;
    }

    private void DisableAudioChangeForAWhile()
    {
        canTriggerAudioChange = false;

        Invoke(nameof(EnableAudioChange), 1f);
    }

    private void EnableAudioChange()
    {
        canTriggerAudioChange = true;
    }

    private void DisableAudioChangeForCinematics()
    {
        canTriggerAudioChange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(onTriggerEnterEvent != null && canTriggerAudioChange)
        {
            onTriggerEnterEvent.Invoke();
        }
    }

    public void ChangeThemeTo(string themeName)
    {
        AudioManager.instance.EndCurrentTheme();

        if (themeName == "Arctic")
        {
            StartCoroutine(StartArcticTheme());
        }

        else if(themeName == "Forest")
        {
            StartCoroutine(StartForestTheme());
        }
    }

    private IEnumerator StartArcticTheme()
    {
        Debug.Log("Waiting for arctic theme transition to be triggered...");

        yield return new WaitUntil(() => AudioManager.instance.themeTransitionOn == false);

        Debug.Log("Arctic theme about to be triggered...");

        AudioManager.instance.StartNewTheme(ThemeName.Theme_Arctic);
    }

    private IEnumerator StartForestTheme()
    {
        Debug.Log("Waiting for forest theme transition to be triggered...");

        yield return new WaitUntil(() => AudioManager.instance.themeTransitionOn == false);

        Debug.Log("Forest theme about to be triggered...");

        AudioManager.instance.StartNewTheme(ThemeName.Theme_Forest);
    }
}


