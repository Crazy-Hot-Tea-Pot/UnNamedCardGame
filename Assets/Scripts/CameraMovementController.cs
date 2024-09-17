using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovementController : MonoBehaviour
{
    //Controls how fast the camera moves.
    public float panSpeed = 20f;

    //Defines how close the mouse should be to the screen edges to trigger the camera movement.
    public float panBorderThickness = 10f;

    //Allows you to define boundaries within which the camera can move.
    public Vector2 panLimit = new Vector2(50,50);

    //Speed for rotating the camera
    public float rotationSpeed = 100f;

    // Add a sensitivity modifier for finer control
    public float rotationSensitivity = 0.1f;

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
    void OnDisable()
    {
        playerInputActions.CameraControls.RotateCamera.performed -= _ => StartRotation();
        playerInputActions.CameraControls.RotateCamera.canceled -= _ => StopRotation();
        playerInputActions.CameraControls.Disable();
    }
}
