using System.Collections.Generic;
using UnityEngine;

public class GraphicsSettingsResolutions : MonoBehaviour
{
    private Resolution[] resolutions;
    public TMPro.TMP_Dropdown resolutionDropdown; //Note: Work with [SerializeField] private and GetResolutionDropdown() if it needs to be accessed
    public Sprite dropDownSprite; //Note: Work with [SerializeField] private and GetDropDownSprite() if it needs to be accessed

    private void Start()
    {
        resolutions = Screen.resolutions;

        try
        {
            resolutionDropdown.ClearOptions();
        }
        catch (System.Exception)
        {

            throw;
        }
        //Point of clearing the options is to not have placeholders mess up the dropdown

        List<TMPro.TMP_Dropdown.OptionData> resOptions = new List<TMPro.TMP_Dropdown.OptionData>();

        int currentResolutionIndex = 0;

        //Code to gather possible resolutions into the dropdown
        for (int i = 0; i < resolutions.Length; i++)
        {
            string resOption = resolutions[i].width + " x " + resolutions[i].height;
            resOptions.Add(new TMPro.TMP_Dropdown.OptionData(resOption,dropDownSprite));

            //Setting default res
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}

