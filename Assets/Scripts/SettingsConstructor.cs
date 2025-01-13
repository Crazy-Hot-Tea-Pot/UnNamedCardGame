using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A constructor for the settings menu in the ui for saving it's various components
/// </summary>
public class SettingsConstructor : MonoBehaviour
{
    private float h_gain;
    private float h_gamma;
    private bool h_toggleBloom;
    private int h_resolutionDropDown;
    private bool h_windowedModeOn;

    public SettingsConstructor(float gain, float gamma, bool toggleBloom, int resolutionDropDown, bool windowedModeOn)
    {
        ///Video Settings
        h_gain = gain;
        h_gamma = gamma;
        h_toggleBloom = toggleBloom;
        h_windowedModeOn = windowedModeOn;
    }
}
