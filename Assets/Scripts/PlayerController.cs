using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;
    public PlayerInputActions inputActions;

    private InputAction select;
    private InputAction deSelect;

    public MoveAbleObject selectedObjectInstance;

    // Awake is called
    void Awake()
    {
        inputActions = new PlayerInputActions();

        // Automatically finds the camera
        mainCamera = Camera.main;
    }
    /// <summary>
    /// Enables
    /// </summary>
    private void OnEnable()
    {
        select = inputActions.Player.Select;
        select.Enable();
        select.performed += OnClick;

        deSelect = inputActions.Player.DeSelect;
        deSelect.Enable();
        deSelect.performed += OnDeselect;
    }

    /// <summary>
    /// Disables
    /// </summary>
    private void OnDisable()
    {
        select.Disable();
        select.performed -= OnClick;

        deSelect.Disable();
        deSelect.performed -= OnDeselect;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.name);

            MoveAbleObject hitObject = hit.collider.gameObject.GetComponent<MoveAbleObject>();

            if (hitObject != null)
                selectedObjectInstance = hitObject;

            if (selectedObjectInstance != null && hit.collider.CompareTag("Ground"))
            {
                NavMeshAgent agent = selectedObjectInstance.GetComponent<NavMeshAgent>();
                if (agent != null)
                    agent.SetDestination(hit.point);
            }
        }
    }
    private void OnDeselect(InputAction.CallbackContext context)
    {
        if (selectedObjectInstance != null)
        {
            selectedObjectInstance.Deselect();
            selectedObjectInstance=null;
        }
    }
}
