using UnityEngine;
using System.Collections;
using UnityEngine.Events;

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

    private void Start()
    {
        maxArcticVolume = arcticTheme.volume;
        maxForestVolume = forestTheme.volume;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(onTriggerEnterEvent != null)
        {
            onTriggerEnterEvent.Invoke();
        }

        else
        {
            Debug.Log("No trigger events defined for trigger enter!");
        }

        //entered forest
        /*if (FoxMovement.instance.gameObject != null && arcticFoxModel.activeInHierarchy)
        {
            modelToggle.ChangeModelToForest();
            arcticTheme.Stop();
            forestTheme.Play();
        }

        //entered arctic
        if (FoxMovement.instance.gameObject != null && redFoxModel.activeInHierarchy)
        {
            modelToggle.ChangeModelToArctic();
            forestTheme.Stop();
            arcticTheme.Play();
        }*/

        //Debug.Log($"{name} entered");
    }

    public void ChangeThemeTo(string themeName)
    {
        if(themeName == "Arctic")
        {
            currentTheme = forestTheme;
            targetTheme = arcticTheme;
        }

        else if(themeName == "Forest")
        {
            currentTheme = arcticTheme;
            targetTheme = forestTheme;
        }

        StartCoroutine(MixThemes(currentTheme, targetTheme));
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


