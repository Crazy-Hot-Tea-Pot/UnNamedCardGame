using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for drops in game.
/// </summary>
[System.Serializable]
public class Drop
{
    // The name of the item (e.g., "Scrap", "Shiv", "Chip")
    public string dropName;
    // The type of the item (e.g., "Scrap", "Weapon", "Chip")
    public string dropType;
    // The amount of the item dropped (for items like Scrap)
    public int quantity;

    // Constructor to initialize the Drop
    public Drop(string itemName, string itemType, int quantity)
    {
        this.dropName = itemName;
        this.dropType = itemType;
        this.quantity = quantity;
    }

    public override string ToString()
    {
        return $"{dropName} ({dropType}) - Quantity: {quantity}";
    }
}
