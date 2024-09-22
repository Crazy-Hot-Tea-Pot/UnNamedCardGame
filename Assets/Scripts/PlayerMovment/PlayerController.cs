using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

// Controller for player this class is not the input class that is generated.
public class PlayerController : MonoBehaviour
{
    // Reference to inputAction class that is generated by unity.
    public PlayerInputActions playerInputActions;

    //Camera in the scene
    private Camera mainCamera;

    // The Select Action from inputAction class.
    private InputAction select;

    // The Deselect Action from inputAction class.
    //private InputAction deSelect;

    // Reference to selected object in the scene that is moveable
    private GameObject MoveableObject;

    [Header("Player stats")]
    [SerializeField]
    private int health;
    [SerializeField]
    private int energy;

    /// <summary>
    /// Returns PLayer Health
    /// </summary>
    public int Health
    {
        get { return health; }
    } 
    /// <summary>
    /// Returns PlayerEnergy
    /// </summary>
    public int Energy
    {
        get { return energy; }
    }

    [Space(50)]
    /// <summary>
    /// Abilities player can do.
    /// Loaded in code.
    /// </summary>
    [Tooltip("Loaded on scene load.")]
    public List<Ability> abilities = new List<Ability>();

    // Awake is called when instance is being loaded
    void Awake()
    {
        // assign player Input class
        playerInputActions = new PlayerInputActions();

        // Automatically finds the camera
        mainCamera = Camera.main;

    }
    void Start()
    {
        //assign Object To Move if empty
        if(MoveableObject == null)
            MoveableObject = GameObject.FindGameObjectWithTag("Player");

        Initialize();
    }

    /// <summary>
    /// Initialize Player
    /// </summary>
    void Initialize()
    {
        health = 50;
        energy = 50;

        //loads abilities from folder
        abilities.AddRange(Resources.LoadAll<Ability>("Abilities"));
    }
    /// <summary>
    /// Enables
    /// </summary>
    private void OnEnable()
    {
        select = playerInputActions.Player.Select;
        select.Enable();
        select.performed += OnClick;

        //deSelect = playerInputActions.Player.DeSelect;
        //deSelect.Enable();
        //deSelect.performed += OnDeselect;
    }

    /// <summary>
    /// Disables
    /// </summary>
    private void OnDisable()
    {
        select.Disable();
        select.performed -= OnClick;

        //deSelect.Disable();
        //deSelect.performed -= OnDeselect;
    }

    /// <summary>
    /// On click is run everytime the user clicks into the scene.
    /// Using Physics raycast.
    /// Depends on the result it will either:
    /// Assign the selectedObject .
    /// Move the selectedObejct to the position on Ground.
    /// </summary>
    private void OnClick(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.InCombat)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.transform.name);

                //MoveAbleObject hitObject = hit.collider.gameObject.GetComponent<MoveAbleObject>();

                //if (hitObject != null)
                //    selectedObjectInstance = hitObject;

                if (MoveableObject != null && hit.collider.CompareTag("Ground"))
                {
                    NavMeshAgent agent = MoveableObject.GetComponent<NavMeshAgent>();
                    if (agent != null)
                        agent.SetDestination(hit.point);
                }
            }
        }
    }

    //Deselect the object in instance and do other clean up.
    //private void OnDeselect(InputAction.CallbackContext context)
    //{
    //    if (selectedObjectInstance != null)
    //    {
    //        selectedObjectInstance.Deselect();
    //        selectedObjectInstance=null;
    //    }
    //}
}
