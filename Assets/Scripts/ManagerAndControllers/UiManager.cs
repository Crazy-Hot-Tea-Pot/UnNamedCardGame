using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }


    public List<GameObject> listOfUis;

    private static UiManager instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnStartCombat += StartCombat;
        GameManager.Instance.OnEndCombat += EndCombat;
        GameManager.Instance.OnSceneChange += SceneChange;
        GameManager.Instance.OnGameModeChanged += UpdateUIForGameMode;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void UpdateUIForGameMode()
    {
        // Update UI elements based on the game mode
        switch (GameManager.Instance.CurrentGameMode)
        {
            case GameManager.GameMode.Title:
                Debug.Log("[UiManager] Displaying Title Screen UI.");
                break;
            case GameManager.GameMode.Combat:
                Debug.Log("[UiManager] Displaying Combat UI.");
                break;
            case GameManager.GameMode.Roaming:
                Debug.Log("[UiManager] Displaying Roaming UI.");
                break;
            case GameManager.GameMode.Pause:
                Debug.Log("[UiManager] Displaying Pause Menu.");
                break;
            case GameManager.GameMode.Settings:
                Debug.Log("[UiManager] Displaying Settings Screen.");
                break;
            case GameManager.GameMode.Credits:
                Debug.Log("[UiManager] Displaying Credits Screen.");
                break;
            default:
                Debug.Log("[UiManager] Hiding all UI.");
                break;
        }
    }
    private void StartCombat()
    {
        Debug.Log("[ManagerName] Combat started.");
    }

    private void EndCombat()
    {
        Debug.Log("[ManagerName] Combat ended.");
    }

    private void SceneChange(Levels newLevel)
    {
        Debug.Log($"[ManagerName] Scene changed to {newLevel}.");
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStartCombat -= StartCombat;
            GameManager.Instance.OnEndCombat -= EndCombat;
            GameManager.Instance.OnSceneChange -= SceneChange;
            GameManager.Instance.OnGameModeChanged -= UpdateUIForGameMode;
        }
    }
}
