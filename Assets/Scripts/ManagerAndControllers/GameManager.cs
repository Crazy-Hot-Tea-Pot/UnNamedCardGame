using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameManager gameManager;
    public PlayerUIManager uiManager;


    ///<summary>A variable to hold player turns that assumes the player turn is true and the enemy turn is false</summary>
    bool playerTurn;
    ///<summary>Hand limit</summary>
    public int handlimit;
    ///<summary>Deck limit</summary>
    public int decklimit;
    ///<summary>Draws per turn</summary>
    public int drawsPerTurn;
    /// <summary>
    /// Is the player in combat true is yes
    /// </summary>
    //private bool inCombat;
    //Since this is player combat i moved it to the player class.

    /// <summary>
    /// Is the player in combat true is yes
    /// </summary>
    public bool InCombat
    {
        get
        {
            return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().InCombat;
        }
    }
    public List<GameObject> playerHand;
    public List<GameObject> playerDeck;
    public List<GameObject> enemyList;
    public List<GameObject> usedChips;

    //UIVeriables
    public GameObject panel;
    public GameObject uiCanvas;


    public static GameManager Instance
    {
        get;
        private set;
    }
    /// <summary>
    /// Gets Draws per turn amount.
    /// </summary>
    public int DrawsPerTurn
    {
        get
        {
            return drawsPerTurn;
        }
        private set
        {
            drawsPerTurn = value;
        }
    }

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Makes sure we have a valid number
        if(handlimit < DrawsPerTurn + 1)
        {
            handlimit = DrawsPerTurn + 1;
        }
        playerTurn = true;
        ShufflePlayerDeck();
        DrawCard(DrawsPerTurn);

    }

    // Update is called once per frame
    void Update()
    {
        //Remove this later it represents AI that doesn't exist
        if(playerTurn == false)
        {
            playerTurn = ChangeTurn(playerTurn);
        }
    }

    ///<summary>Change turn is a method that allows the turn to change whenever it's needed. It assumes true is our player and false is the enemy</summary>
    bool ChangeTurn(bool turn)
    {
        //Switch to enmy turn
        if(turn == true)
        {
            turn = false;
        }
        //Switch to player turn
        else if(turn == false)
        {
            turn = true;
            //Draw one card
            DrawCard(DrawsPerTurn);
        }
        return turn;
    }

    ///<summary>Shuffles the player deck</summary>
    public void ShufflePlayerDeck()
    {
        //Makes sure there are atleast 3 cards and not null
        if(playerDeck.Count != 0 && playerDeck.Count != 1 && playerDeck != null)
        {
            //Cycles through the player deck
            for (int i = 0; i < playerDeck.Count - 1; i++)
            {
                //Collects random number for our cards
                int num1 = Roll(0, playerDeck.Count);
                int num2 = Roll(0, playerDeck.Count);
                //Holds the value of the next card to be replaced
                GameObject placeHolder = playerDeck[num2-1];
                //Replaces card 2 with card 1
                playerDeck[num2-1] = playerDeck[num1-1];
                //Replaces card 1 with the place holder (Chip 2)
                playerDeck[num1-1] = placeHolder;
                //Clears our place holder
                placeHolder = null;
            }
        }

    }

    ///<summary>Draws a card</summary>
    public void DrawCard(int draws)
    {        
            //How many cards need to be drawn
        for (int i = 0; i < draws; i++)
        {

            //checks the hand limit and continues if possible otherwise nothing happens
            if (playerHand.Count + 1 < handlimit)
            {
                //take the first card on the top of the pile and add it to the players hand
                playerHand.Add(playerDeck[0]);
                playerDeck.RemoveAt(0);
            }
            //If limit reached
            else
            {
                Debug.LogWarning("Limit Reached");
            }
        }
         UpdateUI();       

    }

    ///<summary>>Generates a random number</summary
    int Roll(int max, int min)
    {
        int random = Random.Range(min, max);
        return random;
    }

    //A method for updating the card ui elements
    void UpdateUI()
    {
        for(int i = 0; i < playerHand.Count; i++)
        {
            Instantiate(playerHand[i], panel.transform);
        }
    }

    ///<summary>Destroys the current card</summary>
    public void CardDeath(int value)
    {
        //Remove the card from the player hand
        playerHand.Remove(playerHand[value]);
        //Remove the card from the InventoryUI
        uiManager.RemoveFromUI(playerHand[value], uiManager.panelDeck);
    }

    /// <summary>
    /// A method that can be used to transition into combat when out of combat
    /// </summary>
    public void StartCombat()
    {
        //Enables combat UI
        uiCanvas.SetActive(true);
        //Resets player turn
        playerTurn = true;
        //Enables Combat
        //inCombat = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().InCombat = true;        
    }

    /// <summary>
    /// A method to transition out of combat
    /// </summary>
    public void EndCombat()
    {
        //Deactivates the UI for combat
        uiCanvas.SetActive(false);
        //Disables combat
        //inCombat = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().InCombat = false;
    }

    /// <summary>
    /// Adds enemies to a list
    /// </summary>
    public void RememberEnemy(GameObject enemy)
    {
        //A for each loop to chceck if the element exists in the list
        bool tempCleared = true;
        foreach (GameObject tempCheck in enemyList)
        {
            if (enemy.name == tempCheck.name)
            {
                tempCleared = false;
            }
        }
        //Adds it to the list if cleared
        if (tempCleared == true)
        {
            //This adds enemies to the enemy list
            enemyList.Add(enemy);
        }

        //Had to put this here cause this is after all enemies are added @_@
        GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>().StartCombat();
    }

    /// <summary>
    /// Picks up a card and adds it to the deck aswell as deck inventory. This method needs first a card and second the object to be deleted because card objects
    /// Are UI specific
    /// </summary>
    /// <param name="card"></param>
    public void PickUpCard(GameObject card, GameObject deleteOb)
    {
        //If the player deck isn't at limit
        if(playerDeck.Count !< decklimit)
        {
            //Destroy the card from the game world
            Destroy(deleteOb);
            //Add card to deck
            playerDeck.Add(card);
            //Add to the inventory UI
            uiManager.AddCardToDeck(card);
  
        }
        else
        {
            //In the future play a sound or have a visual effect for can't pick it up
            Debug.Log("Can't pick it up");
        }
        
    }

    /// <summary>
    /// TO FIX
    /// have this called either at end of turn or after a card has been used.
    /// Retry foreach loop to remove correct chip.
    /// </summary>
    /// <param name="card"></param>
    public void KillCard(GameObject card)
    {
        usedChips.Add(card);
        
        foreach(GameObject temp in playerHand)
        {
            if(temp.GetComponent<Chip>().newCard.chipName == card.GetComponent<Chip>().newCard.chipName)
            {
                playerHand.Remove(temp);
            }
        }
        DrawCard(DrawsPerTurn);
    }
}
