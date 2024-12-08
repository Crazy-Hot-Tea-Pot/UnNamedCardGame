using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gear : MonoBehaviour
{
    public Item Item;
    public CombatController CombatController;
    public GameObject Player;
    public Button Button;

    public string GearName
    {
        get
        {
            return gearName;
        }
        set
        {
            gearName = value;
        }
    }    

    private string gearName;    

    // Start is called before the first frame update
    void Start()
    {

        Player = GameObject.FindGameObjectWithTag("Player");

        // Set image to chip
        GetComponent<Image>().sprite = Item.itemImage;

        Button.onClick.AddListener(() =>  UseItem() );
    }

    public void UseItem()
    {
        try
        {
            // Check if there is a target available
            if (CombatController.Target == null)
                throw new NullReferenceException("No target assigned.");

            if (Item == null)
                throw new NullReferenceException("No Item equipped.");
            else
            {
                Item.ItemActivate();
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
    public void EquipItem(Item newItem)
    {
        //Unequip Item
        Item.IsEquipped = false;
        Item = null;

        //Equip new newItem
        Item = newItem;
        Item.IsEquipped = true;
        GearName = Item.itemName;
        this.gameObject.name = GearName;
    }
}
