using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovementController : MonoBehaviour
{
    [Header("Pan Settings")]
    //Controls how fast the camera moves.
    public float panSpeed = 20f;

    //Defines how close the mouse should be to the screen edges to trigger the camera movement.
    public float panBorderThickness = 10f;

    //Allows you to define boundaries within which the camera can move.
    public Vector2 panLimit = new Vector2(50,50);

    [Header("Rotation Settings")]
    //Speed for rotating the camera
    public float rotationSpeed = 100f;

    /// <summary>
    /// Adjust rotationSensitivity:
    /// Increase rotationSensitivity for faster, more sensitive rotation.
    /// Decrease rotationSensitivity for smoother, slower camera rotation.
    /// </summary>
    public float rotationSensitivity = 0.1f;

    [Header("Zoom Settings")]    
    // Zoom Speed
    public float zoomSpeed = 5f;
    //How much FOV should change per scroll
    public float zoomIncrement = 5f;
    // Minimum Zoom in
    public float minZoom = 10f;
    // Maxmum Zoom out
    public float maxZoom = 60f;
    // Smooth zooming
    private float targetFOV;
    // Time for smoothing the zoom
    public float zoomSmoothTime = 0.2f;
    // A reference for smooth damp velocity
    private float zoomVelocity = 0f;
    

    // Input actions for controlling the camera
    private PlayerInputActions playerInputActions;

    // Rotation tracking
    private bool isRotating = false;

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        playerInputActions.CameraControls.Enable();
        playerInputActions.CameraControls.RotateCamera.performed += _ => StartRotation();
        playerInputActions.CameraControls.RotateCamera.canceled += _ => StopRotation();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set initial FOV
        targetFOV = Camera.main.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        CornerMovement(pos);

        if (isRotating)
        {
            RotateCamera();
        }

        // Handle zoom functionality
        ZoomCamera();

        // Handle panning
        PanCamera();
    }
    /// <summary>
    /// Moves Camera when mouse it as border.
    /// </summary>
    private void CornerMovement(Vector3 pos)
    {        

        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
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
        transform.Rotate(Vector3.up, mouseX * rotationSpeed * rotationSensitivity * Time.deltaTime, Space.World);

        // Rotate the camera around its X-axis (vertical movement of the mouse).
        //transform.Rotate(Vector3.right, -mouseY * rotationSpeed * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.right, -mouseY * rotationSpeed * rotationSensitivity * Time.deltaTime, Space.Self);
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
            targetFOV -= scrollInput * zoomIncrement; //Mathf.Sign(scrollInput) * zoomIncrement;

            // Clamp target FOV to ensure it remains within the limits
            targetFOV = Mathf.Clamp(targetFOV, minZoom, maxZoom);

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

            transform.position += (right + forward) * panSpeed * Time.deltaTime;
        }

        // Pan using arrow keys or WASD
        Vector2 panKeyInput = playerInputActions.CameraControls.PanKeys.ReadValue<Vector2>();
        if (panKeyInput != Vector2.zero)
        {
            Vector3 right = transform.right * panKeyInput.x;
            Vector3 forward = transform.forward * panKeyInput.y;
            forward.y = 0;

            transform.position += (right + forward) * panSpeed * Time.deltaTime;
        }

        // Ensure camera stays within boundaries
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -panLimit.x, panLimit.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -panLimit.y, panLimit.y)
        );
    }
    void OnDisable()
    {
        playerInputActions.CameraControls.RotateCamera.performed -= _ => StartRotation();
        playerInputActions.CameraControls.RotateCamera.canceled -= _ => StopRotation();
        playerInputActions.CameraControls.Disable();
    }
}
