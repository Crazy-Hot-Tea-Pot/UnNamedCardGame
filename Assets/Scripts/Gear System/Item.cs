using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Gear/Item")]
public class Item : ScriptableObject
{
    public enum ItemType { Weapon, Armor, Equipment }

    public enum Teir { Base, Bronze,Silver,Gold,Platinum}

    public Teir ItemTeir = Teir.Base;    

    /// <summary>
    /// name of item.
    /// </summary>
    public string itemName;

    /// <summary>
    /// What item does
    /// </summary>
    public string itemDescription;

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

    /// <summary>
    /// The weight in decimal representing a % that the selection is weighted by. For example 0.50 would be 50% chance of droping
    /// </summary>
    public float ItemRarityWeight;

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

            ItemEquipped();
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

    [Header("Tier Values")]
    [Tooltip("How much to increase for Damage Or Shield per Tier")]
    public List<int> valueIncreaseBy = new() { 0, 2, 3, 4, 5 };

    [Tooltip("How much to increase Energy By for each Teir")]
    public List<int> energyCostIncreaseBy = new() { 0, 2, 3, 4, 5 };

    private bool isEquipped = false;

    private bool playerOwned = false;

    void OnEnable()
    {
        // Reset equipped and owned status
        isEquipped = false;
        playerOwned = false;
        ItemTeir=Teir.Base;
    }

    public void ItemActivate(PlayerController player,Enemy targetEnemy = null)
    {
        foreach (ItemEffect effect in itemEffects)
        {
            effect.Activate(player,this, targetEnemy);
        }
    }
    public void ItemEquipped()
    {
        if (isEquipped)
        {
            foreach (var effect in itemEffects)
            {
                effect.IsEquipped = IsEquipped;                
            }
        }
    }   

    public int GetEnergyCostIncreaseBy()
    {
        int tempReturnValue = 0;

        switch (ItemTeir)
        {
            case Teir.Base:
                tempReturnValue = energyCostIncreaseBy[(int)Teir.Base];
                break;
            case Teir.Bronze:
                tempReturnValue = energyCostIncreaseBy[(int)Teir.Bronze];
            break;
            case Teir.Silver:
                tempReturnValue = energyCostIncreaseBy[(int)Teir.Silver];
                break;
            case Teir.Gold:
                tempReturnValue = energyCostIncreaseBy[(int)Teir.Gold];
                break;
            case Teir.Platinum:
                tempReturnValue = energyCostIncreaseBy[(int)Teir.Platinum];
                break;
        }

        return tempReturnValue;
    }
    public int GetValueIncreaseBy()
    {
        int tempReturnValue = 0;

        switch (ItemTeir)
        {
            case Teir.Base:
                tempReturnValue = valueIncreaseBy[(int)Teir.Base];
                break;
            case Teir.Bronze:
                tempReturnValue = valueIncreaseBy[(int)Teir.Bronze];
                break;
            case Teir.Silver:
                tempReturnValue = valueIncreaseBy[(int)Teir.Silver];
                break;
            case Teir.Gold:
                tempReturnValue = valueIncreaseBy[(int)Teir.Gold];
                break;
            case Teir.Platinum:
                tempReturnValue = valueIncreaseBy[(int)Teir.Platinum];
                break;
        }

        return tempReturnValue;
    }

    [ContextMenu("Reset to Default")]
    public void ResetToDefault()
    {
        isEquipped = false;
        playerOwned = false;
        ItemTeir = Teir.Base;
        Debug.Log($"{itemName} reset to default values.");
    }
}
