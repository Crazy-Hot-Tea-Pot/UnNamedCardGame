using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BackgroundSoundTool : EditorWindow
{
    private BgSound selectedBgSound;
    private float defaultVolume = 1f;
    // Toggle for persistent background sound
    private bool isGlobal = false;

    // List of selected levels for play and pause
    private List<Levels> playInScenes = new List<Levels>();
    private List<Levels> pauseInScenes = new List<Levels>();

    [MenuItem("Tools/Background Sound Tool")]
    public static void ShowWindow()
    {
        GetWindow<BackgroundSoundTool>("Background Sound Tool");
    }

    private void OnGUI()
    {

        GUILayout.Label("Add Background Sound", EditorStyles.boldLabel);

        // Select a background sound
        selectedBgSound = (BgSound)EditorGUILayout.EnumPopup("Background Sound", selectedBgSound);

        // Volume control
        defaultVolume = EditorGUILayout.Slider("Default Volume", defaultVolume, 0f, 1f);

        // Persistent toggle
        isGlobal = EditorGUILayout.Toggle("Play Across Scenes", isGlobal);

        // Show "Play in Scenes" option only when isGlobal is true
        if (isGlobal)
        {
            GUILayout.Label("Play in Scenes", EditorStyles.boldLabel);
            foreach (Levels level in System.Enum.GetValues(typeof(Levels)))
            {
                bool isSelected = playInScenes.Contains(level);
                bool toggle = EditorGUILayout.Toggle(level.ToString(), isSelected);
                if (toggle && !isSelected)
                {
                    playInScenes.Add(level);
                }
                else if (!toggle && isSelected)
                {
                    playInScenes.Remove(level);
                }
            }

            // Multi-select dropdown for "Pause in Scenes"
            GUILayout.Label("Pause in Scenes", EditorStyles.boldLabel);
            foreach (Levels level in System.Enum.GetValues(typeof(Levels)))
            {
                bool isSelected = pauseInScenes.Contains(level);
                bool toggle = EditorGUILayout.Toggle(level.ToString(), isSelected);
                if (toggle && !isSelected)
                {
                    pauseInScenes.Add(level);
                }
                else if (!toggle && isSelected)
                {
                    pauseInScenes.Remove(level);
                }
            }
        }        

        if (GUILayout.Button("Add Background Sound to Scene"))
        {
            AddBackgroundSoundToScene();
        }

    }

    private void AddBackgroundSoundToScene()
    {
        // Delete the existing Global background sound if it exists
        GameObject existingGlobalSound = GameObject.Find("GlobalBgSound");
        if (existingGlobalSound != null)
        {
            DestroyImmediate(existingGlobalSound);
        }

        // Create a new GameObject for the background sound
        string soundObjectName = isGlobal ? "GlobalBgSound" : "BgSound";
        GameObject bgSoundObject = new GameObject(soundObjectName);

        //Make object stay in scene
        Undo.RegisterCreatedObjectUndo(bgSoundObject, "Add Background Sound");


        // Add AudioSource
        AudioSource audioSource = bgSoundObject.AddComponent<AudioSource>();
        audioSource.clip = GetBGAudio(selectedBgSound);
        audioSource.loop = true;
        audioSource.volume = defaultVolume;
        audioSource.Play();
        bgSoundObject.AddComponent<SoundBGVolume>();

        // Attach the Global script if needed
        if (isGlobal)
        {
            bgSoundObject.AddComponent<BackgroundSoundGlobal>();
            bgSoundObject.GetComponent<BackgroundSoundGlobal>().SetSceneBehavior(playInScenes, pauseInScenes);
        }

        // Inform the user
        EditorUtility.DisplayDialog("Background Sound", $"{(isGlobal ? "Global" : "Scene-only")} background sound added to the scene!", "OK");
    }

    private AudioClip GetBGAudio(BgSound sound)
    {
        foreach (SoundAsset.BgSounds bgSound in SoundAsset.soundAssets.soundBGArray)
        {
            if (bgSound.bgSound == sound)
            {
                return bgSound.audioClip;
            }
        }

        Debug.LogError($"Sound {sound} not found in SoundAssets.");
        return null;
    }
}
