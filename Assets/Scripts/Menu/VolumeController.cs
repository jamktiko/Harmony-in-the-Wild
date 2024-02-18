using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider; //Note: Does this have to be public? could this be [SerializeField] private and get method instead?
    public float SliderValue; //Note: Does this have to be public? could this be [SerializeField] private and get method instead?

    public void Start()
    {
        if (PlayerPrefs.GetFloat("save", SliderValue) == 0) 
        {
            PlayerPrefs.SetFloat("save", 100);
        }
        else
        {
            volumeSlider.value = PlayerPrefs.GetFloat("save", SliderValue);
            AudioListener.volume = PlayerPrefs.GetFloat("save", SliderValue);
        }
    }

    public void ChangeSlider(float value) 
    {
        SliderValue = value;
        PlayerPrefs.SetFloat("save", SliderValue);
        AudioListener.volume = PlayerPrefs.GetFloat("save");
    }
}
