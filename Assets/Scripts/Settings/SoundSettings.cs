using System;
using UnityEngine;
using static SettingsData;

[System.Serializable]
public class SoundSettings
{
    //background Volume
    public float BGMVolume
    {
        get
        {
            return bgmVolume;
        }
        set
        {
            bgmVolume = value;
        }
    }

    // Sound Effects Volume
    public float SFXVolume
    {
        get
        {
            return sfxVolume;
        }
        private set
        {
            sfxVolume = value;
        }
    }

    public bool BGMMute
    {
        get
        {
            return bgmMute;
        }
        private set
        {
            bgmMute = value;
        }
    }

    public bool SFXMute
    {
        get
        {
            return sfxMute;
        }
        private set
        {
            sfxMute = value;
        }
    }

    [SerializeField]
    private float bgmVolume;
    [SerializeField]
    private float sfxVolume;
    [SerializeField]
    private bool bgmMute;
    [SerializeField]
    private bool sfxMute;

    public SoundSettings(SoundSettingsData data)
    {
        if (data.SettingsEdited)
        {
            SFXVolume = data.SFXVolume;
            bgmVolume = data.BGMVolume;
            SFXMute = data.SFXMute;
            BGMMute = data.BGMMute;
        }
        else
        {
            SFXVolume = 100f;
            BGMVolume = 100f;
            SFXMute = false;
            BGMMute = false;
        }
    }

    public SoundSettingsData GetDataToWrite()
    {
        return new SoundSettingsData
        {
            SettingsEdited = true,
            BGMVolume = BGMVolume,
            SFXVolume = SFXVolume,
        };
    }
}