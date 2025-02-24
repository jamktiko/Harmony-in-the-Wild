using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Themes")]
    [SerializeField] private AudioSource themeAudioSource;
    [SerializeField] private float themeTransitionTimeIn;
    [SerializeField] private float themeTransitionTimeOut;
    [HideInInspector] public bool themeTransitionOn = false;
    [HideInInspector] public bool themeIsPlaying;
    [SerializeField] private List<ThemeData> themeList;

    [Header("SFX")]
    [SerializeField] private List<AudioObject> audioList;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("There is more than one Audio Manager in the scene!");
            Destroy(gameObject);
        }

        else
        {
            instance = this;
        }
    }

    public void PlaySound(AudioName audioToPlay, Transform parent)
    {
        AudioObject newAudioData = new AudioObject();

        // locate new audio object
        foreach(AudioObject audioObject in audioList)
        {
            if(audioObject.name == audioToPlay)
            {
                newAudioData = audioObject;
                break;
            }
        }

        GameObject spawnedAudioObject = Instantiate(newAudioData.audioPrefab, parent);

        // detach the object from the parent; if the parent is destroyed, the audio should still continue playing
        if (!newAudioData.forceToBeUnderPlayer)
        {
            spawnedAudioObject.transform.parent = null;
        }

        // the objects that need to be left under the parent are related to abilities, so they should be under player object
        // however, the call for the audio to start currently needs to be in ability manager, so it doesn't have the reference
        else
        {
            spawnedAudioObject.transform.parent = FoxMovement.instance.transform;
            spawnedAudioObject.transform.position = spawnedAudioObject.transform.parent.position;
        }
    }

    public void EndCurrentTheme()
    {
        if(themeAudioSource.clip == null)
        {
            Debug.Log("No theme clip playing, cannot stop it.");
            return;
        }

        themeTransitionOn = true;

        Debug.Log($"About to finish theme: {themeAudioSource.clip.name}.");

        StartCoroutine(ChangeThemeVolume(false, themeAudioSource.volume));
    }

    public void StartNewTheme(ThemeName themeToPlay)
    {
        themeTransitionOn = true;

        ThemeData newThemeData = new ThemeData();

        foreach(ThemeData theme in themeList)
        {
            if(theme.name == themeToPlay)
            {
                newThemeData = theme;
                break;
            }
        }

        themeAudioSource.clip = newThemeData.clip;
        themeAudioSource.Play();

        Debug.Log($"About to start theme: {themeToPlay}.");

        StartCoroutine(ChangeThemeVolume(true, newThemeData.maxVolume));
    }

    private IEnumerator ChangeThemeVolume(bool increaseVolume, float maxVolume)
    {
        float percentage = 0;

        if (increaseVolume)
        {
            themeAudioSource.volume = 0f;

            while (themeAudioSource.volume < maxVolume)
            {
                themeAudioSource.volume = Mathf.Lerp(0, maxVolume, percentage);
                percentage += Time.deltaTime / themeTransitionTimeIn;
                yield return null;
            }

            themeIsPlaying = true;
        }

        else
        {
            while (themeAudioSource.volume > 0)
            {
                themeAudioSource.volume = Mathf.Lerp(maxVolume, 0, percentage);
                percentage += Time.deltaTime / themeTransitionTimeOut;
                yield return null;
            }

            themeAudioSource.clip = null;
            themeIsPlaying = false;
        }

        themeTransitionOn = false;
    }
}