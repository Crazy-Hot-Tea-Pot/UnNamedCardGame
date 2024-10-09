using UnityEngine;

[CreateAssetMenu(fileName = "New Trinket Ability", menuName = "Abilities/TrinketScrapVarient")]
public class TrinketScrapVarient : Ability
{
    /// <summary>
    /// How much scrap will be gained
    /// </summary>
    public int gainedScrap;

    /// <summary>
    /// This variable allows us to know if the ability has been used but is not yet ready to be active
    /// </summary>
    private bool isWaiting = false;

    /// <summary>
    /// An extra check to restrict uses. False is charged
    /// </summary>
    private bool isRecharged;

    public override void PassiveAbility()
    {
            //If not in combat
            if (!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().InCombat)
            {
                //We are no longer waiting
                isWaiting = false;
                
            }
            //If in combat
            else
            {
                //Can't be used
                isWaiting = true;
                
            }

            //If the item has a charge we can use it
            if(!isRecharged)
            {
                isRecharged = true;
                //if is waiting is true we can use the ability
                if (!isWaiting)
                {
                    Debug.Log(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Scrap + "At start");
                    //Costs the player energy
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PlayedCardOrAbility(energyCost);
                    //Give the player scrap
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Scrap += gainedScrap;
                    //Stop waiting
                    isWaiting = false;

                    Debug.Log(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Scrap + "At modified");
                }
            }
            //This part is the only reason this method doesn't run a million times while active
            //If recharge is not active again false is charged
            else if(isRecharged)
            {
                //but we are out of combat
                if(isWaiting)
                {   
                    //The we are recharged
                    isRecharged = false;
                }    
            }

        
    }
}
