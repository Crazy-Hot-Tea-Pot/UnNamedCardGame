using UnityEngine;

[CreateAssetMenu(fileName = "New Chip", menuName = "Chip System/New Chip")]
public class NewChip : ScriptableObject
{
    protected bool isUpgraded;
    public enum ChipRarity
    {
        Basic,
        Common,
        Rare
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
        // This will be overridden by specific card types
        Debug.Log(chipName + " played.");
        //GameObject.FindGameObjectWithTag("Player").
        //    GetComponent<PlayerController>().
        //    PlayedCardOrAbility(energyCost);
    }
}
