using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NewChip : ScriptableObject
{
    

    protected bool isUpgraded = false;
    protected bool isActive;   
    protected int disableCounter;
    public GameObject ThisChip;
    public enum ChipRarity
    {
        Basic,
        Common,
        Rare
    }
    /// <summary>
    /// This variable decides if the chip is isActive or inactive
    /// </summary>
    public bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            isActive = value;

            ThisChip.GetComponent<Button>().interactable = value;

            if (isActive)
                disableCounter = 0;
        }
    }
    /// <summary>
    /// Track how long a card is disabled.
    /// </summary>
    public int DisableCounter
    {
        get
        {
            return disableCounter;
        }
        set
        {
            disableCounter = value;
        }
    }
    /// <summary>
    /// Rariry of the card.
    /// </summary>
    public ChipRarity chipRarity;
    /// <summary>
    /// Name of card
    /// </summary>
    public string chipName;
    /// <summary>
    /// Description of card.
    /// </summary>
    public string description;
    /// <summary>
    /// Image for card.
    /// </summary>
    public Sprite chipImage;
    /// <summary>
    /// If this card can be upgraded.
    /// </summary>
    public bool canBeUpgraded;
    /// <summary>
    /// How much it would cost to upgrade this card
    /// </summary>
    public int costToUpgrade;
    /// <summary>
    /// Information to display to player about card.
    /// </summary>
    public string ChipTip= "<#A20000>*Error missing from Chip Sect Corp database.*</color>";

    /// <summary>
    /// If Chip is upgraded.
    /// </summary>
    public virtual bool IsUpgraded
    {
        get
        {
            return isUpgraded;
        }
        set
        {
            if(canBeUpgraded)
                isUpgraded = value;
            else
            {
                Debug.Log("This card can't be upgraded.");
            }
        }
    }

    /// <summary>
    /// This chip hits all enemies
    /// </summary>
    public bool hitAllTargets;

    public virtual void OnChipPlayed(PlayerController player)
    {
        Debug.Log(chipName + " played.");
    }

    public virtual void OnChipPlayed(PlayerController player, Enemy Target)
    {
        if(!IsActive)
        Debug.Log(chipName + " played.");
    }

    /// <summary>
    /// Any action chip needs to do at end of Turn.
    /// </summary>
    public virtual void EndRound()
    {
        if (IsActive)
        {
            DisableCounter++;

            if (DisableCounter >= 2)
            {
                IsActive = true;
            }
        }
    }

}
