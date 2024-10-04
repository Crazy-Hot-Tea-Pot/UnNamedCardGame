using UnityEngine;

/// <summary>
/// The whole game setting will be stored here to access across the game.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private CameraSettings cameraSettings;

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
        // Initialize other settings like AudioSettings, GameplaySettings, etc.
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
