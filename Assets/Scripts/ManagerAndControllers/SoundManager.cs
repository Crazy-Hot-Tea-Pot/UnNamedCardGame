using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    //public enum SoundFX
    //{
    //    None
    //}
    //public enum BgSound
    //{
    //    None
    //}


    /// <summary>
    /// Call after changing sound volume.
    /// </summary>
    public static void MasterVolumeChanged()
    {
        try
        {
            GameObject.Find("BgSound").GetComponent<AudioSource>().volume = SettingsManager.Instance.SoundSettings.BGMVolume;
        }
        catch
        {
            Debug.LogError("BG doesn't Exist");
        }
    }

    public static void StartBackground(BgSound bgSound)
    {
        GameObject BackgroundSound = new GameObject("BgSound");
        AudioSource audioSource = BackgroundSound.AddComponent<AudioSource>();
        audioSource.clip = GetBGAudio(bgSound);
        audioSource.loop = true;
        audioSource.volume = SettingsManager.Instance.SoundSettings.BGMVolume;
        audioSource.Play();
        BackgroundSound.AddComponent<SoundBGVolume>();
    }
    /// <summary>
    /// Plays audio for one interation.
    /// </summary>
    /// <param name="sound"></param>
    public static void PlaySound(SoundFX sound)
    {
        GameObject soundGameObject = new GameObject("SoundFX");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

        audioSource.PlayOneShot(GetAudio(sound));
        //Set audio to 3D
        audioSource.spatialBlend = 1.0f;
        // audioSource.PlayClipAtPoint(GetAudio(sound), transform.TransformPoint(_controller.center));
        audioSource.volume = SettingsManager.Instance.SoundSettings.SFXVolume;
        soundGameObject.AddComponent<SoundFXLife>().SoundLength = GetAudio(sound).length;
    }

    private static AudioClip GetBGAudio(BgSound sound)
    {
        foreach (SoundAsset.BgSounds Bgsound in SoundAsset.soundAssets.soundBGArray)
        {
            if (Bgsound.bgSound == sound)
            {
                return Bgsound.audioClip;
            }
        }
        Debug.LogError("Sound" + sound + "not found");
        return null;
    }
    /// <summary>
    /// Gets audio clip from sound assets
    /// </summary>
    /// <param name="sound"></param>
    /// <returns></returns>
    private static AudioClip GetAudio(SoundFX sound)
    {
        foreach (SoundAsset.SoundFXClip soundFxClip in SoundAsset.soundAssets.soundFXClipArray)
        {
            if (soundFxClip.soundFX == sound)
            {
                return soundFxClip.audioClip;
            }
        }
        Debug.LogError("Sound" + sound + "not found");
        return null;
    }
}
