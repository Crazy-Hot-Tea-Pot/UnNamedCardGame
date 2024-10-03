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


    #region InventoryUIVariables
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
    /// A variable to tell us if the inventory UI is open
    /// </summary>
    private bool isActive = false;

    /// <summary>
    /// Inventory button
    /// </summary>
    public GameObject buttonInv;

    /// <summary>
    /// Deck button
    /// </summary>
    public GameObject buttonDeck;
    #endregion

    private void Awake()
    {
        //Fills the inventory UI for deck
        fillDeck();

        //assigns player input class
        inputActions = new PlayerInputActions();
        //Assigns the input for the player to open the UI for inventory
        openInventory = inputActions.Player.InventoryUI;
        //Enables the UI
        openInventory.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //On hold open the inventroy UI if input is recieved
       if(openInventory.IsPressed())
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
        isActive = true;

    }

    /// <summary>
    /// Closes the invnetory UI if the canvas is open
    /// </summary>
    public void CloseInventory()
    {
        //If canvas is open close it
        if(uiCanvas.active == true)
        {
            if(panelDeck.active == true)
            {
                switchMenuInventory();
            }
            uiCanvas.SetActive(false);
        }

        //Tell the program the inventory is closed
        isActive = false;
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
        for(int i = 0; i < gameManager.playerDeck.Count; i++)
        {
            Instantiate(gameManager.playerDeck[i], panelDeck.transform);
        }
    }

    /// <summary>
    /// Adds a card to the UI of the player inventory. NOTE THIS HAS NOTHING TO DO WITH FUNCTIONAL DECK INVENTORY THAT'S GAMEMANAGER
    /// </summary>
    public void AddCardToDeck(GameObject card)
    {
        Instantiate(card, panelDeck.transform);
    }

    /// <summary>
    /// Adds an item to the inventory UI
    /// </summary>
    /// <param name="item"></param>
    public void AddToInventory(GameObject item)
    {
        Instantiate(item, panelInventory.transform);
    }
    /// <summary>
    /// This function just destroys anything you put in here put the card or item you want destroyed as in the UI element and it's gone. If you shove an enemy in here
    /// that's gone too.
    /// </summary>
    /// <param name="card"></param>
    public void RemoveFromUI(GameObject card)
    {
        Destroy(card);
    }
    #endregion
}
