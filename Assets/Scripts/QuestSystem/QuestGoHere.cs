using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestGoHere", menuName = "Quest/QuestSystem/GoHere")]
public class QuestGoHere : Quest
{
    [Tooltip("The position of the player when this quest object is complete")]
    public GameObject questEndPosition;

    private bool canEnd = false;
    //Override the Run quest method
    public override void RunQuest()
    {
        if (!complete)
        {

            if (questEndPosition == null)
            {
                Debug.Log("Quest has no end position and can not continue. I have completed the quest");
                CompleteQuest();
            }
            else
            {
                //if collided with player
                if(canEnd)
                {
                    CompleteQuest();
                }
            }
        }
    }

    //Overide complete quest to delete the queset object
    public override void CompleteQuest()
    {
        //Use the base of the method
        base.CompleteQuest();

        try
        {
            GameObject.Find(questEndPosition.name);
        }
        catch
        {
            Debug.Log("Could not find " + questEndPosition.name);
        }
    }

    //Pass through can end to show collision has been made in QuestCollison
    public override void TouchPassThrough()
    {
        canEnd = true;
    }
}

