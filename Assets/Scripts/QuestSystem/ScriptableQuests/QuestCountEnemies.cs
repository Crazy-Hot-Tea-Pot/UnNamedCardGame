using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestCountEnemies", menuName = "Quest/QuestCountEnemies")]
public class QuestCountEnemies : Quest
{
    public int totalEnemies;
    private int remainingEnemies;
    public string enemyName;

    private string ccQuestName;
    private string ccQuestDesc;

    private void Awake()
    {
        remainingEnemies = totalEnemies;
        ccQuestDesc = questDesc;
        ccQuestName = questName;
    }
    public override void RunQuest()
    {

        //Update the description
        questDesc = ccQuestDesc + "Kill " + enemyName + " (" + remainingEnemies + "/" + totalEnemies + ")";
        //Update the name
        questName = ccQuestName + "Kill " + enemyName + " (" + remainingEnemies + "/" + totalEnemies + ")";

        //If remaining enemies is equal to 0 complete the quest
        if (remainingEnemies == 0)
        {
            questDesc = ccQuestDesc;
            questName = ccQuestName;
            CompleteQuest();
        }

    }

    /// <summary>
    /// When an enemy dies hand the counter quest it's name if it's the right enemy incriment the counter. IMPORTANT!! I highly recomend using this in a try catch.
    /// </summary>
    /// <param name="name">A varaible to compare against the saved name in the quest. This should be the exact name of the enemy type you are looking for example Looter not Looter(clonen)</param>
    public override void EnemyQuestCounterUpdate(string enemyTypeName)
    {
        //If the name matches
        if(enemyTypeName == enemyName)
        {
            //Take away from the counter
            remainingEnemies -= 1;
        }
    }
}
