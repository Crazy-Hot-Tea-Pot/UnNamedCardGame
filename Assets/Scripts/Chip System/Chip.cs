using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The card class creates the basic properties of the class
/// </summary>
public class Chip : MonoBehaviour
{
    [SerializeField]
    private bool active;

    public CombatController CombatController;
    public GameObject Player;

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
    /// Chip title is the title that the card will display in game and the ID of the card.
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

    public NewChip newCard;

    void Start()
    {
        cardTitle = newCard.chipName + " Card";
        this.gameObject.name = CardTitle;

        // Set image to card
        GetComponent<Image>().sprite = newCard.chipImage;

        //Assign method to button
        imageButton = GetComponent<Button>();
        imageButton.onClick.AddListener(CardSelected);


        CombatController = GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    /// <summary>
    /// Runs Scriptable Chip
    /// </summary>
    public void CardSelected()
    {
        Debug.Log(CardTitle + " Card");
        active = true;  
        //Check if player turn to play play card
        if (CombatController.CanIMakeAction(Player))
        {
            //Check if player is jammed
            if (Player.GetComponent<PlayerController>().IsJammed)
            {
                Debug.Log("Player is Jammed");
                CombatController.TurnUsed(Player);
                Player.GetComponent<PlayerController>().JammedStacks--;
            }
            else
            {
                //Check if newCard is assigned
                if (newCard != null)
                {
                    //REMOVED AS NO LONGER NEED ENERGY TO PLAY CARD
                    //Check if player has enough energy to play card.
                    //if (newCard.energyCost > GameObject.
                    //    FindGameObjectWithTag("Player").
                    //    GetComponent<PlayerController>().Energy)
                    //{
                    //    //for when player doesn't have enough energy
                    //}
                    //else
                    //{
                    if (Player.GetComponent<PlayerController>().NextChipActivatesTwice)
                    {
                        newCard.OnChipPlayed(Player.GetComponent<PlayerController>());
                        newCard.OnChipPlayed(Player.GetComponent<PlayerController>());
                        CombatController.TurnUsed(Player);
                        //Remove effect after it has been used.
                        Player.GetComponent<PlayerController>().RemoveEffect(Effects.Effect.Motivation);
                    }
                    else
                    {
                        newCard.OnChipPlayed(Player.GetComponent<PlayerController>());
                        CombatController.TurnUsed(Player);
                    }
                    //}
                }
                else
                {
                    Debug.LogWarning("No script attached.");
                }
            }
        }
        else
        {
            Debug.Log("Not this obejcts turn yet.");
        }
    }
}
