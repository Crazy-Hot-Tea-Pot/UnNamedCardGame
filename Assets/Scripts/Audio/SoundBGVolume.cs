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

        audioSource.volume = SettingsManager.Instance.SoundSettings.GetBGSoundForComponent();

        // Subscribe to the BGM volume change event
        if (SettingsManager.Instance?.SoundSettings != null)
        {
            SettingsManager.Instance.SoundSettings.OnBGMVolumeChanged += VolumeChange;
        }
    }

    private void VolumeChange(float newVolume)
    {
        if (newVolume > audioSource.volume)
        {
            // Adjust duration as needed
            RaiseVolume(1f);
        }
        else
        {
            // Adjust duration and pass the new volume
            LowerVolume(1f, newVolume);
        }
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
            audioSource.volume = Mathf.Lerp(startVolume, SettingsManager.Instance.SoundSettings.GetBGSoundForComponent(), elapsedTime / duration);
            yield return new WaitForSeconds(duration);
        }

        audioSource.volume = SettingsManager.Instance.SoundSettings.GetBGSoundForComponent();
    }

    void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        if (SettingsManager.Instance?.SoundSettings != null)
        {
            SettingsManager.Instance.SoundSettings.OnBGMVolumeChanged -= VolumeChange;
        }
    }
}