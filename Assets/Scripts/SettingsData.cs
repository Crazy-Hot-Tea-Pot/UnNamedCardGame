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
        public int resolution;

        //Create setting for vector4
        //public Vector4 gain;
        //public Vector4 gamma;

        public bool bloom;

        public bool windowedMode;
    }

    public WriteVideoFileData VidoeData;

    public SettingsData()
    {
        VidoeData = new WriteVideoFileData();

    }
}
