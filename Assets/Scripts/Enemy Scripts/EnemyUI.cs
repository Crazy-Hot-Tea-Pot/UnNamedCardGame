using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    /// <summary>
    /// Name of Enemy Goes here.
    /// </summary>
    public TextMeshProUGUI EnemyNameBox;

    /// <summary>
    /// Enemy HealthBar Bar.
    /// </summary>
    public Image healthBar;

    /// <summary>
    /// Enemy HealthBar Text.
    /// </summary>
    public TextMeshProUGUI healthText;

    /// <summary>
    /// Enemy ShieldBar Container.
    /// </summary>
    public GameObject shieldContainer;

    /// <summary>
    /// Enemy sheild bar.
    /// </summary>
    public Image shieldBar;

    /// <summary>
    /// Enemy shield text.
    /// </summary>
    public TextMeshProUGUI shieldText;

    // Adjust this for smoother or quicker transitions
    public float UiDuration = 0.5f;

    /// <summary>
    /// reference to effects Panel.
    /// </summary>
    public GameObject EffectsPanel;

    /// <summary>
    /// Prefabs of Effects enemy will use."Case sensitive"
    /// </summary>
    public List<GameObject> effectPrefabs;

    /// <summary>
    /// list of active effects.
    /// </summary>
    public List<GameObject> activeEffects;

    /// <summary>
    /// Intent text box.
    /// </summary>
    public GameObject IntentText;

    private Camera playerCamera;

    void Awake()
    {
        playerCamera = Camera.main;    
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {        
        FaceCamera();
    }

    /// <summary>
    /// Displays EnemyName
    /// </summary>
    /// <param name="enemyName"></param>
    public void SetEnemyName(string enemyName)
    {
        EnemyNameBox.SetText(enemyName);
    }

    /// <summary>
    /// Update Enemy HP Bar
    /// </summary>
    /// <param name="currentHp"></param>
    /// <param name="maxHp"></param>
    public void UpdateHealth(int currentHp, int maxHp)
    {
        float targetHealthPercentage = (float)currentHp / maxHp;

        // Stop any currently running health update coroutine
        StopCoroutine(UpdateHealthOverTime(targetHealthPercentage));

        // Start the coroutine to smoothly update the health bar
        StartCoroutine(UpdateHealthOverTime(targetHealthPercentage));
    }

    /// <summary>
    /// Update Enemy ShieldBar Bar
    /// </summary>
    public void UpdateShield(int currentShield, int maxShield)
    {       

        if (currentShield == 0 && maxShield == 0)
        {
            shieldContainer.SetActive(false);
        }
        else
        {
            shieldContainer.SetActive(true);               

            // Calculate the target shield percentage
            float shieldPercentage = (float)currentShield / maxShield;

            //ShieldBar.fillAmount = shieldPercentage;

            StopCoroutine(UpdateShieldOverTime(shieldPercentage, maxShield));

            // Start the coroutine to smoothly update the shield bar
            StartCoroutine(UpdateShieldOverTime(shieldPercentage, maxShield));
        }
    }

    /// <summary>
    /// Check if the effect is already active.
    /// Method to instantiate and add effect to EffectsPanel.
    /// Instantiate effect icon and set its parent to EffectsPanel.
    /// Track active effects.
    /// </summary>
    /// <param name="effectPrefab">Name of effect case sensitive</param>
    public void AddEffect(string effectName)
    {
        if (activeEffects.Exists(effect => effect.name == effectName))
            return;

        GameObject effectPrefab = effectPrefabs.Find(prefab => prefab.name == effectName);

        try
        {
            GameObject effectInstance = Instantiate(effectPrefab, EffectsPanel.transform);
            effectInstance.name = effectName;
            activeEffects.Add(effectInstance);
        }
        catch
        {
            Debug.LogError("So Somebody fucked up.");
        }
    }

    /// <summary>
    /// Method to remove an effect from EffectsPanel.
    /// Remove from tracking list.
    /// Destroy the effect GameObject.
    /// </summary>
    /// <param name="effect"></param>
    public void RemoveEffect(string effectName)
    {
        GameObject effectToRemove = activeEffects.Find(effect => effect.name == effectName);
        try
        {
            if (effectToRemove != null)
            {
                activeEffects.Remove(effectToRemove);
                Destroy(effectToRemove);
            }
        }
        catch
        {
            Debug.LogError("So Somebody fucked up.");
        }
    }

    /// <summary>
    /// Update intent box.
    /// </summary>
    /// <param name="intent"></param>
    /// <param name="colorOfIntent"></param>
    public void UpdateIntent(string intent,Color colorOfIntent)
    {
        IntentText.SetActive(true);
        IntentText.GetComponent<TextMeshProUGUI>().SetText(intent);
        IntentText.GetComponent<TextMeshProUGUI>().color = colorOfIntent;
    }

    /// <summary>
    /// Make the canvas face the player camera
    /// </summary>
    private void FaceCamera()
    {
        if (playerCamera != null)
        {
            Vector3 direction = (playerCamera.transform.position - this.transform.position).normalized;
            direction.y = 0;
            this.transform.rotation = Quaternion.LookRotation(-direction);
        }
    }

    private IEnumerator UpdateHealthOverTime(float targetFillAmount)
    {        

        float initialFillAmount = healthBar.fillAmount;
        float elapsedTime = 0f;       

        while (elapsedTime < UiDuration)
        {
            elapsedTime += Time.deltaTime;
            float newFillAmount = Mathf.Lerp(initialFillAmount, targetFillAmount, elapsedTime / UiDuration);
            healthBar.fillAmount = newFillAmount;

            // Update health text as percentage
            int currentHealthPercentage = Mathf.RoundToInt(newFillAmount * 100);
            healthText.SetText($"{currentHealthPercentage}%");

            yield return null;
        }

        // Ensure it snaps to the final target amount
        healthBar.fillAmount = targetFillAmount;
        healthText.SetText($"{Mathf.RoundToInt(targetFillAmount * 100)}%");
    }

    private IEnumerator UpdateShieldOverTime(float targetFillAmount,float maxShield)
    {
        float initialFillAmount = shieldBar.fillAmount;
        float elapsedTime = 0f;

        // Initial shield amount
        int initialCurrentShield = Mathf.RoundToInt(initialFillAmount * maxShield);

        // Target shield amount
        int targetCurrentShield = Mathf.RoundToInt(targetFillAmount * maxShield);

        // Store the initial max shield value
        int initialMaxShield = (int)maxShield;

        // Target max shield value
        int finalMaxShield = Mathf.RoundToInt(targetFillAmount * maxShield);

        while (elapsedTime < UiDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / UiDuration;

            // Lerp shield bar fill amount
            float newFillAmount = Mathf.Lerp(initialFillAmount, targetFillAmount, progress);
            shieldBar.fillAmount = newFillAmount;

            // Dynamically calculate current and max shield values
            int currentShield = Mathf.RoundToInt(Mathf.Lerp(initialCurrentShield, targetCurrentShield, progress));
            int updatedMaxShield = Mathf.RoundToInt(Mathf.Lerp(initialMaxShield, finalMaxShield, progress));

            // Update the shield text
            shieldText.SetText($"{currentShield}/{updatedMaxShield}");

            yield return null;
        }

        // Ensure it snaps to the final values
        shieldBar.fillAmount = targetFillAmount;
        shieldText.SetText($"{targetCurrentShield}/{finalMaxShield}");
    }
}