using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;
    public float SliderValue;
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
