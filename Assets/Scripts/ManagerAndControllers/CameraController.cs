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
        playerInputActions.CameraControls.RotateCamera.canceled += ctx => SwitchToDefaultCamera();
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

    }

    private void SwitchToDefaultCamera()
    {
        DefaultCamera.Priority = 10;
        RotationCamera.Priority = 0;
    }

    private void SwitchToRotationCamera()
    {
        DefaultCamera.Priority = 0;
        RotationCamera.Priority = 10;
    }

    void OnDisable()
    {

        playerInputActions.CameraControls.Disable();

    }
}
