using UnityEngine;

/// <summary>
/// The whole game setting will be stored here to access across the game.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    private CameraSettings cameraSettings;

    private SoundSettings soundSettings;

    private DataSettings dataSettings;

    public static SettingsManager Instance {
        get; 
        private set; }

    //[Header("Settings")]
    public CameraSettings CameraSettings
    {
        get
        {
            return cameraSettings;
        }
        //private set;
    }
    public SoundSettings SoundSettings
    {
        get
        {
            return soundSettings;
        }
    }

    public DataSettings DataSettings
    {
        get
        {
            return dataSettings;
        }
    }

    void Awake()
    {
        // Check if another instance of the SettingsManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep this object between scenes
            InitializeSettings();
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicates
        }
    }
    /// <summary>
    /// Initialize All settings
    /// </summary>
    void InitializeSettings()
    {
        cameraSettings = new CameraSettings();
        soundSettings = new SoundSettings();
        dataSettings = new DataSettings();
    }

    /// <summary>
    /// Load all settings
    /// </summary>
    public void LoadSettings()
    {

    }
    /// <summary>
    /// Save all settings
    /// </summary>
    public void SaveSettings()
    {

    }

}
