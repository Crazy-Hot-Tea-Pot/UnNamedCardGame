using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static SettingsData;

[System.Serializable]
public class VideoSettings
{
    // Screen resolution
    public int Resolution
    {
        get
        {
            return resolution;
        }
        private set
        {
            resolution = value;
        }
    }

    /// <summary>
    /// Fullscreen, Windowed, Borderless
    /// </summary>
    public bool ScreenMode
    {
        get
        {
            return screenMode;
        }
        private set
        {
            screenMode = value;            
        }
    }  

    /// <summary>
    /// variable for Gain
    /// </summary>
    public Vector4 CurrentGain
    {
        get
        {
            return currentGain;
        }
        private set
        {
            currentGain = value;
        }
    }

    /// <summary>
    /// Variable for gamma
    /// </summary>
    public Vector4 CurrentGamma
    {
        get
        {
            return currentGamma;
        }
        private set
        {
            currentGamma = value;
        }
    }

    /// <summary>
    /// If Bloom is enabled.
    /// </summary>
    public bool BloomEnabled
    {
        get
        {
            return bloom;
        }
        private set
        {
            bloom = value;
        }
    }

    [SerializeField]
    private int resolution;
    [SerializeField]
    private bool screenMode;
    [SerializeField]
    private Vector4 currentGain;
    [SerializeField]
    private Vector4 currentGamma;
    [SerializeField]
    private bool bloom;

    /// <summary>
    /// Default contructor for video settings.
    /// Have to apply the volumes in this constructor.
    /// </summary>
    public VideoSettings(WriteVideoFileData VideoFileData)
    {
        //Added this so you would know to load default values or load what the data has.
        if (VideoFileData.SettingsEdited)
        {
            //Load values from VideoFileData
        }
        else
        {
            //set default values
        }
        //ApplySettingsFromSave(VideoFileData);
    }

    /// <summary>
    /// What happens when we start (This is really a copy of SettingsUIController apply method without UI stuff)
    /// </summary>
    /// <param name="videoFileData"></param>
    public void ApplySettingsFromSave(WriteVideoFileData videoFileData)
    {
        foreach (VolumeProfile levelProfile in SettingsManager.Instance.VolumeSettings)
        {
            //Try to get the variable for gain
            if (levelProfile.TryGet(out LiftGammaGain gainSettings))
            {
                //Save the gain and gamma
                //THIS I WROTE METHODS TO RETURN GAIN AND GAMMA AS VECTOR4
                //SettingsManager.Instance.VideoSettings.SetandSaveGainandGamma(gainSettings, videoFileData.Gain, videoFileData.Gamma);
            }
            //If this value doesn't exist
            else
            {
                Debug.Log("There is no gain");
            }

            //Enable and disable bloom check
            if (levelProfile.TryGet(out Bloom bloomSettings))
            {
                if (videoFileData.bloom)
                {
                    SettingsManager.Instance.VideoSettings.DisableBloom(bloomSettings);
                }
                else
                {
                    SettingsManager.Instance.VideoSettings.EnabledBloom(bloomSettings);
                }
                //bloomSettings.active = bloomOn.isOn;
            }
            //If there is no bloom
            else
            {
                Debug.Log("There is no bloom");
            }
        }

        //Apply Unity Settings
        //Check if windowed and assign the toggle to match
        if (videoFileData.windowedMode == false)
        {
            //Sets full screen
            SettingsManager.Instance.VideoSettings.IsFullScreen(true);
            //UnityEngine.Screen.fullScreen = true;
            Debug.Log("Full screen");
        }
        else
        {
            //Sets windowed
            SettingsManager.Instance.VideoSettings.IsFullScreen(false);
            //UnityEngine.Screen.fullScreen = false;
            Debug.Log("Windowed");
        }


        //Screen resolution
        SetandSaveResolution(videoFileData.resolution);

    }

    #region ProfileSettings

    /// <summary>
    /// Enabled bloom in graphics.
    /// </summary>
    /// <param name="CurrentBloom">Volume bloom to apply to</param>
    public void EnabledBloom(Bloom CurrentBloom)
    {
        //Enable on the volume profile
        CurrentBloom.active = true;
        //Enable the bloom bool
        BloomEnabled = true;
    }

    /// <summary>
    /// Disable bloom in graphics.
    /// </summary>
    ///<param name="CurrentBloom">Volume bloom to apply to</param>
    public void DisableBloom(Bloom CurrentBloom)
    {
        //Disable on the volume profile
        CurrentBloom.active = false;
        //Disable bloom bool
        BloomEnabled = false;
    }

    /// <summary>
    /// Saves the data for gain and gamma
    /// </summary>
    /// <param name="currentGainandGamma"></param>
    /// <param name="gammaSlider"></param>
    /// <param name="gainSlider"></param>
    public void SetandSaveGainandGamma(LiftGammaGain currentGainandGamma, float gammaSlider, float gainSlider)
    {
        //Set brightness to meet the new value. W represents the value of the intensity and we add +0.5f so it's usable as this value uses negative values but sliders don't.
        currentGainandGamma.gamma.value += new Vector4(0, 0, 0, gammaSlider - 0.5f);
        //Repeat the same process for gain
        currentGainandGamma.gain.value += new Vector4(0, 0, 0, gainSlider - 0.5f);

        //Actually save
        CurrentGain = currentGainandGamma.gain.value;
        CurrentGamma = currentGainandGamma.gamma.value;
    }

    /// <summary>
    /// Saves the data for full screen
    /// </summary>
    /// <param name="windowedModeOn"></param>
    public void IsFullScreen(bool windowedModeOn)
    {
        //Set windowed mode
        UnityEngine.Screen.fullScreen = windowedModeOn;

        //Actually save
        ScreenMode = windowedModeOn;
    }

    /// <summary>
    /// Saves the data for resolution
    /// </summary>
    /// <param name="resolution"></param>
    public void SetandSaveResolution(int resolutionTemp)
    {
        //Actually save
        Resolution = resolutionTemp;

        //Screen resolution
        if (resolution == 0)
        {
            UnityEngine.Screen.SetResolution(1920, 1080, ScreenMode);
            Debug.Log("1920x1080");
        }
        else if (resolution == 1)
        {
            UnityEngine.Screen.SetResolution(1366, 763, ScreenMode);
            Debug.Log("1366x763");
        }
        else if (resolution == 2)
        {

            UnityEngine.Screen.SetResolution(2560, 1440, ScreenMode);
            Debug.Log("2560x1440");
        }
        else if (resolution == 3)
        {

            UnityEngine.Screen.SetResolution(3840, 2160, ScreenMode);
            Debug.Log("3840x2160");
        }
    }

    /// <summary>
    /// Return all data in here for saving.
    /// </summary>
    /// <returns></returns>
    public WriteVideoFileData GetDataToWrite()
    {
        return new WriteVideoFileData
        {
            SettingsEdited = true,
            resolution = Resolution,
            gainX = CurrentGain.x,
            gainY = CurrentGain.y,
            gainZ = CurrentGain.z,
            gainW = CurrentGain.w,
            gammaX = CurrentGamma.x,
            gammaY = CurrentGamma.y,
            gammaZ = CurrentGamma.z,
            gammaW = CurrentGamma.w,
            bloom = BloomEnabled,
            windowedMode = ScreenMode
        };
    }

    #endregion
}