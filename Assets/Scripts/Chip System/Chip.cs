using System;
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
    private bool isInWorkShop=false;

    [SerializeField] 
    private bool isInInventoryChip=false;

    public CombatController CombatController;
    public GameObject Player;
    public UpgradeController UpgradeController;
    

    //Make chip know its not an clickable chip
    public bool IsInInventoryChip
    {
        get
        {
            return isInInventoryChip;
        }
        set
        {
            isInInventoryChip = value;
        }
    }
    public bool IsInWorkShop
    {
        get
        {
            return isInWorkShop;
        }
        set
        {
            isInWorkShop = value;
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
        newChip.ThisChip = this.gameObject;

        // Set image to card
        GetComponent<Image>().sprite = newChip.chipImage;        

        if (IsInInventoryChip)
        {
            GetComponent<Button>().interactable = false;
        }
        else if (IsInWorkShop)
        {
            //Assign method to button
            imageButton = GetComponent<Button>();
            imageButton.onClick.AddListener(UpgradeChipSelected);
            imageButton.interactable = true;

            UpgradeController = GameObject.FindGameObjectWithTag("UpgradeController").GetComponent<UpgradeController>();
        }
        else
        {
            //Assign method to button
            imageButton = GetComponent<Button>();
            imageButton.onClick.AddListener(ChipSelected);

            CombatController = GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>();
            Player = GameObject.FindGameObjectWithTag("Player");

            newChip.IsActive = true;
        }        
    }
    private void UpgradeChipSelected()
    {
        UpgradeController.ChipSelectToUpgrade(newChip);
    }
    /// <summary>
    /// Runs Scriptable Chip
    /// </summary>
    public void ChipSelected()
    {
        Debug.Log(ChipTitle + " Chip");
        newChip.IsActive = true;
        try
        {
            //Check if player turn to play play card
            if (!CombatController.PlayerUsedChip)
            {

                // Check if there is a target available
                if (CombatController.Target == null)
                {
                    throw new NullReferenceException("No target assigned.");
                }

                //Check if player is jammed
                if (Player.GetComponent<PlayerController>().IsJammed)
                {
                    //CombatController.TurnUsed(Player);
                    return;
                }

                //Check if newChip is assigned
                if (newChip != null)
                {
                    if (Player.GetComponent<PlayerController>().NextChipActivatesTwice)
                    {
                        if (newChip is DefenseChip defenseChip)
                            newChip.OnChipPlayed(Player.GetComponent<PlayerController>());
                        else
                        {
                            if (newChip.hitAllTargets)
                            {
                                // Looping to attack twice
                                for (int i = 0; i < 2; i++)
                                {
                                    foreach (GameObject target in GameManager.Instance.enemyList)
                                    {
                                        newChip.OnChipPlayed(Player.GetComponent<PlayerController>(), target.GetComponent<Enemy>());
                                    }
                                }
                            }
                            else
                            {
                                newChip.OnChipPlayed(Player.GetComponent<PlayerController>(), CombatController.Target.GetComponent<Enemy>());
                                newChip.OnChipPlayed(Player.GetComponent<PlayerController>(), CombatController.Target.GetComponent<Enemy>());
                            }
                        }

                        //Player made chip move
                        CombatController.PlayerUsedChip = true;

                        //Remove effect after it has been used.
                        Player.GetComponent<PlayerController>().RemoveEffect(Effects.Effect.Motivation);
                    }
                    else
                    {
                        if (newChip is DefenseChip defenseChip)
                            newChip.OnChipPlayed(Player.GetComponent<PlayerController>());
                        else
                        {
                            if (newChip.hitAllTargets)
                            {
                                foreach (GameObject target in GameManager.Instance.enemyList)
                                {
                                    newChip.OnChipPlayed(Player.GetComponent<PlayerController>(), target.GetComponent<Enemy>());
                                }
                            }
                            else
                                newChip.OnChipPlayed(Player.GetComponent<PlayerController>(), CombatController.Target.GetComponent<Enemy>());
                        }

                        //Player made chip move
                        CombatController.PlayerUsedChip = true;
                    }
                    GameManager.Instance.KillChip(this.gameObject);
                    Destroy(this.gameObject);
                }
                else
                {
                    throw new NullReferenceException("No chip script attached.");
                }
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.LogWarning($"Null reference error: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Generic catch for any other exceptions that may occur
            Debug.LogError($"An unexpected error occurred: {ex.Message}");
        }
    }    
}
