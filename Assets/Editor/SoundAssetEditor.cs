using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(SoundAsset))]
public class SoundAssetEditor : Editor
{
    void OnEnable()
    {
        // Automatically regenerate enums
        SoundEnumGenerator.GenerateEnums(); 

        SoundAsset soundAsset = (SoundAsset)target;        

        //Automatically Populate
        PopulateSoundArrays(soundAsset);
    }
    //public override void OnInspectorGUI()
    //{
    //    DrawDefaultInspector(); // Draw the default inspector

    //    SoundAsset soundAsset = (SoundAsset)target;

    //    //Button to populate sounds
    //    if (GUILayout.Button("Populate Sounds from Resources"))
    //    {
    //        PopulateSoundArrays(soundAsset);
    //    }
    //}

    private void PopulateSoundArrays(SoundAsset soundAsset)
    {
        // Load Background Sounds from Resources/Sounds/BG
        AudioClip[] bgClips = Resources.LoadAll<AudioClip>("Sounds/BG");
        soundAsset.soundBGArray = new SoundAsset.BgSounds[bgClips.Length];
        for (int i = 0; i < bgClips.Length; i++)
        {
            string clipName = bgClips[i].name.Replace(" ", "_");
            if (System.Enum.TryParse(clipName, out BgSound bgEnumValue))
            {
                soundAsset.soundBGArray[i] = new SoundAsset.BgSounds
                {
                    bgSound = bgEnumValue,
                    audioClip = bgClips[i]
                };
            }
            else
            {
                Debug.LogWarning($"Enum not found for BG sound: {clipName}");
            }
        }

        // Load Sound Effects from Resources/Sounds/FX
        AudioClip[] fxClips = Resources.LoadAll<AudioClip>("Sounds/FX");
        soundAsset.soundFXClipArray = new SoundAsset.SoundFXClip[fxClips.Length];
        for (int i = 0; i < fxClips.Length; i++)
        {
            string clipName = fxClips[i].name.Replace(" ", "_");
            if (System.Enum.TryParse(clipName, out SoundFX fxEnumValue))
            {
                soundAsset.soundFXClipArray[i] = new SoundAsset.SoundFXClip
                {
                    soundFX = fxEnumValue,
                    audioClip = fxClips[i]
                };
            }
            else
            {
                Debug.LogWarning($"Enum not found for FX sound: {clipName}");
            }
        }

        // Save changes to the prefab instance
        EditorUtility.SetDirty(soundAsset);
        Debug.Log("Sound arrays populated successfully.");
    }
}
