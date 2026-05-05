using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Screen_System : MonoBehaviour
{
    public Toggle toggle;

    //
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    // Start is called before the first frame update
    void Start()
    {
        if(Screen.fullScreen)
        {
            toggle.isOn=true;

        }

        else
        {
            toggle.isOn = false;
        }

        RevisarResolution();
    }

    public void FullScreen (bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
        if (fullScreen==true)
        {
            Debug.Log("Pantalla completa");
        }

        else
        {
            Debug.Log("Pantalla pequena");
        }
    }

    public void RevisarResolution()
    {
        resolutions=Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int actualResolution = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                actualResolution= i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value=actualResolution;
        resolutionDropdown.RefreshShownValue();

    }

    public void ChangeResolution (int resolutionIndex)
    {
       Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
