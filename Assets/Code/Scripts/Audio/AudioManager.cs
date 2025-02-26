using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [FormerlySerializedAs("themeAudioSource")]
    [Header("Themes")]
    [SerializeField] private AudioSource _themeAudioSource;
    [FormerlySerializedAs("themeTransitionTimeIn")] [SerializeField] private float _themeTransitionTimeIn;
    [FormerlySerializedAs("themeTransitionTimeOut")] [SerializeField] private float _themeTransitionTimeOut;
    [FormerlySerializedAs("themeTransitionOn")] [HideInInspector] public bool ThemeTransitionOn = false;
    [FormerlySerializedAs("themeIsPlaying")] [HideInInspector] public bool ThemeIsPlaying;
    [FormerlySerializedAs("themeList")] [SerializeField] private List<ThemeData> _themeList;

    [FormerlySerializedAs("audioList")]
    [Header("SFX")]
    [SerializeField] private List<AudioObject> _audioList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than one Audio Manager in the scene!");
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioName audioToPlay, Transform parent)
    {
        AudioObject newAudioData = new AudioObject();

        // locate new audio object
        foreach (AudioObject audioObject in _audioList)
        {
            if (audioObject.Name == audioToPlay)
            {
                newAudioData = audioObject;
                break;
            }
        }

        GameObject spawnedAudioObject = Instantiate(newAudioData.AudioPrefab, parent);

        // detach the object from the parent; if the parent is destroyed, the audio should still continue playing
        if (!newAudioData.ForceToBeUnderPlayer)
        {
            spawnedAudioObject.transform.parent = null;
        }

        // the objects that need to be left under the parent are related to abilities, so they should be under player object
        // however, the call for the audio to start currently needs to be in ability manager, so it doesn't have the reference
        else
        {
            spawnedAudioObject.transform.parent = FoxMovement.Instance.transform;
            spawnedAudioObject.transform.position = spawnedAudioObject.transform.parent.position;
        }
    }

    public void EndCurrentTheme()
    {
        if (_themeAudioSource.clip == null)
        {
            Debug.Log("No theme clip playing, cannot stop it.");
            return;
        }

        ThemeTransitionOn = true;

        Debug.Log($"About to finish theme: {_themeAudioSource.clip.name}.");

        StartCoroutine(ChangeThemeVolume(false, _themeAudioSource.volume));
    }

    public void StartNewTheme(ThemeName themeToPlay)
    {
        ThemeTransitionOn = true;

        ThemeData newThemeData = new ThemeData();

        foreach (ThemeData theme in _themeList)
        {
            if (theme.Name == themeToPlay)
            {
                newThemeData = theme;
                break;
            }
        }

        _themeAudioSource.clip = newThemeData.Clip;
        _themeAudioSource.Play();

        Debug.Log($"About to start theme: {themeToPlay}.");

        StartCoroutine(ChangeThemeVolume(true, newThemeData.MaxVolume));
    }

    private IEnumerator ChangeThemeVolume(bool increaseVolume, float maxVolume)
    {
        float percentage = 0;

        if (increaseVolume)
        {
            _themeAudioSource.volume = 0f;

            while (_themeAudioSource.volume < maxVolume)
            {
                _themeAudioSource.volume = Mathf.Lerp(0, maxVolume, percentage);
                percentage += Time.deltaTime / _themeTransitionTimeIn;
                yield return null;
            }

            ThemeIsPlaying = true;
        }

        else
        {
            while (_themeAudioSource.volume > 0)
            {
                _themeAudioSource.volume = Mathf.Lerp(maxVolume, 0, percentage);
                percentage += Time.deltaTime / _themeTransitionTimeOut;
                yield return null;
            }

            _themeAudioSource.clip = null;
            ThemeIsPlaying = false;
        }

        ThemeTransitionOn = false;
    }
}