using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown textResolutionDropdown;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutionsList;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;


    void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutionsList = new List<Resolution>();

        textResolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++) {
            if (resolutions[i].refreshRate==currentRefreshRate) {
                filteredResolutionsList.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i<filteredResolutionsList.Count;  i++) {
            string resolutionOptions = filteredResolutionsList[i].width + "x" + filteredResolutionsList[i].height + " " + filteredResolutionsList[i].refreshRate + "Hz";
            options.Add(resolutionOptions);

            if (filteredResolutionsList[i].width == Screen.width && filteredResolutionsList[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        textResolutionDropdown.AddOptions(options);
        textResolutionDropdown.value = currentResolutionIndex;
        textResolutionDropdown.RefreshShownValue();

    }

    public void SetResolution(int resolutionIndex) { 
        Resolution resolution = filteredResolutionsList[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
        PlayerPrefs.Save();
    }
}