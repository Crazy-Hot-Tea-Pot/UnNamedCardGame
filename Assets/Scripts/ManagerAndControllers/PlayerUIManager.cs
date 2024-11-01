using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    /// <summary>
    /// Holds the ui for player screen like health bars
    /// </summary>
    public GameObject uiPlayerCanvas;

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
    /// A list for card selection
    /// </summary>
    private List<NewChip> selectionList;

    /// <summary>
    /// A short selection list for the final list of players picking cards
    /// </summary>
    private List<NewChip> shortSelection;

    //Options for the player to use when selecting new cards
    public GameObject panelDropCards;
    private GameObject optionOne;
    private GameObject optionTwo;
    private GameObject optionThree;
    private Image imageOne;
    private Image imageTwo;
    private Image imageThree;

    //Panel for deleting cards
    public GameObject panelCardDelete;

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
    public GameObject uiInventoryCanvas;

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

        //Add the card buttons for drops and attach buttons
#pragma warning disable CS0618 // Type or member is obsolete is suppressed because I need this find child
        GameObject parent = GameObject.Find(panelDropCards.transform.FindChild("CardScreen").name);
        optionOne = GameObject.Find(parent.transform.FindChild("OptionOne").name);
        optionOne.AddComponent<Button>().onClick.AddListener(cardOptionOne);
        optionTwo = GameObject.Find(parent.transform.FindChild("OptionTwo").name);
        optionTwo.AddComponent<Button>().onClick.AddListener(cardOptionTwo);
        optionThree = GameObject.Find(parent.transform.FindChild("OptionThree").name);
        optionThree.AddComponent<Button>().onClick.AddListener(cardOptionThree);
        panelDropCards.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Update resource pool ui elements
        UpdateEnergy(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Energy, GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().MaxEnergy);
        UpdateHealth();
        //On press open the inventroy UI if input is recieved or close if already in combat. WasPressedThisFrame() makes the input not be spammed it waits till the frame ends before recollecting
        if (openInventory.WasPressedThisFrame() && !GameManager.Instance.InCombat)
        {
            if (uiInventoryCanvas.activeSelf == false)
            {
                OpenInventroy();
            }
            else if (uiInventoryCanvas.activeSelf == true)
            {
                CloseInventory();
            }
        }

    }

    #region InventoryUIStuff
    /// <summary>
    /// A method for the game to open the invnetory UI this method will also open the canvas of that UI if closed
    /// </summary>
    public void OpenInventroy()
    {
        //If the UI canvas is closed open it otherwise continue on
        if (uiInventoryCanvas.activeSelf == false)
        {
            //set ui canvas as active
            uiInventoryCanvas.SetActive(true);
        }

        //Tell the program the inventory is open
        IsActive = true;
        //Close playerUI
        uiPlayerCanvas.SetActive(false);
    }

    /// <summary>
    /// Closes the invnetory UI if the canvas is open
    /// </summary>
    public void CloseInventory()
    {
        //If canvas is open close it
        if (uiInventoryCanvas.activeInHierarchy == true)
        {
            //If deck panel is open
            if (panelDeck.activeInHierarchy == true)
            {
                //Switch to default screen
                switchMenuInventory();
            }
            uiInventoryCanvas.SetActive(false);
        }

        //Tell the program the inventory is closed
        IsActive = false;
        //Open PlayerUI
        uiPlayerCanvas.SetActive(true);
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
            chipComponenet.IsInInventoryChip = true;
            chipComponenet.newChip = gameManager.playerDeck[i];
        }
    }

    /// <summary>
    /// Adds a card to the UI of the player inventory. NOTE THIS HAS NOTHING TO DO WITH FUNCTIONAL DECK INVENTORY THAT'S GAMEMANAGER
    /// </summary>
    public void AddCardToDeck(NewChip card)
    {
        Instantiate(card.chipImage, panelDeck.transform);
        card.GameObject().GetComponent<Chip>().IsInInventoryChip = true;
    }

    /// <summary>
    /// Adds an item to the inventory UI
    /// </summary>
    /// <param name="item"></param>
    public void AddToInventory(GameObject item)
    {
        //No duplicated items
        if (!GameObject.FindGameObjectWithTag(item.tag))
        {
            Instantiate(item, panelInventory.transform);
        }
    }

    /// <summary>
    /// Instantiates abilites as UI objects so that they might be added into the game
    /// </summary>
    public void MakeActiveAbility(GameObject ability)
    {
        if (!GameObject.FindGameObjectWithTag(ability.tag))
        {
            Instantiate(ability, panelAbilty.transform);
        }
    }

    /// <summary>
    /// Destroys the ability from the UI
    /// </summary>
    /// <param name="ability"></param>
    public void MakeDeactiveAbility(GameObject ability)
    {
        //We have to renable the player canvas to delete from it
        uiPlayerCanvas.SetActive(true);
        Destroy(GameObject.FindGameObjectWithTag(ability.tag));
        //Hide it quick enough the player doesn't see
        uiPlayerCanvas.SetActive(false);
    }

    /// <summary>
    /// This function allows items to be removed from the UI it requires a object first and then a parent name Deck, Swap or Inventory as a string
    /// </summary>
    /// <param name="card"></param>
    public void RemoveFromUI(GameObject remObject, string parent)
    {
        if (parent == "Inventory")
        {
            //Destroys the game object from the UI element
            Destroy(GameObject.FindGameObjectWithTag(remObject.tag));
        }
        else if (parent == "Deck")
        {
            //Destroys the game object from the UI element
            Destroy(panelDeck.transform.Find(remObject.name));
        }
        else if(parent == "Swap")
        {
            //Destroys the game object from UI element
            Destroy(panelCardDelete.transform.Find(remObject.name));
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

    #region CardDropUIandFunction
    /// <summary>
    /// This method adds enemies cards to the list
    /// </summary>
    public void AddChipChoices(NewChip chip)
    {
        selectionList.Add(chip);
    }

    /// <summary>
    /// This method allows us to draw cards from enemy and use this to present a list of available cards to the player
    /// </summary>
    public void DropCard()
    {
        //If our short list isn't too big
        if (shortSelection.Count < 3)
        {
            //Roll a random number of the list length 3 times
            for (int i = 0; i < 3; i++)
            {
                NewChip holder;
                //Get a random number
                int roll = GameManager.Instance.Roll(1, selectionList.Count);
                //Assign holder that random roll
                holder = selectionList[roll];

                //If there can be no repeats
                if (selectionList.Count >= 3)
                {
                    //Fail out counter
                    int failOut = 0;
                    //No repeats
                    while (shortSelection.Contains(holder))
                    {
                        failOut++;
                        //Redo random number
                        int reroll = GameManager.Instance.Roll(1, selectionList.Count);
                        //Replace holder so the loop can exit
                        holder = selectionList[reroll];

                        //If we have looped 5 times forget it and move on a duplicate is better then an infinite loop. This is most likely to happen if there are too many duplicating cards
                        //Especially if fighting two of the same enemies
                        if(failOut == 5)
                        {
                            break;
                        }
                    }
                    //Add to the short list
                    shortSelection.Add(holder);
                }
                //If the list is too small then continue from here
                else if (selectionList.Count < 3)
                {
                    //Add to the short list
                    shortSelection.Add(holder);
                }
            }
        }

        //Update the image sprites
        imageOne.sprite = shortSelection[0].chipImage;
        imageTwo.sprite = shortSelection[1].chipImage;
        imageThree.sprite = shortSelection[3].chipImage;

    }

    /// <summary>
    /// Card drop UI is opened
    /// </summary>
    public void openDropUI()
    {
        //Display UI
        panelDropCards.SetActive(true);
    }

    /// <summary>
    /// Card drop UI is closed
    /// </summary>
    public void closeDropUI()
    {
        panelDropCards.SetActive(false);
        //Open panel card delete for next section
        panelCardDelete.SetActive(true);
    }

    /// <summary>
    /// Closes the swap panel
    /// </summary>
    public void CloseSwapPanel()
    {
        panelCardDelete.SetActive(false);
    }

    /// <summary>
    /// This works just like the fill deck method except that it fills the panelDropCards with cards
    /// </summary>
    public void FillCardSwap()
    {
        closeDropUI();
        //Removes all objects from panelCardDelete to repopulate.
        foreach (Transform child in panelCardDelete.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < gameManager.playerDeck.Count; i++)
        {
            //Creates the chip object in card delete pannel
            GameObject chipTemp = Instantiate(gameManager.ChipPrefab, panelCardDelete.transform);
            //Destroy button component to then replace
            Destroy(chipTemp.GetComponent<Button>());
            //Adds a listener on a button that on click will use the swap chip variable
            chipTemp.AddComponent<Button>().onClick.AddListener(() => ChipSwap(chipTemp));
            Chip chipComponenet = chipTemp.GetComponent<Chip>();
            chipComponenet.IsInInventoryChip = true;
            chipComponenet.newChip = gameManager.playerDeck[i];
        }
    }

    /// <summary>
    /// A method to remove a card when clicked
    /// </summary>
    public void ChipSwap(GameObject deleteableObj)
    {
        //Remove item from the ui of deck
        RemoveFromUI(deleteableObj, "Deck");
        //Remove item from ui of swap screen
        RemoveFromUI(deleteableObj, "Swap");
        //Remove item from the actual game manager deck
        GameManager.Instance.playerDeck.Remove(deleteableObj.GetComponent<Chip>().newChip);

        CloseSwapPanel();
    }

    /// <summary>
    /// Adds card option one
    /// </summary>
    public void cardOptionOne()
    {
        //Add the card to the deck
        AddCardToDeck(shortSelection[0]);
        //Empty the storage
        ClearSelectionList();

        //Closes the card drop ui panel and opens a new panel to allow the player to swap a card
        FillCardSwap();
    }
    /// <summary>
    /// Adds card option two
    /// </summary>
    public void cardOptionTwo()
    {
        //Add the card to the deck
        AddCardToDeck(shortSelection[1]);
        //Empty the storage
        ClearSelectionList();

        //Closes the card drop ui panel and opens a new panel to allow the player to swap a card
        FillCardSwap();
    }

    /// <summary>
    /// Adds card option three
    /// </summary>
    public void cardOptionThree()
    {
        //Add the card to the deck
        AddCardToDeck(shortSelection[2]);
        //Empty the storage
        ClearSelectionList();

        //Closes the card drop ui panel and opens a new panel to allow the player to swap a card
        FillCardSwap();
    }

    /// <summary>
    /// Clears the selection list
    /// </summary>
    public void ClearSelectionList()
    {
        //Empty the list
        selectionList.Clear();
    }
    #endregion
}
