using System.Collections;
using UnityEngine;

/// <summary>
/// This allows us to have more control over the BG audio
/// </summary>
public class SoundBGVolume : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Lower background volume over time to amount given.
    /// </summary>
    /// <param name="duration">How fast to lower volume.</param>
    /// <param name="amount">The amount to lower to.</param>
    public void LowerVolume(float duration, float amount)
    {
        StartCoroutine(LowerVolumeOverTime(duration, amount));
    }
    /// <summary>
    /// Raises the background volume overtime by the amount given.
    /// </summary>
    /// <param name="duration"></param>
    public void RaiseVolume(float duration)
    {
        StartCoroutine(RaiseVolumeOverTime(duration));
    }

    private IEnumerator LowerVolumeOverTime(float duration, float amount)
    {
        float elapsedTime = 0;
        float startVolume = audioSource.volume;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, amount, elapsedTime / duration);
            yield return null;
        }

        audioSource.Pause();
    }
    private IEnumerator RaiseVolumeOverTime(float duration)
    {
        audioSource.UnPause();
        float elapsedTime = 0;
        float startVolume = audioSource.volume;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, SettingsManager.Instance.SoundSettings.BGMVolume, elapsedTime / duration);
            yield return new WaitForSeconds(duration);
        }

        audioSource.volume = SettingsManager.Instance.SoundSettings.BGMVolume;
    }
}