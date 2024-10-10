using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for the Stats Image
/// </summary>
public class StatsUIController : MonoBehaviour
{

    [Header("Energy Stuff")]

    private Image energyBar;

    //Speed to fill the bar at.
    public float fillSpeed;


    // Start is called before the first frame update
    void Start()
    {
        try
        {
            energyBar = transform.Find("Energy").GetComponent<Image>();
        }
        catch
        {
            Debug.LogWarning("Can't find EneryBar");
        }
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Initialize()
    {
        fillSpeed = 0.5f;
    }

    /// <summary>
    /// Call this to change the Energy Bar UI. this method accepts both UI and Float no need to change before.
    /// </summary>
    /// <param name="currentEnergy"></param>
    /// <param name="maxEnergy"></param>
    public void UpdateUIEnergyBar(float currentEnergy,float maxEnergy)
    {
            
        if (energyBar != null)
        {
            // Normalize the energy value to a 0-1 range
            float tempTargetFillAmount = currentEnergy / maxEnergy;

            //Clap at 0 and 1 so don't go over
            tempTargetFillAmount = Mathf.Clamp01(tempTargetFillAmount);

            StopAllCoroutines();
            
            StartCoroutine(FillEnergyOverTime(tempTargetFillAmount));
        }
        else
        {
            Debug.LogError("Energy Bar Image component not found.");
        }
    }

    /// <summary>
    /// Call this to change the Energy Bar UI. this method accepts both UI and Float no need to change before.
    /// </summary>
    /// <param name="currentEnergy"></param>
    /// <param name="maxEnergy"></param>
    public void UpdateUIEnergyBar(int currentEnergy, int maxEnergy)
    {

        if (energyBar != null)
        {
            // Convert int to float for accurate division
            float tempCurrentEnergy = (float)currentEnergy;
            float tempMaxEnergy = (float)maxEnergy;

            // Normalize the energy value to a 0-1 range
            float tempTargetFillAmount = tempCurrentEnergy / tempMaxEnergy;

            // Clamp the value between 0 and 1 to avoid overfilling
            tempTargetFillAmount = Mathf.Clamp01(tempTargetFillAmount);

            StopAllCoroutines();

            // Start the coroutine to smoothly fill the bar to the target amount
            StartCoroutine(FillEnergyOverTime(tempTargetFillAmount));
        }
        else
        {
            Debug.LogError("Energy Bar Image component not found.");
        }
    }

    /// <summary>
    /// Fill EnergyBar by amount over time.
    /// </summary>
    /// <param name="targetFillAmount"></param>
    /// <returns></returns>
    private IEnumerator FillEnergyOverTime(float targetFillAmount)
    {

        // While the bar is not at the target fill amount, update it
        while (!Mathf.Approximately(energyBar.fillAmount, targetFillAmount))
        {
            // Lerp between current fill and target fill by the fill speed
            energyBar.fillAmount = Mathf.Lerp(energyBar.fillAmount, targetFillAmount, fillSpeed * Time.deltaTime);

            // Ensure the fill value gradually updates each frame
            yield return null;
        }

        // Ensure it snaps to the exact target amount at the end
        energyBar.fillAmount = targetFillAmount;
        
    }

}
