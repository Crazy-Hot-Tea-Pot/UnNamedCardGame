using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// The whole game setting will be stored here to access across the game.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance
    {
        get;
        private set;
    }

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

    public SettingsData CurrentSettingsData
    {
        get
        {
            return currentSettingsData;
        }
        set
        {
            currentSettingsData = value;
        }
    }

    [Header("Settings")]
    [SerializeField]
    private CameraSettings cameraSettings;

    [SerializeField]
    private SoundSettings soundSettings;

    [SerializeField]
    private DataSettings dataSettings;

    [SerializeField]
    private VideoSettings videoSettings;

    [SerializeField]
    private string saveDirectory;

    [SerializeField]
    private SettingsData currentSettingsData;

    void Awake()
    {
        // Check if another instance of the SettingsManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep this object between scenes

            //Save Directory
            saveDirectory = Path.Combine(Application.dataPath, "Settings");

            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            //Initialize all Settings
            InitializeSettings();
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicates
        }

        CurrentSettingsData = new SettingsData();
    }

    void Start()
    {


        GameManager.Instance.OnSceneChange += SceneChange;

        CurrentSettingsData.VidoeData.test = "video settins saved";
    }

    /// <summary>
    /// Initialize All settings by calling the default constructor.
    /// </summary>
    void InitializeSettings()
    {
        LoadSettings();

        cameraSettings = new CameraSettings();
        soundSettings = new SoundSettings();
        dataSettings = new DataSettings();

        //TODO change this to call the constructor if dataManager has settings.
        videoSettings = new VideoSettings();
    }

    /// <summary>
    /// Load all settings
    /// </summary>
    public void LoadSettings()
    {
        string saveFilePath = Path.Combine(saveDirectory, $"Settings.json");

        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning($"Save file not found: Settings");
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        SettingsData loadedData = JsonUtility.FromJson<SettingsData>(json);
    }
    /// <summary>
    /// Save all settings
    /// </summary>
    public void SaveSettings()
    {
        string saveFilePath = Path.Combine(saveDirectory, $"Settings.json");

        string json = JsonUtility.ToJson(CurrentSettingsData, true);

        File.WriteAllText(saveFilePath, json);

        Debug.Log($"Settings saved successfully: {saveFilePath}");
    }

    private void SceneChange(Levels newLevel)
    {
        switch (newLevel)
        {
            case Levels.Title:

                break;
        }
    }

    void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSceneChange -= SceneChange;
        }

        SaveSettings();
    }
}
