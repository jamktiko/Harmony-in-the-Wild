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
        MasterSliderValue = PlayerPrefs.GetFloat("MasterVolume", 0.0001f);
        MusicSliderValue = PlayerPrefs.GetFloat("MusicVolume", 0.0001f);

        if(MasterSliderValue <= 0 || MusicSliderValue <= 0)
        {
            MasterSliderValue = 1f;
            MusicSliderValue = 1f;

            PlayerPrefs.SetFloat("MasterVolume", MasterSliderValue);
            PlayerPrefs.SetFloat("MusicVolume", MusicSliderValue);
        }

        //set master volume
        masterVolumeSlider.value = MasterSliderValue;
        mixer.SetFloat("MasterVolumeMixer", Mathf.Log10(MasterSliderValue) * 20f);
        //set music volume
        musicVolumeSlider.value = MusicSliderValue;
        mixer.SetFloat("MusicVolumeMixer", Mathf.Log10(MusicSliderValue) * 20f);
    }

    public void ChangeMasterSlider(float value) 
    {
        MasterSliderValue = value;
        PlayerPrefs.SetFloat("MasterVolume", MasterSliderValue);
        mixer.SetFloat("MasterVolumeMixer", Mathf.Log10(MasterSliderValue) * 20f);
    }
    public void ChangeMusicSlider(float value)
    {
        MusicSliderValue = value;
        PlayerPrefs.SetFloat("MusicVolume", MusicSliderValue);
        mixer.SetFloat("MusicVolumeMixer", Mathf.Log10(MusicSliderValue) * 20f);
    }
}
