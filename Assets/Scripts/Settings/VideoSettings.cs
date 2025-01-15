using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class VideoSettings
{
    // Screen resolution
    public Vector2Int Resolution
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
    public FullScreenMode ScreenMode
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
    /// Gain
    /// </summary>
    public float Gain
    {
        get
        {
            return gain;
        }
        private set
        {
            gain = value;
        }
    }

    /// <summary>
    /// Gamma
    /// </summary>
    public float Gamma
    {
        get
        {
            return gamma;
        }
        set
        {
            gamma = value;
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

    private Vector2Int resolution;
    private FullScreenMode screenMode;
    private float gain;
    private float gamma;
    private bool bloom;

    /// <summary>
    /// Default contructor for video settings.
    /// Have to apply the volumes in this constructor.
    /// </summary>
    public VideoSettings()
    {

    }
    #region Bloom    

    /// <summary>
    /// Enabled bloom in graphics.
    /// TODO Add code to enable bloom.
    /// </summary>
    /// <param name="CurrentVolume">Volume to apply to</param>
    public void EnabledBloom(VolumeProfile CurrentVolume)
    {
        //add code here

        BloomEnabled = true;
    }

    /// <summary>
    /// Disable bloom in graphics.
    /// TODO add code to disable bloom.
    /// </summary>
    ///<param name="CurrentVolume">Volume to apply to</param>
    public void DisableBloom(VolumeProfile CurrentVolume)
    {
        //add code here

        BloomEnabled = false;
    }

    #endregion
}