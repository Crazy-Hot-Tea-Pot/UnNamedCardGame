using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewEquipment : MonoBehaviour
{

    //Holds the button
    private Button buttonVar;

    /// <summary>
    /// Holds the button that activates on press
    /// </summary>
    public Equipment equipmentButton;


    // Start is called before the first frame update
    void Start()
    {
        //Assign method to button
        buttonVar = GetComponent<Button>();
        //Creates a listener that triggers the onabilityactive method when pressed
        buttonVar.onClick.AddListener(DisplayAbilities);        
    }

    // Update is called once per frame
    void Update()
    {
        //Destroys consumable objects
        if(equipmentButton.destroyme)
        {
            Destroy(this.gameObject);
        }
    }

    public void DisplayAbilities()
    {
        //On button press
        equipmentButton.ActivateEquipmnet();
    }
}
