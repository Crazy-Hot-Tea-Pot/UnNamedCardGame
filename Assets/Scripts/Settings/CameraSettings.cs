using System;
using Unity.VisualScripting;
using UnityEngine;
using static SettingsData;

[System.Serializable]
public class CameraSettings
{
    //How fast the camera moves.
    public float CameraSpeed
    {
        get
        {
            return cameraSpeed;
        }
        private set
        {
            cameraSpeed = value;
        }
    }

    //If Player wants BoarderMovement on or off.
    public bool BoarderMouseMovement
    {
        get
        {
            return boarderMouseMovement;
        }
        private set
        {
            boarderMouseMovement = value;
        }
    }

    [SerializeField]
    private float cameraSpeed;
    [SerializeField]
    private bool boarderMouseMovement;

    // Constructor
    public CameraSettings(CameraSettingsData data)
    {

        if (data.SettingsEdited)
        {
            cameraSpeed = data.CameraSpeed;
            BoarderMouseMovement = data.BoarderMouseMovement;
        }
        else
        {
            CameraSpeed = 3f;
            BoarderMouseMovement = false;            
        }
    }

    /// <summary>
    /// Returns Data for saving.
    /// </summary>
    /// <returns></returns>
    public CameraSettingsData GetDataToWrite()
    {
        return new CameraSettingsData
        {
            SettingsEdited = true,
            CameraSpeed = this.CameraSpeed,
            BoarderMouseMovement = this.BoarderMouseMovement
        };
    }

}