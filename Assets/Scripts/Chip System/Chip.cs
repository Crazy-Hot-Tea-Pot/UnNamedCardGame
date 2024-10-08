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

    private string chipTitle;    

    /// <summary>
    /// Chip title is the title that the card will display in game and the ID of the card.
    /// </summary>
    public string ChipTitle
    {
        get
        {
            return chipTitle;
        }
        private set
        {
            chipTitle = value;
        }
    }

    /// <summary>
    /// Button Component so player can click on card
    /// </summary>
    private Button imageButton;  

    public NewChip newChip;

    void Start()
    {
        chipTitle = newChip.chipName + " Chip";
        this.gameObject.name = ChipTitle;

        // Set image to card
        GetComponent<Image>().sprite = newChip.chipImage;

        //Assign method to button
        imageButton = GetComponent<Button>();
        imageButton.onClick.AddListener(ChipSelected);


        CombatController = GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    /// <summary>
    /// Runs Scriptable Chip
    /// </summary>
    public void ChipSelected()
    {
        Debug.Log(ChipTitle + " Chip");
        active = true;  
        //Check if player turn to play play card
        if (CombatController.CanIMakeAction(Player))
        {
            //Check if player is jammed
            if (Player.GetComponent<PlayerController>().IsJammed)
            {
                Debug.Log("Player is Jammed");
                CombatController.TurnUsed(Player);             
            }
            else
            {
                //Check if newChip is assigned
                if (newChip != null)
                {
                    //REMOVED AS NO LONGER NEED ENERGY TO PLAY CARD
                    //Check if player has enough energy to play card.
                    //if (newChip.energyCost > GameObject.
                    //    FindGameObjectWithTag("Player").
                    //    GetComponent<PlayerController>().Energy)
                    //{
                    //    //for when player doesn't have enough energy
                    //}
                    //else
                    //{
                    if (Player.GetComponent<PlayerController>().NextChipActivatesTwice)
                    {
                        newChip.OnChipPlayed(Player.GetComponent<PlayerController>(), CombatController.Target.GetComponent<Enemy>());
                        newChip.OnChipPlayed(Player.GetComponent<PlayerController>(), CombatController.Target.GetComponent<Enemy>());
                        CombatController.TurnUsed(Player);
                        //Remove effect after it has been used.
                        Player.GetComponent<PlayerController>().RemoveEffect(Effects.Effect.Motivation);
                    }
                    else
                    {
                        newChip.OnChipPlayed(Player.GetComponent<PlayerController>(), CombatController.Target.GetComponent<Enemy>());
                        CombatController.TurnUsed(Player);
                    }
                    //}
                    //After card has been used add to kill cards and destroy.
                    GameManager.Instance.KillChip(this.gameObject);
                    Destroy(this.gameObject);
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
