using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A constructor for the settings menu in the ui for saving it's various components
/// </summary>
[System.Serializable]
public class SettingsData
{
    [System.Serializable]
    public class WriteVideoFileData
    {
        public string test;
    }

    public WriteVideoFileData VidoeData;

    public SettingsData()
    {
        VidoeData = new WriteVideoFileData();
    }
}
