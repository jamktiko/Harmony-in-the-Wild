using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

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

        if (!newAudioData.keepUnderParent)
        {
            spawnedAudioObject.transform.parent = null;
        }
    }
}