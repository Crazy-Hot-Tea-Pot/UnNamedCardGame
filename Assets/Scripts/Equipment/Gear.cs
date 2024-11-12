using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Equipment/Gear")]
public class Gear : Equipment
{
    public List<Ability> AbilityList;
    private bool isEquipted = false;
    public override void ActivateEquipmnet()
    {
        //Does the method base first
        base.ActivateEquipmnet();
        
        //If being equiped
        if(isEquipted == false)
        {
            //Loop through the UI and add abilites
            for(int i = 0; i < AbilityList.Count; i++)
            {
                GameObject.Find("PlayerUIManager").GetComponent<PlayerUIManager>().MakeActiveAbility(AbilityList[i]);
            }

            //Tell me it should be equipted
            Debug.Log(itemName + " is equiped");
            //Mark it as equiped
            isEquipted = true;
        }
        //If being unequipted
        else
        {
            //Loop through and remove each UI element
            for (int i = 0; i < AbilityList.Count; i++)
            {
                GameObject.Find("PlayerUIManager").GetComponent<PlayerUIManager>().MakeDeactiveAbility(AbilityList[i]);
            }

            //Tell me it shoud be unequipted
            Debug.Log(itemName + "Unequipted");
            //Mark as unequipted
            isEquipted = false;
        }

    }
}
