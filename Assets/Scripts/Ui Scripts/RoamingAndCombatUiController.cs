using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoamingAndCombatUiController : UiController
{
    public float SpeedOfFill = 1f;

    [Header("Camera UI")]
    public GameObject CameraIndicator;
    public CameraModeIndicatorController CameraModeIndicatorController;

    [Header("Health")]
    public Image HealthBar;
    public TextMeshProUGUI HealthText;
    // Green (395E44 in RGB normalized)
    public Color fullHealthColor = new Color(0.23f, 0.37f, 0.27f);
    public Color lowHealthColor = Color.red;

    [Header("Shield")]
    public GameObject ShieldContainer;
    public Image ShieldBar;
    public TextMeshProUGUI ShieldText;

    [Header("Energy")]
    public Image EnergyBar;

    [Header("Gear")]
    public Gear Armor;
    public Gear Weapon;
    public Gear Equipment;

    [Header("Combat Mode Stuff")]
    public GameObject PlayerHandContainer;
    public GameObject EnergyAndGearContainer;
    public GameObject EndTurn;
    public Button EndTurnButton;

    // Start is called before the first frame update
    void Start()
    {
        EndTurnButton.onClick.AddListener(() => GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>().EndTurn(GameObject.FindGameObjectWithTag("Player")));
        
        EndTurn.SetActive(false);
        PlayerHandContainer.SetActive(false);
        CameraIndicator.SetActive(false);

        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (player == null)
        {
            UpdateHealth(player.Health, player.MaxHealth);
            UpdateShield(player.Shield, player.MaxShield);
            UpdateEnergy(player.Energy, player.MaxEnergy);
        }

        Armor.EquipItem(GearManager.Instance.GetEquippedItem(Item.ItemType.Armor));
        Weapon.EquipItem(GearManager.Instance.GetEquippedItem(Item.ItemType.Weapon));
        Equipment.EquipItem(GearManager.Instance.GetEquippedItem(Item.ItemType.Equipment));
    }

    public override void Initialize()
    {
        Debug.Log("RoamingAndCombatUiController initialized");      
    }

    /// <summary>
    /// Close hand and reopen with new cards or just draw hand with cards.
    /// </summary>
    public IEnumerator RedrawPlayerHand()
    {
        if(PlayerHandContainer.GetComponent<PlayerHandContainer>().PanelIsVisible)
            PlayerHandContainer.GetComponent<PlayerHandContainer>().TogglePanel();

        yield return new WaitForSeconds(1f);

        ChipManager.Instance.RefreshPlayerHand();

        yield return new WaitForSeconds(1f);

        if (!PlayerHandContainer.GetComponent<PlayerHandContainer>().PanelIsVisible && ChipManager.Instance.PlayerHand.Count != 0)
            PlayerHandContainer.GetComponent<PlayerHandContainer>().TogglePanel();
    }

    public void ChangeEndButtonVisibility(bool visibility)
    {
        EndTurn.SetActive(visibility);
        Debug.Log("Setting Button to " + visibility);
    }

    /// <summary>
    /// Updates the UI for Player HealthBar
    /// </summary>
    public void UpdateHealth(int currentHealth, int maxHealth)
    {        

        float targetHealthPercentage = (float)currentHealth / maxHealth;

        StopCoroutine(UpdateHealthOverTime(targetHealthPercentage));

        // Start the coroutine to smoothly update the health bar
        StartCoroutine(UpdateHealthOverTime(targetHealthPercentage));
    }   
    
    /// <summary>
    /// Updates the UI for Player ShieldAmount
    /// </summary>
    public void UpdateShield(int Shield, int MaxShield)
    {        

        if (Shield == 0 && MaxShield == 100)
        {
            ShieldContainer.SetActive(false);
        }
        else
        {
            ShieldContainer.SetActive(true);

            // Calculate the target ShieldAmount percentage
            float shieldPercentage = (float)Shield / (float)MaxShield;

            StopCoroutine(UpdateShieldOverTime(shieldPercentage,Shield,MaxShield));

            // Start the coroutine to smoothly update the ShieldAmount bar
            StartCoroutine(UpdateShieldOverTime(shieldPercentage, Shield, MaxShield));
        }
    }

    /// <summary>
    /// Updates the UI for Player Energy
    /// </summary>
    /// <param name="currentEnergy"></param>
    /// <param name="maxEnergy"></param>
    public void UpdateEnergy(int currentEnergy, int maxEnergy)
    {
        // Normalize the energy value to a 0-1 range
        float tempTargetFillAmount = (float)currentEnergy / maxEnergy;


        StopCoroutine(FillEnergyOverTime(tempTargetFillAmount));

        StartCoroutine(FillEnergyOverTime(tempTargetFillAmount));
    }

    /// <summary>
    /// switch UI Modes
    /// </summary>
    /// <param name="CombatMode"></param>
    public void SwitchMode(bool CombatMode)
    {
        PlayerHandContainer.SetActive(CombatMode);
        EnergyAndGearContainer.GetComponent<Animator>().SetBool("Visible", CombatMode);
    }

    private IEnumerator UpdateHealthOverTime(float targetFillAmount)
    {
        // While the bar is not at the target fill amount, update it
        while (Mathf.Abs(HealthBar.fillAmount - targetFillAmount) > 0.001f)
        {
            // Lerp between current fill and target fill by the fill speed
            HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, targetFillAmount, SpeedOfFill * Time.deltaTime);

            // Lerp the color based on the health percentage
            HealthBar.color = Color.Lerp(lowHealthColor, fullHealthColor, HealthBar.fillAmount);

            // Display percentage as an integer (0 to 100)
            int percentage = Mathf.RoundToInt(HealthBar.fillAmount * 100);
            HealthText.SetText(percentage + "%");

            // Ensure the fill value gradually updates each frame
            yield return null;
        }

        // Ensure it snaps to the exact target amount at the end
        HealthBar.fillAmount = targetFillAmount;
        HealthBar.color = Color.Lerp(lowHealthColor, fullHealthColor, HealthBar.fillAmount);

        // Display percentage as an integer (0 to 100)
        int finalPercentage = Mathf.RoundToInt(targetFillAmount * 100);
        HealthText.SetText(finalPercentage + "%");
    }

    private IEnumerator UpdateShieldOverTime(float targetFillAmount, int Shield, int MaxShield)
    {      

        int initialCurrentShield = Mathf.RoundToInt(ShieldBar.fillAmount * MaxShield);
        int targetCurrentShield = Shield;

        int initialMaxShield = MaxShield;

        float elapsedTime = 0f;

        while (elapsedTime < SpeedOfFill)
        {
            elapsedTime += Time.deltaTime;

            // Lerp the ShieldAmount bar fill amount
            float newFillAmount = Mathf.Lerp(ShieldBar.fillAmount, targetFillAmount, elapsedTime / SpeedOfFill);
            ShieldBar.fillAmount = newFillAmount;

            // Dynamically calculate ShieldAmount values
            int currentShield = Mathf.RoundToInt(Mathf.Lerp(initialCurrentShield, targetCurrentShield, elapsedTime / SpeedOfFill));
            int maxShield = Mathf.RoundToInt(Mathf.Lerp(initialMaxShield, MaxShield, elapsedTime / SpeedOfFill));

            // Update the ShieldAmount text
            ShieldText.SetText($"{currentShield}/{maxShield}");

            yield return null;
        }

        // Snap to final values
        ShieldBar.fillAmount = targetFillAmount;
        ShieldText.SetText($"{targetCurrentShield}/{MaxShield}");
    }

    /// <summary>
    /// Fill EnergyBar by amount over time.
    /// </summary>
    /// <param name="targetFillAmount"></param>
    /// <returns></returns>
    private IEnumerator FillEnergyOverTime(float targetFillAmount)
    {

        // While the bar is not at the target fill amount, update it
        while (Mathf.Abs(EnergyBar.fillAmount - targetFillAmount) > 0.001f)
        {
            // Lerp between current fill and target fill by the fill speed
            EnergyBar.fillAmount = Mathf.Lerp(EnergyBar.fillAmount, targetFillAmount, SpeedOfFill * Time.deltaTime);            
           

            // Ensure the fill value gradually updates each frame
            yield return null;
        }

        // Ensure it snaps to the exact target amount at the end
        EnergyBar.fillAmount = targetFillAmount;
    }

    void OnDestroy()
    {
        EndTurnButton.onClick.RemoveAllListeners();
    }
}