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

    [Header("Cameras")]

    public CinemachineVirtualCamera DefaultCamera;
    public CinemachineFreeLook RotationCamera;
    public CinemachineVirtualCamera PanCamera;



    [Header("Camera Info")]
    
    public float cameraSpeed;




    // Input actions for controlling the camera
    private PlayerInputActions playerInputActions;


    void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        playerInputActions.CameraControls.Enable();

        playerInputActions.CameraControls.RotateCamera.performed += ctx => SwitchToRotationCamera();
       // playerInputActions.CameraControls.RotateCamera.canceled += ctx => SwitchToDefaultCamera();
        playerInputActions.CameraControls.FreeCam.performed += ctx => SwitchToPanCamera();
        // playerInputActions.CameraControls.FreeCam.canceled += ctx => SwitchToDefaultCamera();
        playerInputActions.CameraControls.ResetCamera.performed += ctx => SwitchToDefaultCamera();
    }    


    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        DefaultCamera.Follow = player;
        DefaultCamera.LookAt = player;
        RotationCamera.Follow = player;
        RotationCamera.LookAt = player;

        SwitchToDefaultCamera();

    }

    void Update()
    {
        if (PanCamera.Priority == 10)
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
    }

    private void SwitchToDefaultCamera()
    {
        DefaultCamera.Priority = 10;
        RotationCamera.Priority = 0;
        PanCamera.Priority = 0;
    }

    private void SwitchToRotationCamera()
    {
        DefaultCamera.Priority = 0;
        RotationCamera.Priority = 10;
        PanCamera.Priority = 0;
    }
    private void SwitchToPanCamera()
    {
        PanCamera.Priority = 10;
        RotationCamera.Priority = 0;
        DefaultCamera.Priority = 0;
    }

    void OnDisable()
    {

        playerInputActions.CameraControls.Disable();

    }
}
