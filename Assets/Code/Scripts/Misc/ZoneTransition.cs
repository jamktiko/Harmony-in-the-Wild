using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ZoneTransition : MonoBehaviour
{
    [Header("Audio Transition")]
    [SerializeField] private float transitionTime = 0.75f;
    
    [Header("Forest")]
    [SerializeField] AudioSource forestTheme;
    [SerializeField] private GameObject redFoxModel;

    [Header("Arctic")]
    [SerializeField] AudioSource arcticTheme;
    [SerializeField] private GameObject arcticFoxModel;

    [Header("Result")]
    [SerializeField] private UnityEvent onTriggerEnterEvent;

    private AudioSource currentTheme;
    private AudioSource targetTheme;
    private float maxArcticVolume;
    private float maxForestVolume;

    private bool enteringScene = true;

    private void Start()
    {
        //maxArcticVolume = arcticTheme.volume;
        //maxForestVolume = forestTheme.volume;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(onTriggerEnterEvent != null)
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
            //currentTheme = forestTheme;
            //targetTheme = arcticTheme;
        }

        else if(themeName == "Forest")
        {
            StartCoroutine(StartForestTheme());
            //currentTheme = arcticTheme;
            //targetTheme = forestTheme;
        }

        //StartCoroutine(MixThemes(currentTheme, targetTheme));
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

    private IEnumerator MixThemes(AudioSource nowPlaying, AudioSource target)
    {
        float percentage = 0;

        while(nowPlaying.volume > 0)
        {
            if (nowPlaying == arcticTheme)
            {
                nowPlaying.volume = Mathf.Lerp(maxArcticVolume, 0, percentage);
                percentage += Time.deltaTime / transitionTime;
                yield return null;
            }

            else
            {
                nowPlaying.volume = Mathf.Lerp(maxForestVolume, 0, percentage);
                percentage += Time.deltaTime / transitionTime;
                yield return null;
            }
        }

        nowPlaying.Stop();

        if(target.isPlaying == false)
        {
            target.volume = 0;
            target.Play();
        }

        percentage = 0;

        if(target == arcticTheme)
        {
            while(target.volume < maxArcticVolume)
            {
                target.volume = Mathf.Lerp(0, maxArcticVolume, percentage);
                percentage += Time.deltaTime / transitionTime;
                yield return null;
            }
        }

        else
        {
            while (target.volume < maxForestVolume)
            {
                target.volume = Mathf.Lerp(0, maxForestVolume, percentage);
                percentage += Time.deltaTime / transitionTime;
                yield return null;
            }
        }
    }
}


