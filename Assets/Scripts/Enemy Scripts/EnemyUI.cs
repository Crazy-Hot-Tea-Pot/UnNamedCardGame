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
    /// Enemy Health Bar.
    /// </summary>
    public Image healthBar;
    /// <summary>
    /// Enemy Shield Container.
    /// </summary>
    public GameObject shieldContainer;
    /// <summary>
    /// Enemy sheild bar.
    /// </summary>
    public Image shieldBar;
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
    /// Update Enemy Shield Bar
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

            // Reset the ShieldBar fill amount to 0 if it's being activated for the first time
            if (shieldBar.fillAmount == 0)
                shieldBar.fillAmount = 0;

            // Calculate the target shield percentage
            float shieldPercentage = (float)currentShield / maxShield;

            //ShieldBar.fillAmount = shieldPercentage;

            StopCoroutine(UpdateShieldOverTime(shieldPercentage));

            // Start the coroutine to smoothly update the shield bar
            StartCoroutine(UpdateShieldOverTime(shieldPercentage));
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
        // While the bar is not at the target fill amount, update it
        while (Mathf.Abs(healthBar.fillAmount - targetFillAmount) > 0.001f)
        {
            // Lerp between the current fill and the target fill amount
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFillAmount, 0.5f * Time.deltaTime);

            // Optionally update health bar color or other visual cues here

            // Ensure the fill value gradually updates each frame
            yield return null;
        }

        // Ensure it snaps to the exact target amount at the end
        healthBar.fillAmount = targetFillAmount;
    }

    private IEnumerator UpdateShieldOverTime(float targetFillAmount)
    {
        // While the difference between the current fill amount and the target is greater than the threshold
        while (Mathf.Abs(shieldBar.fillAmount - targetFillAmount) > 0.001f)
        {
            // Lerp between current fill and target fill by the fill speed
            shieldBar.fillAmount = Mathf.Lerp(shieldBar.fillAmount, targetFillAmount, 0.5f * Time.deltaTime);

            // Display percentage as an integer (0 to 100)
            //int percentage = Mathf.RoundToInt(shieldBar.fillAmount * 100);
            //shiedText.SetText(percentage + "%");


            // Ensure the fill value gradually updates each frame
            yield return null;
        }

        // Ensure it snaps to the exact target amount at the end
        shieldBar.fillAmount = targetFillAmount;

        // Display percentage as an integer (0 to 100)
        //int finalPercentage = Mathf.RoundToInt(targetFillAmount * 100);
        //shiedText.SetText(finalPercentage + "%");


        if (shieldBar.fillAmount <= 0f)
            shieldContainer.SetActive(false);
    }
}
