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

    private float bgmVolume;

    // Sound Effects Volume
    public float SFXVolume = 100f;

    public SoundSettings()
    {
        BGMVolume = 100f;
    }
    
}