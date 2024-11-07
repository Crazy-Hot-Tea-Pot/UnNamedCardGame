using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewAbility : MonoBehaviour
{

    //Holds the button
    private Button buttonVar;

    //Holds the class for ability
    public Ability abilityButton;
    // Start is called before the first frame update
    void Start()
    {
        //Assign method to button
        buttonVar = GetComponent<Button>();
        //Creates a listener that triggers the onabilityactive method when pressed
        try
        {
            buttonVar.onClick.AddListener(OnAbilityAcitve);
        }
        catch
        {
            Debug.Log("Ability is passive no button");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Allows for abilites with passive function to function
        abilityButton.PassiveAbility();
    }

    private void OnAbilityAcitve()
    {
        //Check if player turn to play play card
        if (!GameObject.Find("CombatController").GetComponent<CombatController>().PlayerUsedAbility)
        {
            GameObject.Find("CombatController").GetComponent<CombatController>().PlayerUsedAbility = true;
            //Allows for abilites activated on press to function
            abilityButton.Activate();
        }
    }
}
