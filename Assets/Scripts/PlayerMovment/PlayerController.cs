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

    [SerializeField]
    private bool inCombat;

    // The Deselect Action from inputAction class.
    //private InputAction deSelect;

    // Reference to selected object in the scene that is moveable
    private GameObject MoveableObject;

    [Header("Player stats")]
    [SerializeField]
    private int health;
    [SerializeField]
    private int shield;
    [SerializeField]
    private int energy;
    [SerializeField]
    private int scrap;

    /// <summary>
    /// Returns PLayer Health
    /// </summary>
    public int Health
    {
        get { return health; }
        private set { health = value; }
    }
    public int Shield
    {
        get
        {
            return shield;
        }
        private set
        {
            shield = value;
        }
    }
    /// <summary>
    /// Returns PlayerEnergy
    /// </summary>
    public int Energy
    {
        get { return energy; }
        private set
        {
            energy = value;
        }
    }
    /// <summary>
    /// Player Scrap
    /// </summary>
    public int Scrap
    {
        get
        {
            return scrap;
        }
        set
        {
            scrap = value;
            if (scrap < 0) 
                scrap = 0;
        }
    }

    public bool InCombat
    {
        get { return inCombat; }
        set
        {
            inCombat = value;
        }
    }

    [Space(50)]
    /// <summary>
    /// Abilities player can do.
    /// </summary>
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
        Health = 50;
        Energy = 50;
        Scrap = 100;

        //loads abilities from folder
        //abilities.AddRange(Resources.LoadAll<Ability>("Abilities"));
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

                if (MoveableObject != null && hit.collider.CompareTag("Ground"))
                {
                    NavMeshAgent agent = MoveableObject.GetComponent<NavMeshAgent>();
                    if (agent != null)
                        agent.SetDestination(hit.point);
                }
            }
        }
    }
    /// <summary>
    /// Give player shield.
    /// </summary>
    /// <param name="shieldAmount"></param>
    public void ApplyShieldToPlayer(int shieldAmount)
    {
        Shield += shieldAmount;
    }
    /// <summary>
    /// Called when player has played a card or use an ability.
    /// will remove energy.
    /// </summary>
    /// <param name="energyUseage"></param>
    public void PlayedCardOrAbility(int energyUseage)
    {
        Energy -= energyUseage;
    }
    /// <summary>
    /// Deal Damage to player.
    /// </summary>
    /// <param name="damage">Amount of Damage as Int.</param>
    public void TakeDamage(int damage)
    {
        // if has shield
        if (Shield > 0)
        {
            if (damage >= Shield)
            {
                damage -= Shield;
                Shield = 0;
                Debug.Log(name + "Shield destroyed.");
            }
            else
            {
                // Reduce the shield by the damage amount
                Shield -= damage;
                // No remaining damage to apply to HP
                damage = 0;
            }            
        }
        Health = Health - damage;
    }

    /// <summary>
    /// A getter for enegry
    /// </summary>
    /// <returns></returns>
    public int GetEnergy()
    {
        return energy;
    }
    /// <summary>
    /// Returns the scrap stolen or whats left.
    /// </summary>
    /// <param name="amount">the amount of scrap want to steal</param>
    /// <returns></returns>
    public int StealScrap(int amount)
    {
        if (Scrap < amount)
        {
            int scrapleft = Scrap;
            Scrap -= amount;
            return scrapleft;
        }
        else
        {
            Scrap = Scrap - amount;
            return amount;
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
