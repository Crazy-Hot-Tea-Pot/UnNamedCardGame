using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    ///<summary>A variable to hold player turns that assumes the player turn is true and the enemy turn is false</summary>
    bool playerTurn;
    ///<summary>Hand limit</summary>
    int handlimit;
    ///<summary>Deck limit</summary>
    int decklimit;
    ///<summary>Draws per turn</summary>
    int drawsPerTurn;
    public List<GameObject> playerHand;
    public List<GameObject> playerDeck;

    //UIVeriables
    public GameObject Panel;
    

    // Start is called before the first frame update
    void Start()
    {
        playerTurn = true;
        handlimit = 4;
        decklimit = 20;
        drawsPerTurn = 3;
        ShufflePlayerDeck();
        DrawCard(drawsPerTurn);
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
            DrawCard(drawsPerTurn);
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
                int num1 = Roll(0, playerDeck.Count - 1);
                int num2 = Roll(0, playerDeck.Count - 1);
                //Holds the value of the next card to be replaced
                GameObject placeHolder = playerDeck[num2];
                //Replaces card 2 with card 1
                playerDeck[num2] = playerDeck[num1];
                //Replaces card 1 with the place holder (Card 2)
                playerDeck[num1] = placeHolder;
                //Clears our place holder
                placeHolder = null;
            }
        }

    }

    ///<summary>Draws a card</summary>
    public void DrawCard(int draws)
    {
        //checks the hand limit and continues if possible otherwise nothing happens
        if (playerHand.Count + draws < handlimit)
        {
            //How many cards need to be drawn
            for (int i = 0; i < draws; i++)
            {

                //take the first card on the top of the pile and add it to the players hand
                playerHand.Add(playerDeck[0]);
                playerDeck.RemoveAt(0);
                UpdateUI();
            }
        }
        //If limit reached
        else
        {
            Debug.Log("Limit Reached");
        }

    }

    ///<summary>>Generates a random number</summary
    int Roll(int max, int min)
    {
        int random = Random.Range(min, max);
        random = (int)(random * System.Environment.ProcessorCount / 16);
        Debug.Log(random);
        return random;
    }

    //A method for updating the card ui elements
    void UpdateUI()
    {
        for(int i = 0; i < playerHand.Count - 1; i++)
        {
            Instantiate(playerHand[i], Panel.transform);
        }
    }

    ///<summary>Destroys the current card</summary>
    public void cardDeath(int value)
    {
        //Remove the card from the player hand
        Destroy(playerHand[value]);
        playerHand.Remove(playerHand[value]);
       
    }
}
