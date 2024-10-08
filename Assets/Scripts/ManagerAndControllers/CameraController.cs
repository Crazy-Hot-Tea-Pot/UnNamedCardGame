using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Bugs:
/// After Rotating Camera border movement isn't correct direction.
/// </summary>
public class CameraController : MonoBehaviour
{
    private Transform player;
    private enum Camera
    {
        Default,
        Rotating,
        Pan
    }

    [Header("Cameras")]
    [SerializeField]
    private Camera currentCamera;

    public CinemachineVirtualCamera defaultCamera;
    public CinemachineFreeLook rotationCamera;    



    [Header("Camera Info")]
    [SerializeField]
    private float cameraSpeed;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float panSpeed;

    public float borderThickness = 10f;


    [SerializeField]
    private float minZoomDistance = 0.01f;
    [SerializeField]
    private float maxZoomDistance = 20f;

    [SerializeField]
    // Rotation tracking
    private bool isRotating = false;


    // Input actions for controlling the camera
    private PlayerInputActions playerInputActions;


    void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        playerInputActions.CameraControls.Enable();
        playerInputActions.CameraControls.RotateCamera.performed += _ => StartRotation();
        playerInputActions.CameraControls.RotateCamera.canceled += _ => StopRotation();
        playerInputActions.CameraControls.ResetCamera.performed += _ => ResetToDefaultCamera();
        playerInputActions.CameraControls.Zoom.performed += _ => ZoomCamera();
    }


    // Start is called before the first frame update
    void Start()
    {
        currentCamera = Camera.Default;

        cameraSpeed = SettingsManager.Instance.CameraSettings.CameraSpeed;

        rotationSpeed = SettingsManager.Instance.CameraSettings.RotationSpeed;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        defaultCamera.Follow = player;
        defaultCamera.LookAt = player;
        rotationCamera.Follow = player;
        rotationCamera.LookAt = player;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)        
            RotateCamera();        
    }
    private void ResetToDefaultCamera()
    {
        SwitchToCamera(Camera.Default);
    }
    /// <summary>
    /// Moves Camera when mouse it as border.
    /// </summary>
    private void BoarderMovement()
    {
        
    }

    /// <summary>
    /// Starts camera rotation.
    /// </summary>
    private void StartRotation()
    {
        isRotating = true;
    }

    /// <summary>
    /// Stops camera rotation.
    /// </summary>
    private void StopRotation()
    {
        isRotating = false;
    }

    /// <summary>
    /// Handles camera rotation based on mouse movement.
    /// </summary>
    private void RotateCamera()
    {
        SwitchToCamera(Camera.Rotating);
        Vector2 mouseDelta = playerInputActions.CameraControls.Look.ReadValue<Vector2>();

        // Rotate the camera based on mouse movement
        rotationCamera.m_XAxis.Value += mouseDelta.x * rotationSpeed * Time.deltaTime;
        rotationCamera.m_YAxis.Value -= mouseDelta.y * rotationSpeed * Time.deltaTime;

    }

    /// <summary>
    /// Handles camera zoom based on scroll wheel.
    /// </summary>
    private void ZoomCamera()
    {
        // Read the scroll value as a float (since it's a single axis control)
        float scrollAmount = playerInputActions.CameraControls.Zoom.ReadValue<float>();

        // Adjust the camera distance (z offset) for zooming on the default camera
        if (defaultCamera != null)
        {
            var framingTransposer = defaultCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (framingTransposer != null)
            {
                float currentDistance = framingTransposer.m_CameraDistance;
                currentDistance -= scrollAmount * cameraSpeed; // Use the scroll amount for zooming
                currentDistance = Mathf.Clamp(currentDistance, minZoomDistance, maxZoomDistance);
                framingTransposer.m_CameraDistance = currentDistance;
            }
        }

        // If we want zooming with other camera
        //// If using a FreeLookCamera, adjust the radius (zoom effect for FreeLook)
        //if (rotationCamera != null)
        //{
        //    float currentRadius = rotationCamera.m_Orbits[1].m_Radius; // Middle rig (default)
        //    currentRadius -= scrollAmount * cameraSpeed;
        //    currentRadius = Mathf.Clamp(currentRadius, minZoomDistance, maxZoomDistance);

        //    // Apply the same radius to all orbits (top, middle, bottom)
        //    for (int i = 0; i < rotationCamera.m_Orbits.Length; i++)
        //    {
        //        rotationCamera.m_Orbits[i].m_Radius = currentRadius;
        //    }
        //}
    }

    /// <summary>
    /// Handles camera panning with the middle mouse button or arrow keys.
    /// </summary>
    private void PanCamera()
    {

    }

    private void SwitchToCamera(Camera camera)
    {
        switch (camera)
        {
            case Camera.Default:
                defaultCamera.Priority = 10;
                //panCamera.Priority = 0;
                rotationCamera.Priority = 0;
                currentCamera = camera;
                break;
            case Camera.Rotating:
                rotationCamera.Priority = 10;
               // panCamera.Priority = 0;
                defaultCamera.Priority = 0;
                currentCamera = camera;
                break;                
            default:
                break;
        }
    }

    void OnDisable()
    {
        playerInputActions.CameraControls.RotateCamera.performed -= _ => StartRotation();
        playerInputActions.CameraControls.RotateCamera.canceled -= _ => StopRotation();
        playerInputActions.CameraControls.Disable();
    }
}
