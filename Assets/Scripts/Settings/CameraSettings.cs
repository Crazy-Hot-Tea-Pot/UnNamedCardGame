using UnityEngine;

public class CameraSettings
{
    private Vector3 defaultCameraPositon = new Vector3(0, 4, -10);
    private Quaternion defaultCameraRotation = Quaternion.Euler(30, 0, 0);
    private float defaultFOV = 60f;
    private float cameraSpeed = 3f;
    private float cameraRotationSpeed = 100f;
    private float rotationSensitivity=0.1f;
    private float minimumZoom=10f;
    private float maximumZoom = 150f;


    // Properties for camera settings
    public float ZoomSpeed { get; set; } = 5f;

    //Default Camera Positon
    public Vector3 DefaultCameraPosition {
        get
        {
            return defaultCameraPositon;
        }
        //set
        //{
        //    defaultCameraPositon = value;
        //}
    }
    //Default Camera Rotation
    public Quaternion DefaultCameraRotation
    {
        get
        {
            return defaultCameraRotation;
        }
        //set
        //{
        //    defaultCameraRotation = value;
        //}
    }
    //How fast the camera moves.
    public float CameraSpeed { 
        get => cameraSpeed; 
        //set => cameraSpeed = value; 
    }
    //Speed for rotating the camera
    public float CameraRotationSpeed {
        get => cameraRotationSpeed;
        //set => rotationSpeed = value; 
    }
    /// <summary>
    /// Adjust rotationSensitivity:
    /// Increase rotationSensitivity for faster, more sensitive rotation.
    /// Decrease rotationSensitivity for smoother, slower camera rotation.
    /// </summary>
    public float RotationSensitivity
    {
        get => rotationSensitivity;       
    }
    public float MinimumZoom
    {
        get
        {
            return minimumZoom;
        }
    }

    public float MaximumZoom { 
        get => maximumZoom; 
        //set => maximumZoom = value; 
    }
    public float DefaultFOV { 
        get => defaultFOV; 
       // set => defaultFOV = value; 
    }

    // Constructor
    public CameraSettings() { }

    //ToDO methods for resetting or adjusting camera settings
}