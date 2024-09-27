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
        MasterSliderValue = PlayerPrefs.GetFloat("MasterVolume",0);
        MusicSliderValue = PlayerPrefs.GetFloat("MusicVolume",0);
        //set master volume
        masterVolumeSlider.value = MasterSliderValue;
        mixer.SetFloat("MasterVolumeMixer", MasterSliderValue);
        //set music volume
        musicVolumeSlider.value = MusicSliderValue;
        mixer.SetFloat("MusicVolumeMixer", MusicSliderValue);
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
