using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    /// <summary>
    /// This variable holds the game manager a different script and allows us to pull and pass variables and methods between them.
    /// </summary>
    public GameManager gameManager;
    /// <summary>
    /// This variable holds the input actions for the player to allow for user input
    /// </summary>
    public PlayerInputActions inputActions;

    /// <summary>
    /// This input action holds input for opening the inventory UI
    /// </summary>
    private InputAction openInventory;

    /// <summary>
    /// This input action holds input for dropping Inventory Items
    /// </summary>
    private InputAction dropItem;

    /// <summary>
    /// Instance variable for creating instances 
    /// </summary>
    private PlayerUIManager instance;

    /// <summary>
    /// Instance getter and setter
    /// </summary>
    public static PlayerUIManager Instance
    {
        get;
        private set;
    }

    #region InventoryUIVariables
    /// <summary>
    /// Inventory List for items in the players hands
    /// </summary>
    public List<GameObject> InventoryList;

    /// <summary>
    /// Holds the UI canvas for inventory
    /// </summary>
    public GameObject uiCanvas;
    /// <summary>
    /// Holds the chipPanel that the inventory UI must instantiate into
    /// </summary>
    public GameObject panelInventory;
    /// <summary>
    /// Holds the chipPanel that displays deck
    /// </summary>
    public GameObject panelDeck;
    /// <summary>
    /// Holds all active abilities
    /// </summary>
    public GameObject panelAbilty;

    /// <summary>
    /// A variable to tell us if the inventory UI is open
    /// </summary>
    private bool isActive = false;

    public bool IsActive
    {
        get
        {
            return isActive;
        }
        private set
        {
            isActive = value;
        }
    }

    /// <summary>
    /// Inventory button
    /// </summary>
    public GameObject buttonInv;

    /// <summary>
    /// Deck button
    /// </summary>
    public GameObject buttonDeck;
    #endregion

    #region UIResourcePools
    /// <summary>
    /// Health bar UI element
    /// </summary>
    public Slider healthBar;

    [Header("Energy Stuff")]

    public Image energyBar;

    //Speed to fill the bar at.
    public float fillSpeed;
    #endregion

    private void Awake()
    {

        //assigns player input class
        inputActions = new PlayerInputActions();
        //Assigns the input for the player to open the UI for inventory
        openInventory = inputActions.Player.InventoryUI;
        //Assign the input for the player to drop items from the UI
        dropItem = inputActions.Player.DropItem;
        //Enables the UI
        openInventory.Enable();

        #region donotdistroy
        //Checks if the instance is null
        if (Instance == null)
        {
            //Makes the object and instance
            Instance = this;
            //Makes the instance do not distroy
            DontDestroyOnLoad(this.gameObject);
        }
        //Deletes doublicates if there are any
        else
        {
            Destroy(this.gameObject);
        }
        #endregion
    }
    // Start is called before the first frame update
    void Start()
    {
        //Fills the inventory UI for deck
        fillDeck();

        //Sets UI maximums for resource pools
        StartingPools();

        //Initalizatoin for fill
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        //Update resource pool ui elements
        UpdateEnergy(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Energy, GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().MaxEnergy);
        UpdateHealth();
        //On hold open the inventroy UI if input is recieved
        if (openInventory.IsPressed() && !GameManager.Instance.InCombat)
       {
            OpenInventroy();
       }

    }

    #region InventoryUIStuff
    /// <summary>
    /// A method for the game to open the invnetory UI this method will also open the canvas of that UI if closed
    /// </summary>
    public void OpenInventroy()
    {
        //If the UI canvas is closed open it otherwise continue on
        if(uiCanvas.activeSelf == false)
        {
            //set ui canvas as active
            uiCanvas.SetActive(true);
        }

        //Tell the program the inventory is open
        IsActive = true;

    }

    /// <summary>
    /// Closes the invnetory UI if the canvas is open
    /// </summary>
    public void CloseInventory()
    {
        //If canvas is open close it
        if(uiCanvas.activeInHierarchy == true)
        {
            if(panelDeck.activeInHierarchy == true)
            {
                switchMenuInventory();
            }
            uiCanvas.SetActive(false);
        }

        //Tell the program the inventory is closed
        IsActive = false;
    }

    /// <summary>
    /// Switches the UI menu to deck
    /// </summary>
    public void switchMenuDeck()
    {
        //Disables active chipPanel and activate decks
        panelInventory.SetActive(false);
        panelDeck.SetActive(true);
        //Disable and enable buttons
        buttonDeck.SetActive(false);
        buttonInv.SetActive(true);
    }

    /// <summary>
    /// Switches the UI menu to Inventory
    /// </summary>
    public void switchMenuInventory()
    {
        //Disables active chipPanel and activate inventory
        panelDeck.SetActive(false);
        panelInventory.SetActive(true);
        //Disable and enable buttons
        buttonDeck.SetActive(true);
        buttonInv.SetActive(false);
    }

    /// <summary>
    /// Fills the deck in the UI in the PlayerManager to display deck inventory. ONLY USE AT START UP
    /// </summary>
    public void fillDeck()
    {
        //Removes all objects from panelDeck to repopulate.
        foreach (Transform child in panelDeck.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < gameManager.playerDeck.Count; i++)
        {
            GameObject chipTemp = Instantiate(gameManager.ChipPrefab, panelDeck.transform);
            Chip chipComponenet = chipTemp.GetComponent<Chip>();
            chipComponenet.newChip = gameManager.playerDeck[i];
        }
    }

    /// <summary>
    /// Adds a card to the UI of the player inventory. NOTE THIS HAS NOTHING TO DO WITH FUNCTIONAL DECK INVENTORY THAT'S GAMEMANAGER
    /// </summary>
    public void AddCardToDeck(NewChip card)
    {
        Instantiate(card.chipImage, panelDeck.transform);
    }

    /// <summary>
    /// Adds an item to the inventory UI
    /// </summary>
    /// <param name="item"></param>
    public void AddToInventory(GameObject item)
    {
        //No duplicated items
        if(!GameObject.FindGameObjectWithTag(item.tag))
        {
            Instantiate(item, panelInventory.transform);
        }
    }

    /// <summary>
    /// Instantiates abilites as UI objects so that they might be added into the game
    /// </summary>
    public void MakeActiveAbility(GameObject ability)
    {
        if(!GameObject.FindGameObjectWithTag(ability.tag))
        {
            Instantiate(ability, panelAbilty.transform);
        }
    }

    /// <summary>
    /// Destroys the ability from the UI
    /// </summary>
    /// <param name="ability"></param>
    public void MakeDeactiveAbility(string ability)
    {
        Destroy(GameObject.FindGameObjectWithTag(ability));
    }

    /// <summary>
    /// This function allows items to be removed from the UI it requires a object first and then a parent name deck or inventory as a string
    /// </summary>
    /// <param name="card"></param>
    public void RemoveFromUI(GameObject remObject, string parent)
    {
        if(parent == "Inventory")
        {
            //Destroys the game object from the UI element
            Destroy(GameObject.FindGameObjectWithTag(remObject.tag));
        }
        else if(parent == "Deck")
        {
            //Destroys the game object from the UI element
            Destroy(panelDeck.transform.Find(remObject.name));
        }

        
    }

    #endregion

    #region Resource Pool Methods

    /// <summary>
    /// Updates the UI for player health
    /// </summary>
    public void UpdateHealth()
    {
        //Change value of health bar
        healthBar.value = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Health;
    }

    //Sets variables to initalize fill speed
    public void Initialize()
    {
        fillSpeed = 0.5f;
    }

    //Provides the ability to update energy right away
    public void InstantUpdateEnergy(float currentEnergy)
    {
        energyBar.fillAmount = currentEnergy;
    }

    /// <summary>
    /// Call this to change the Energy Bar UI. this method accepts both UI and Float no need to change before.
    /// </summary>
    /// <param name="currentEnergy"></param>
    /// <param name="maxEnergy"></param>
    public void UpdateEnergy(float currentEnergy, float maxEnergy)
    {
        if (energyBar != null)
        {
            // Normalize the energy value to a 0-1 range
            float tempTargetFillAmount = currentEnergy / maxEnergy;

            //Clap at 0 and 1 so don't go over
            tempTargetFillAmount = Mathf.Clamp01(tempTargetFillAmount);

            StopAllCoroutines();

            StartCoroutine(FillEnergyOverTime(tempTargetFillAmount));
        }
        else
        {
            Debug.LogError("Energy Bar Image component not found.");
        }
    }


    /// <summary>
    /// Fill EnergyBar by amount over time.
    /// </summary>
    /// <param name="targetFillAmount"></param>
    /// <returns></returns>
    private IEnumerator FillEnergyOverTime(float targetFillAmount)
    {

        // While the bar is not at the target fill amount, update it
        while (!Mathf.Approximately(energyBar.fillAmount, targetFillAmount))
        {
            // Lerp between current fill and target fill by the fill speed
            energyBar.fillAmount = Mathf.Lerp(energyBar.fillAmount, targetFillAmount, fillSpeed * Time.deltaTime);

            // Ensure the fill value gradually updates each frame
            yield return null;
        }

        // Ensure it snaps to the exact target amount at the end
        energyBar.fillAmount = targetFillAmount;

    }

    /// <summary>
    /// UI starting points that set the sliders to the maximum of the variables
    /// </summary>
    public void StartingPools()
    {
        healthBar.maxValue = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().MaxHealth;
    }
    #endregion
}
