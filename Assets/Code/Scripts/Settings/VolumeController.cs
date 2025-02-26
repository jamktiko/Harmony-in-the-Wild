using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [FormerlySerializedAs("masterVolumeSlider")] [SerializeField] Slider _masterVolumeSlider; //Note: Does this have to be public? could this be [SerializeField] private and get method instead?
    [FormerlySerializedAs("musicVolumeSlider")] [SerializeField] Slider _musicVolumeSlider; //Note: Does this have to be public? could this be [SerializeField] private and get method instead?
    [FormerlySerializedAs("MasterSliderValue")] [SerializeField] float _masterSliderValue;
    [FormerlySerializedAs("MusicSliderValue")] [SerializeField] float _musicSliderValue;
    [FormerlySerializedAs("mixer")] [SerializeField] AudioMixer _mixer;
    [FormerlySerializedAs("masterVolume")] [SerializeField] AudioMixerGroup _masterVolume; //Note: Does this have to be public? could this be [SerializeField] private and get method instead?
    [FormerlySerializedAs("musicVolume")] [SerializeField] AudioMixerGroup _musicVolume; //Note: Does this have to be public? could this be [SerializeField] private and get method instead?

    public void Start()
    {
        _masterSliderValue = PlayerPrefs.GetFloat("MasterVolume", 0.0001f);
        _musicSliderValue = PlayerPrefs.GetFloat("MusicVolume", 0.0001f);

        if (_masterSliderValue <= 0 || _musicSliderValue <= 0)
        {
            _masterSliderValue = 1f;
            _musicSliderValue = 1f;

            PlayerPrefs.SetFloat("MasterVolume", _masterSliderValue);
            PlayerPrefs.SetFloat("MusicVolume", _musicSliderValue);
        }

        //set master volume
        _masterVolumeSlider.value = _masterSliderValue;
        _mixer.SetFloat("MasterVolumeMixer", Mathf.Log10(_masterSliderValue) * 20f);
        //set music volume
        _musicVolumeSlider.value = _musicSliderValue;
        _mixer.SetFloat("MusicVolumeMixer", Mathf.Log10(_musicSliderValue) * 20f);
    }

    public void ChangeMasterSlider(float value)
    {
        _masterSliderValue = value;
        PlayerPrefs.SetFloat("MasterVolume", _masterSliderValue);
        _mixer.SetFloat("MasterVolumeMixer", Mathf.Log10(_masterSliderValue) * 20f);
    }
    public void ChangeMusicSlider(float value)
    {
        _musicSliderValue = value;
        PlayerPrefs.SetFloat("MusicVolume", _musicSliderValue);
        _mixer.SetFloat("MusicVolumeMixer", Mathf.Log10(_musicSliderValue) * 20f);
    }
}
