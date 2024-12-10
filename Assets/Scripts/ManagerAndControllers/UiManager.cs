using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public PlayerInputActions playerInputActions;

    /// <summary>
    /// Current Ui being displayed.
    /// </summary>
    [SerializeField]
    public GameObject CurrentUI
    {
        get;
        set;
    }
    /// <summary>
    /// List of Prefabs UI.
    /// </summary>
    public List<GameObject> listOfUis;
    public GameObject RoamingAndCombatUI;
    public GameObject InventoryUI;
    public GameObject TerminalUI;

    private UiController currentController;
    private static UiManager instance;

    void OnEnable()
    {
        playerInputActions.Player.Inventory.performed += ToggleInventory; 
    }
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

        // assign player Input class
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnStartCombat += StartCombat;
        GameManager.Instance.OnEndCombat += EndCombat;
        GameManager.Instance.OnSceneChange += SceneChange;
        GameManager.Instance.OnGameModeChanged += UpdateUIForGameMode;

        UpdateUIForGameMode();

    }
    #region RoamingAndCombatUI
    public void UpdateCameraIndicator(CameraController.CameraState cameraState)
    {
        //Activate UI Obejct        
        GetCurrentController<RoamingAndCombatUiController>().CameraIndicator.SetActive(true);

        GetCurrentController<RoamingAndCombatUiController>().CameraModeIndicatorController.SwitchIndicatorTo(cameraState);
    }
    public void UpdateHealth(int currentHealth, int MaxHealth)
    {
        GetCurrentController<RoamingAndCombatUiController>().UpdateHealth(currentHealth, MaxHealth);
    }
    public void UpdateShield(int currentShield, int MaxShield)
    {
        GetCurrentController<RoamingAndCombatUiController>().UpdateShield(currentShield, MaxShield);
    }
    public void UpdateEnergy(int currentEnergy, int MaxEnergy)
    {
        GetCurrentController<RoamingAndCombatUiController>().UpdateEnergy(currentEnergy, MaxEnergy);
    }
    public void EndTurnButtonVisibility(bool Visiable)
    {
        GetCurrentController<RoamingAndCombatUiController>().EndTurn.SetActive( Visiable );
    }
    public void EndTurnButtonInteractable(bool Interact)
    {
        GetCurrentController<RoamingAndCombatUiController>().EndTurnButton.interactable = Interact;
    }
    #endregion
    #region InventoryUI
    public void ToggleInventory(InputAction.CallbackContext context)
    {
        if (context.performed && GameManager.Instance.CurrentGameMode == GameManager.GameMode.Roaming)
        {
            if (CurrentUI.name ==InventoryUI.name )
            {
                SwitchScreen(RoamingAndCombatUI);
            }
            else
            {
                SwitchScreen(InventoryUI);
            }
        }
    }
    public void RefreshInventory()
    {
        GetCurrentController<InventoryController>().RefreshCurrentInventory();
    }
    #endregion
    #region TerminalUI
    public void UpdateScrapDisplay(int playerScrap)
    {
        GetCurrentController<UpgradeTerminalUIController>().UpdateScrapDisplay(playerScrap);
    }
    public string GetUserInput()
    {
        return GetCurrentController<UpgradeTerminalUIController>().UserInput.GetComponent<TMP_InputField>().text;
    }
    public void SetScrapDisplay(bool state)
    {
        GetCurrentController<UpgradeTerminalUIController>().ScrapPanel.SetActive( state );
    }
    public void FillData()
    {
        GetCurrentController<UpgradeTerminalUIController>().FillData();
    }
    #endregion
    private T GetCurrentController<T>() where T : UiController
    {
        T controller = currentController as T;
        if (controller == null)
        {
            Debug.LogWarning($"[UiManager] Current controller is not of type {typeof(T)}.");
        }
        return controller;
    }
    private void UpdateUIForGameMode()
    {       

        // Update UI elements based on the game mode
        switch (GameManager.Instance.CurrentGameMode)
        {
            case GameManager.GameMode.Title:
            case GameManager.GameMode.Loading:
            case GameManager.GameMode.Settings:
            case GameManager.GameMode.Credits:
                break;
            case GameManager.GameMode.Combat:
            case GameManager.GameMode.Roaming:
                SwitchScreen(listOfUis.Find(ui => ui.name == "Roaming And Combat UI"));
                break;
            case GameManager.GameMode.Pause:
                Debug.Log("[UiManager] Displaying Pause Menu.");
                break;
            case GameManager.GameMode.Interacting:
                SwitchScreen(listOfUis.Find(ui => ui.name == "Terminal UI"));
                break;
            default:
                Debug.Log("[UiManager] Hiding all UI.");
                break;
        }
    }
    private void SwitchScreen(GameObject targetScreen)
    {
        if (listOfUis.Count == 0)
        {
            Debug.LogError("Ui list is empty!");
            return;
        }
        if (targetScreen == null)
        {
            Debug.LogError($"[UiManager] No UI prefab found for mode: {GameManager.Instance.CurrentGameMode}");
            return;
        }
        // Destroy current UI if it exists
        if (CurrentUI != null)
        {
            Destroy(CurrentUI);
        }
        // Instantiate the target UI prefab under this object's transform (Canvas)
        CurrentUI = Instantiate(targetScreen, transform);

        CurrentUI.name=targetScreen.name;

        currentController = CurrentUI.GetComponent<UiController>();
        currentController?.Initialize();

        // Optionally reset the local position, rotation, and scale
        CurrentUI.transform.localPosition = Vector3.zero;
        CurrentUI.transform.localRotation = Quaternion.identity;
        CurrentUI.transform.localScale = Vector3.one;
    }
    private void StartCombat()
    {
    }

    private void EndCombat()
    {
    }

    private void SceneChange(Levels newLevel)
    {
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
    void OnDisable()
    {
        playerInputActions.Player.Inventory.performed -= ToggleInventory;
    }
}
