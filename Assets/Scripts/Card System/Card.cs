using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The card class creates the basic properties of the class
/// </summary>
public class Card : MonoBehaviour
{
    [SerializeField]
    private bool active;

    /// <summary>
    /// This variable decides if the card is active or inactive
    /// </summary>
    public bool IsActive
    {
        get
        {
            return active;
        }
    }

    private string cardTitle;    

    /// <summary>
    /// Card title is the title that the card will display in game and the ID of the card.
    /// </summary>
    public string CardTitle
    {
        get
        {
            return cardTitle;
        }
    }

    /// <summary>
    /// Button Component so player can click on card
    /// </summary>
    private Button imageButton;  

    public NewCard newCard;

    void Start()
    {
        cardTitle = newCard.cardName + " Card";
        this.gameObject.name = CardTitle;

        // Set image to card
        GetComponent<Image>().sprite = newCard.cardImage;

        //Assign method to button
        imageButton = GetComponent<Button>();
        imageButton.onClick.AddListener(CardSelected);
        
    }
    /// <summary>
    /// Runs Scriptable Card
    /// </summary>
    public void CardSelected()
    {
        Debug.Log(CardTitle + " Card");
        active = true;  
        //Check if player can play card
        if (GameObject.FindGameObjectWithTag("CombatController").
            GetComponent<CombatController>().
            CanIMakeAction(GameObject.FindGameObjectWithTag("Player")))
        {
            //Check if newCard is assigned
            if (newCard != null)
            {
                //Check if player has enough energy to play card.
                if (newCard.energyCost > GameObject.
                    FindGameObjectWithTag("Player").
                    GetComponent<PlayerController>().Energy)
                {
                    //for when player doesn't have enough energy
                }
                else
                {
                    newCard.OnCardPlayed();
                    GameObject.FindGameObjectWithTag("CombatController").
                    GetComponent<CombatController>().TurnUsed(GameObject.FindGameObjectWithTag("Player"));
                }
            }
            else
            {
                Debug.LogWarning("No script attached.");
            }
        }
        else
        {
            Debug.Log("Not this obejcts turn yet.");
        }
    }
}
