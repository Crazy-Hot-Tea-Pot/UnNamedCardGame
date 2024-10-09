using UnityEngine;
/// <summary>
/// Loads audio from reasources
/// </summary>
public class SoundAsset : MonoBehaviour
{
    private static SoundAsset instance;


    public static SoundAsset soundAssets
    {
        get
        {
            if (instance == null)
                instance = Instantiate(Resources.Load<SoundAsset>("SoundAssets"));
            return instance;
        }

    }

    /// <summary>
    /// An array of Background sounds.
    /// </summary>
    public BgSounds[] soundBGArray;

    [System.Serializable]
    public class BgSounds
    {
        //public SoundManager.BgSound sound;
        public BgSound bgSound;
        public AudioClip audioClip;
    }

    //An array of Other Sounds.
    public SoundFXClip[] soundFXClipArray;

    [System.Serializable]
    public class SoundFXClip
    {
        //public SoundManager.SoundFX sound;
        public SoundFX soundFX;
        public AudioClip audioClip;
    }

}