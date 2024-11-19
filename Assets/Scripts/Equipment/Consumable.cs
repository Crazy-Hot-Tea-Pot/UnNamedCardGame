using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Equipment/Consumable")]
public class Consumable : Equipment
{
    /// <summary>
    /// The UI object that must be removed
    /// </summary>
    public GameObject UIObject;
    /// <summary>
    /// How much Health this consumable restores
    /// </summary>
    public int restoreHP;
    /// <summary>
    /// How much energy this consumable restores
    /// </summary>
    public int restoreEnergy;
    /// <summary>
    /// How much shield this consumable restores
    /// </summary>
    public int restoreShield;

    public override void ActivateEquipmnet()
    {
        //Does the method base first
        base.ActivateEquipmnet();

        //Find the player controller and in order add shield, add Health and add energy based on our variables above
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ApplyShield(restoreShield);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Heal(restoreHP);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().RecoverEnergy(restoreEnergy);

        //Delete the object
        destroyme = true;
        //Remove from invenotry UI
        GameObject.Find("PlayerUIManager").GetComponent<PlayerUIManager>().RemoveFromUI(UIObject, "Inventory");
        //Remove item from the actual inventory
        GameObject.Find("PlayerUIManager").GetComponent<PlayerUIManager>().InventoryList.Remove(UIObject);


    }
}
