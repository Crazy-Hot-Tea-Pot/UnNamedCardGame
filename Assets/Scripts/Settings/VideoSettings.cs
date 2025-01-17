using UnityEngine;

[System.Serializable]
public class VideoSettings
{
    // Screen resolution
    public Vector2Int Resolution;
    // Fullscreen, Windowed, Borderless
    public FullScreenMode FullscreenMode;
    // Toggle VSync
    public bool VSyncEnabled;
    // Quality level (0: Low, 1: Medium, etc.)
    public int GraphicsQuality;
    // Frame rate cap
    public int TargetFrameRate;
    // 0, 2, 4, 8
    public int AntiAliasingLevel;
    // Low, Medium, High, etc.
    public ShadowResolution ShadowQuality;
    // 0: High, 1: Medium, etc.
    public int TextureQuality;    
    // Toggle bloom
    public bool BloomEnabled;
}