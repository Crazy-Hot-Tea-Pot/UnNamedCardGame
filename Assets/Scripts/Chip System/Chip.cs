using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Chip : MonoBehaviour
{
    public enum ChipMode
    {
        None,
        Combat,
        WorkShop,
        Inventory,
        Delete
    }

    private ChipMode mode = ChipMode.None;

    /// <summary>
    /// What mode is the chip in.
    /// </summary>
    public ChipMode Mode
    {
        get
        {
            return mode;
        }
        set
        {
            mode = value;
        }
    }

    public CombatController CombatController;
    public GameObject Player;
    public UpgradeController UpgradeController;

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
    public Button chipButton;  

    public NewChip newChip;

    void Start()
    {
        chipTitle = newChip.chipName + " Chip";
        this.gameObject.name = ChipTitle;
        newChip.ThisChip = this.gameObject;

        Player = GameObject.FindGameObjectWithTag("Player");

        // Set image to chip
        GetComponent<Image>().sprite = newChip.chipImage;

        //When the chip is active start method will run and set it's mode
        if(Mode != ChipMode.None)
        {
            SetChipModeTo(Mode);
        }
    }
    /// <summary>
    /// Tell the Upgrade Controller this is the chip the user selected to ugprade.
    /// </summary>
    private void UpgradeChipSelected()
    {
        UpgradeController.ChipSelectToUpgrade(newChip);
    }
    /// <summary>
    /// Runs Scriptable Chip
    /// </summary>
    public void ChipSelected()
    {        
        try
        {

                // Check if there is a target available
                if (CombatController.Target == null)
                {
                    throw new NullReferenceException("No target assigned.");
                }

                //Check if player is jammed
                if (Player.GetComponent<PlayerController>().IsJammed)
                {
                    return;
                }

                //Check if newChip is assigned
                if (newChip != null)
                {
                    //Plays chip use sound
                    SoundManager.PlayFXSound(SoundFX.ChipPlayed);

                    newChip.IsActive = true;

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

                    }
                    GameManager.Instance.KillChip(this.gameObject);
                    Destroy(this.gameObject);
                }
                else
                {
                    throw new NullReferenceException("No chip script attached.");
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
    /// <summary>
    /// Set chip prefab to different mode so it can be used in multiple different enviroments.
    /// </summary>
    /// <param name="modeToBe">Mode the chip to be in.</param>
    public void SetChipModeTo(ChipMode modeToBe)
    {
        Mode = modeToBe;

        switch (modeToBe)
        {
            case ChipMode.Combat:
                CombatController = GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>();
                chipButton.interactable = true;
                chipButton.onClick.AddListener(ChipSelected);                
                newChip.IsActive = true;                
                break;
            case ChipMode.WorkShop:
                UpgradeController = GameObject.FindGameObjectWithTag("UpgradeController").GetComponent<UpgradeController>();
                chipButton.onClick.AddListener(UpgradeChipSelected);
                chipButton.interactable = true;                
                break;
            case ChipMode.Inventory:
                chipButton.interactable = false;
                break;
            case ChipMode.Delete:
                chipButton.interactable = true;
                break;
            case ChipMode.None:
            default:
                break;
        }
    }

}
