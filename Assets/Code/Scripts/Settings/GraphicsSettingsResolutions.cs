using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GraphicsSettingsResolutions : MonoBehaviour
{
    private Resolution[] _resolutions;
    [FormerlySerializedAs("resolutionDropdown")] public TMPro.TMP_Dropdown ResolutionDropdown; //Note: Work with [SerializeField] private and GetResolutionDropdown() if it needs to be accessed
    [FormerlySerializedAs("dropDownSprite")] public Sprite DropDownSprite; //Note: Work with [SerializeField] private and GetDropDownSprite() if it needs to be accessed

    private void Start()
    {
        _resolutions = Screen.resolutions;

        try
        {
            ResolutionDropdown.ClearOptions();
        }
        catch (System.Exception)
        {

            throw;
        }
        //Point of clearing the options is to not have placeholders mess up the dropdown

        List<TMPro.TMP_Dropdown.OptionData> resOptions = new List<TMPro.TMP_Dropdown.OptionData>();

        int currentResolutionIndex = 0;

        //Code to gather possible resolutions into the dropdown
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string resOption = _resolutions[i].width + " x " + _resolutions[i].height;
            resOptions.Add(new TMPro.TMP_Dropdown.OptionData(resOption, DropDownSprite));

            //Setting default res
            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(resOptions);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}

