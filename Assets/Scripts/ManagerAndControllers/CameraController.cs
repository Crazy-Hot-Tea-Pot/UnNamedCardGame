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

    public bool AllowBoarderMovement;

    public float screenWidth;
    public float screenHeight;
    public Vector2 mousePosition;
    public enum CameraState
    {
        Default,
        Rotation,
        Pan,
        BorderMovement,
        FirstPerson
    }

    [Header("Cameras")]
    [SerializeField]
    private CameraState CurrentCamera;

    public CinemachineVirtualCamera DefaultCamera;
    public CinemachineFreeLook RotationCamera;
    public CinemachineVirtualCamera PanCamera;
    public CinemachineVirtualCamera BorderCamera;
    public CinemachineVirtualCamera FirstPersonCamera;



    [Header("Camera Info")]
    
    public float cameraSpeed;
    public float panningSpeed;
    //5% from edge
    public float borderThreshold = 0.05f;


    public bool IsMouseAtLeftBorder
    {
        get
        {
            return mousePosition.x <= screenWidth * borderThreshold;
        }
    }
    public bool IsMouseAtRightBorder
    {
        get
        {
            return mousePosition.x >= screenWidth * (1 - borderThreshold);
        }
    }
    public bool IsMouseAtTopBorder
    {
        get
        {
            return mousePosition.y >= screenHeight * (1 - borderThreshold);
        }
    }
    public bool IsMouseAtBottomBorder
    {
        get
        {
            return mousePosition.y <= screenHeight * borderThreshold;
        }
    }

    // Input actions for controlling the camera
    private PlayerInputActions playerInputActions;


    void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        playerInputActions.CameraControls.Enable();

        playerInputActions.CameraControls.RotateCamera.performed += ctx => SwitchCamera(CameraState.Rotation);       
        playerInputActions.CameraControls.FreeCam.performed += ctx => SwitchCamera(CameraState.Pan);       
        playerInputActions.CameraControls.ResetCamera.performed += ctx => SwitchCamera(CameraState.Default);

        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }    


    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        

        DefaultCamera.Follow = player;
        DefaultCamera.LookAt = player;
        RotationCamera.Follow = player;
        RotationCamera.LookAt = player;

        SwitchCamera(CameraState.Default);        

    }

    void Update()
    {
        if (AllowBoarderMovement)
        {
            mousePosition = Mouse.current.position.ReadValue();
            if (IsMouseAtLeftBorder || IsMouseAtRightBorder || IsMouseAtTopBorder || IsMouseAtBottomBorder)
            {
                SwitchCamera(CameraState.BorderMovement);
                HandleBorderMovement();
            }
        }
        if (PanCamera.Priority == 10)
        {
            HandlePanCameraMovement();

        }
    }

    private void HandlePanCameraMovement()
    {
        Vector2 mouseDelta = playerInputActions.CameraControls.MoveCamera.ReadValue<Vector2>();

        // Move the camera relative to its local space
        Vector3 rightMovement = PanCamera.transform.right * mouseDelta.x * cameraSpeed * Time.deltaTime;
        Vector3 forwardMovement = PanCamera.transform.forward * mouseDelta.y * cameraSpeed * Time.deltaTime;

        // Keep the movement on a flat plane (ignore vertical movement)
        rightMovement.y = 0;
        forwardMovement.y = 0;

        // Apply the movement
        PanCamera.transform.position += rightMovement + forwardMovement;
    }

    private void HandleBorderMovement()
    {
        // Move the camera based on which border the mouse is at
        Vector3 movement = Vector3.zero;
        if (IsMouseAtLeftBorder)
        {
            movement += -BorderCamera.transform.right; // Move left
        }
        if (IsMouseAtRightBorder)
        {
            movement += BorderCamera.transform.right; // Move right
        }
        if (IsMouseAtTopBorder)
        {
            movement += BorderCamera.transform.forward; // Move forward/up
        }
        if (IsMouseAtBottomBorder)
        {
            movement += -BorderCamera.transform.forward; // Move backward/down
        }

        // Apply the movement
        BorderCamera.transform.position += movement * panningSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Switch the active camera based on the enum state
    /// Reset all cameras to lower priority
    /// Activate the desired camera based on the state
    /// </summary>
    /// <param name="state"></param>
    public void SwitchCamera(CameraState state)
    {       
        DefaultCamera.Priority = 0;
        RotationCamera.Priority = 0;
        PanCamera.Priority = 0;
        BorderCamera.Priority = 0;
        FirstPersonCamera.Priority = 0;
        
        switch (state)
        {
            case CameraState.Default:
                DefaultCamera.Priority = 10;
                break;
            case CameraState.Rotation:
                RotationCamera.Priority = 10;
                break;
            case CameraState.Pan:
                PanCamera.Priority = 10;
                break;
            case CameraState.BorderMovement:
                BorderCamera.Priority = 10;
                break;
            case CameraState.FirstPerson:
                FirstPersonCamera.Priority = 10;
                break;
            default:
                DefaultCamera.Priority = 10;
                break;
        }
        CurrentCamera = state;
    }

    void OnDisable()
    {

        playerInputActions.CameraControls.Disable();

    }
}
