using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDrop : MonoBehaviour
{

    public PlayerUIManager uiManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DropCard()
    {
            //Remove the card based on the targets name in the appropriate parent
            //uiManager.RemoveFromUI(GameObject.Find(this.name), "Card");
    }
}
