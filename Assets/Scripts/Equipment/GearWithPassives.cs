using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Equipment/PassiveGear")]
public class GearWithPassives : Equipment
{
    /// <summary>
    /// Make empty game objects and stuff scriptables on them like the abilites in the UI
    /// </summary>
    public List<GameObject> AbilityList;
    private bool isEquipted = false;
    public override void ActivateEquipmnet()
    {
        //Does the method base first
        base.ActivateEquipmnet();

        //If being equiped
        if (isEquipted == false)
        {
            //Make these scriptables exist
            foreach(GameObject obj in AbilityList)
            {
                //Check the object does not exist
                if(!GameObject.FindGameObjectWithTag(obj.tag))
                {
                    Instantiate(obj);
                }
            }
            //Tell me it should be equipted
            Debug.Log(itemName + " is equiped");
            //Mark it as equiped
            isEquipted = true;
        }
        //If being unequipted
        else
        {
            //Delete abilites in the item matching the tag
            foreach(GameObject obj in AbilityList)
            {
                Destroy(GameObject.FindGameObjectWithTag(obj.tag));
            }
            //Tell me it shoud be unequipted
            Debug.Log(itemName + "Unequipted");
            //Mark as unequipted
            isEquipted = false;
        }

    }
}
