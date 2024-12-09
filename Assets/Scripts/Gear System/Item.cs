using Microsoft.Unity.VisualStudio.Editor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Gear/Item")]
public class Item : ScriptableObject
{
    public enum ItemType { Weapon, Armor, Equipment }

    /// <summary>
    /// name of item.
    /// </summary>
    public string itemName;

    /// <summary>
    /// What item does
    /// </summary>
    public string itemDescription;

    /// <summary>
    /// Tip for player about item.
    /// </summary>
    public string itemTip;

    /// <summary>
    /// Image of item.
    /// </summary>
    public Sprite itemImage;

    /// <summary>
    /// What kind of itemType is the item.
    /// </summary>
    public ItemType itemType;

    /// <summary>
    /// List of itemEffects the item has.
    /// </summary>
    public List<ItemEffect> itemEffects;
    public bool IsEquipped
    {
        get
        {
            return isEquipped
;
        }
        set
        {
            isEquipped = value;

            AppleEffect();
        }
    }    

    public bool IsPlayerOwned
    {
        get
        {
            return playerOwned;
        }
        set
        {
            playerOwned = value;
        }
    }

    private bool isEquipped = false;

    private bool playerOwned = false;
    void OnEnable()
    {
        // Reset equipped and owned status
        isEquipped = false;
        playerOwned = false;
    }
    public void ItemActivate()
    {

    }
    private void AppleEffect()
    {
        //If false remove the passive effect from player.
        if (isEquipped)
        {
            switch (itemType)
            {
                case ItemType.Equipment:
                    foreach (ItemEffect effect in itemEffects)
                    {
                        if (effect.IsPassiveEffect)
                            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ApplyEffect(effect.effectToApply);
                    }
                    break;
            }
        }
        else
        {
            switch (itemType)
            {
                case ItemType.Equipment:
                    foreach (ItemEffect effect in itemEffects)
                    {
                        if (effect.IsPassiveEffect)
                            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().RemoveEffect(effect.effectToApply);
                    }
                    break;
            }
        }
    }

    [ContextMenu("Reset to Default")]
    public void ResetToDefault()
    {
        isEquipped = false;
        playerOwned = false;
        Debug.Log($"{itemName} reset to default values.");
    }
}
