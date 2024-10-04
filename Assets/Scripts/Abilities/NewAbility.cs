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
        buttonVar.onClick.AddListener(OnAbilityAcitve);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAbilityAcitve()
    {
        abilityButton.Activate();
    }
}
