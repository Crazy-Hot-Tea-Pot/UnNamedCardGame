using UnityEngine;

public class CameraSettings
{
    private Vector3 defaultCameraPositon = new Vector3(0, 4, -10);
    private Quaternion defaultCameraRotation = Quaternion.Euler(30, 0, 0);
    private float cameraSpeed = 3f;
    private float rotationSpeed = 1f;
    private bool boarderMouseMovement;


    // Properties for camera settings
    public float ZoomSpeed { get; set; } = 5f;

    //Default Camera Positon
    public Vector3 DefaultCameraPosition {
        get
        {
            return defaultCameraPositon;
        }
        set
        {
            defaultCameraPositon = value;
        }
    }
    //Default Camera Rotation
    public Quaternion DefaultCameraRotation
    {
        get
        {
            return defaultCameraRotation;
        }
        set
        {
            defaultCameraRotation = value;
        }
    }
    //How fast the camera moves.
    public float CameraSpeed { 
        get => cameraSpeed; 
        //set => boarderCameraSpeed = value; 
    }
    /// <summary>
    /// Adjust rotationSpeed:
    /// Increase rotationSpeed for faster, more sensitive rotation.
    /// Decrease rotationSpeed for smoother, slower camera rotation.
    /// </summary>
    public float RotationSpeed
    {
        get => rotationSpeed;       
    }
    public bool BoarderMouseMovement
    {
        get
        {
            return boarderMouseMovement;
        }
        set
        {
            boarderMouseMovement = value;
        }
    }

    // Constructor
    public CameraSettings() { }

    //ToDO methods for resetting or adjusting camera settings
}