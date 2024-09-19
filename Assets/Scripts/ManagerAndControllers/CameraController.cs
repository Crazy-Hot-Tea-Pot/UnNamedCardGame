using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Camera Info")]

    [SerializeField]
    //Default Camera Positon   
    private Vector3 defaultCameraPosition;

    [SerializeField]
    //Default Camera Rotation    
    private Quaternion defaultCameraRotation;

    [SerializeField]
    //Camera is in process of resetting
    private bool isResetting = false;

    [SerializeField]
    // Rotation tracking
    private bool isRotating = false;

    [SerializeField]
    //Speed for rotating the camera
    private float cameraRotationSpeed;

    [SerializeField]
    //Controls how fast the camera moves.    
    private float cameraSpeed;

    [SerializeField]
    //rotationSensitivity for more information look at RotationSensitivity in Camera Settings
    private float rotationSensitivity;

    [SerializeField]
    //Minimum Zoom in
    private float minimumZoom;

    [SerializeField]
    // Maxmum Zoom out
    private float maximumZoom;

    [Space(20)]

    [Header("Camera info (Editable)")]
    //Defines how close the mouse should be to the screen edges to trigger the camera movement.
    public float panBorderThickness = 10f;

    //Allows you to define boundaries within which the camera can move.
    public Vector2 panLimit = new Vector2(50,50);
   
    // Smooth zooming
    private float targetFOV;
    // Time for smoothing the zoom
    public float zoomSmoothTime = 0.2f;
    // A reference for smooth damp velocity
    private float zoomVelocity = 0f;
    

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
        playerInputActions.CameraControls.ResetCamera.performed += _ => StartCoroutine(ResetCamera());
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set initial FOV
        targetFOV = Camera.main.fieldOfView;

        defaultCameraPosition = SettingsManager.Instance.CameraSettings.DefaultCameraPosition;
        defaultCameraRotation = SettingsManager.Instance.CameraSettings.DefaultCameraRotation;
        cameraSpeed = SettingsManager.Instance.CameraSettings.CameraSpeed;
        cameraRotationSpeed = SettingsManager.Instance.CameraSettings.CameraRotationSpeed;
        rotationSensitivity = SettingsManager.Instance.CameraSettings.RotationSensitivity;
        minimumZoom=SettingsManager.Instance.CameraSettings.MinimumZoom;
        maximumZoom = SettingsManager.Instance.CameraSettings.MaximumZoom;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isResetting)
        {            
            if (isRotating)
            {
                RotateCamera();
            }
            CornerMovement();

            // Handle zoom functionality
            ZoomCamera();

            // Handle panning
            PanCamera();
        }
    }
    /// <summary>
    /// Moves Camera when mouse it as border.
    /// </summary>
    private void CornerMovement()
    {
        Vector3 pos = transform.position;

        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += cameraSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= cameraSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += cameraSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= cameraSpeed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
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
        float mouseX = playerInputActions.CameraControls.Look.ReadValue<Vector2>().x;
        float mouseY = playerInputActions.CameraControls.Look.ReadValue<Vector2>().y;

        // Rotate the camera around its Y-axis (horizontal movement of the mouse).
        //transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.up, mouseX * cameraRotationSpeed * rotationSensitivity * Time.deltaTime, Space.World);

        // Rotate the camera around its X-axis (vertical movement of the mouse).
        //transform.Rotate(Vector3.right, -mouseY * rotationSpeed * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.right, -mouseY * cameraRotationSpeed * rotationSensitivity * Time.deltaTime, Space.Self);
    }

    /// <summary>
    /// Handles camera zoom based on scroll wheel.
    /// </summary>
    private void ZoomCamera()
    {
        // Get zoom input from action
        float scrollInput = playerInputActions.CameraControls.Zoom.ReadValue<float>();

        // Only apply zoom if there is scroll input. Added this if statement as it just kept scrolling until max.
        if (scrollInput != 0)
        {
            // Calculate target FOV using zoomIncrement to avoid large jumps
            targetFOV -= scrollInput * cameraSpeed; //Mathf.Sign(scrollInput) * zoomIncrement;

            // Clamp target FOV to ensure it remains within the limits
            targetFOV = Mathf.Clamp(targetFOV, minimumZoom, maximumZoom);

            // Smoothly transition to the target FOV
            Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, targetFOV, ref zoomVelocity, zoomSmoothTime);
        }
        //// Get zoom input from action
        //float scrollInput = playerInputActions.CameraControls.Zoom.ReadValue<float>();

        //// Calculate target FOV
        //targetFOV -= scrollInput * zoomSpeed;
        //targetFOV = Mathf.Clamp(targetFOV, minZoom, maxZoom);

        //// Smooth the zoom transition using Mathf.SmoothDamp
        //Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, targetFOV, ref zoomVelocity, zoomSmoothTime);
    }

    /// <summary>
    /// Handles camera panning with the middle mouse button or arrow keys.
    /// </summary>
    private void PanCamera()
    {
        // Pan using the middle mouse button and mouse movement
        Vector2 panInput = playerInputActions.CameraControls.Pan.ReadValue<Vector2>();
        if (panInput != Vector2.zero)
        {
            Vector3 right = transform.right * panInput.x;
            Vector3 forward = transform.forward * panInput.y;
            forward.y = 0;  // Prevent vertical movement while panning

            transform.position += (right + forward) * cameraSpeed * Time.deltaTime;
        }

        // Pan using arrow keys or WASD
        Vector2 panKeyInput = playerInputActions.CameraControls.PanKeys.ReadValue<Vector2>();
        if (panKeyInput != Vector2.zero)
        {
            Vector3 right = transform.right * panKeyInput.x;
            Vector3 forward = transform.forward * panKeyInput.y;
            forward.y = 0;

            transform.position += (right + forward) * cameraSpeed * Time.deltaTime;
        }

        // Ensure camera stays within boundaries
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -panLimit.x, panLimit.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -panLimit.y, panLimit.y)
        );
    }

    /// <summary>
    /// Reset Camera
    /// </summary>
    private IEnumerator ResetCamera()
    {
        isResetting = true;
        // Loop until the camera reaches the target position and rotation
        while (Vector3.Distance(transform.position, defaultCameraPosition) > 0.01f || Quaternion.Angle(transform.rotation, defaultCameraRotation) > 0.1f)
        {
            // Smoothly interpolate position and rotation towards the default position
            transform.position = Vector3.Lerp(transform.position, defaultCameraPosition, cameraSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, defaultCameraRotation, cameraSpeed * Time.deltaTime);

            // Yield to wait until the next frame
            yield return null;
        }

        // Ensure the camera is exactly at the target position and rotation at the end
        transform.position = defaultCameraPosition;
        transform.rotation = defaultCameraRotation;
        Camera.main.fieldOfView = SettingsManager.Instance.CameraSettings.DefaultFOV;

        isResetting = false;
    }
    void OnDisable()
    {
        playerInputActions.CameraControls.RotateCamera.performed -= _ => StartRotation();
        playerInputActions.CameraControls.RotateCamera.canceled -= _ => StopRotation();
        playerInputActions.CameraControls.ResetCamera.performed -= _ => StartCoroutine(ResetCamera());
        playerInputActions.CameraControls.Disable();
    }
}
