using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] Slider masterVolumeSlider; //Note: Does this have to be public? could this be [SerializeField] private and get method instead?
    [SerializeField] Slider musicVolumeSlider; //Note: Does this have to be public? could this be [SerializeField] private and get method instead?
    [SerializeField] float MasterSliderValue;
    [SerializeField] float MusicSliderValue;
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioMixerGroup masterVolume, musicVolume; //Note: Does this have to be public? could this be [SerializeField] private and get method instead?

    public void Start()
    {
        if (PlayerPrefs.GetFloat("MasterVolume", MasterSliderValue) == 0) 
        {
            PlayerPrefs.SetFloat("MasterVolume", 0);
            mixer.SetFloat("MasterVolumeMixer", PlayerPrefs.GetFloat("MasterVolume"));
        }
        if (PlayerPrefs.GetFloat("MusicVolume", MusicSliderValue) == 0)
        {
            PlayerPrefs.SetFloat("MusicVolume", 0);
            mixer.SetFloat("MusicVolumeMixer", PlayerPrefs.GetFloat("MusicVolume"));
        }
        else
        {
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", MasterSliderValue);
            mixer.SetFloat("MusicVolumeMixer", PlayerPrefs.GetFloat("MusicVolume"));
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", MusicSliderValue);
            mixer.SetFloat("MusicVolumeMixer", PlayerPrefs.GetFloat("MusicVolume"));
        }
    }

    public void ChangeMasterSlider(float value) 
    {
        MasterSliderValue = value;
        PlayerPrefs.SetFloat("MasterVolume", MasterSliderValue);
        mixer.SetFloat("MasterVolumeMixer", PlayerPrefs.GetFloat("MasterVolume", MasterSliderValue));
    }
    public void ChangeMusicSlider(float value)
    {
        MusicSliderValue = value;
        PlayerPrefs.SetFloat("MusicVolume", MusicSliderValue);
        mixer.SetFloat("MusicVolumeMixer", PlayerPrefs.GetFloat("MusicVolume", MusicSliderValue));
    }
}
