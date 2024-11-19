using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// The whole game setting will be stored here to access across the game.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private CameraSettings cameraSettings;

    [SerializeField]
    private SoundSettings soundSettings;

    [SerializeField]
    private DataSettings dataSettings;

    [SerializeField]
    private VideoSettings videoSettings;

    public static SettingsManager Instance {
        get; 
        private set; }
    
    public CameraSettings CameraSettings
    {
        get
        {
            return cameraSettings;
        }
        private set
        {
            cameraSettings = value;
        }
    }

    public SoundSettings SoundSettings
    {
        get
        {
            return soundSettings;
        }
        set
        {
            soundSettings = value;
        }
    }

    public DataSettings DataSettings
    {
        get
        {
            return dataSettings;
        }
        set
        {
            dataSettings = value;
        }
    }
    public VideoSettings VideoSettings
    {
        get
        {
            return videoSettings;
        }
        private set
        {
            videoSettings = value;
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
        videoSettings = new VideoSettings();
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
