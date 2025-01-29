using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private static QuestManager instance;

    [Tooltip("A list of all quests availiable in order")]
    public List<Quest> futureQuestList;
    public List<Quest> completeList;

    private TMP_Text textSpeaker;
    private TMP_Text textPlayer;

    public List<Quest> activeQuest;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //Load all the quest scriptables and clone them
        PopulateQuestSciptables();

        //Set Current Active Quest
        //activeQuest = futureQuestList[0];
        //futureQuestList.RemoveAt(0);
    }

    // Update is called once per frame
    void Update()
    {
        //This line tells the manager to stop if we run out of quests
        //if(activeQuest != null)
        //{
        //    //If the current quest is complete
        //    if (activeQuest.complete == true)
        //    {
        //        //Add to the complete list
        //        completeList.Add(activeQuest);
                
        //        //Change the current quest if it's not null
        //        if(futureQuestList.Count != 0)
        //        {
        //            //Make the next quest current quest
        //            activeQuest = futureQuestList[0];
        //            futureQuestList.RemoveAt(0);
        //        }
        //        //If the quest is null
        //        else
        //        {
        //            //Empty current quest
        //            activeQuest = null;
        //        }
        //    }
        //    //If the quest is not complete then all the quests action
        //    else if (activeQuest.complete == false)
        //    {
        //        activeQuest.speaking(textSpeaker, textPlayer);
        //    }
        //}
    }

    /// <summary>
    /// Complete the current Quest!! and Change the current quest to a given quest and then remove it from future quests ist 
    /// </summary>
    /// <param name="quest"></param>
    public void ChangeQuest(List<Quest> quest)
    {
        //Empty our active list
        activeQuest.Clear();
        //Loop through it
        for(int i = 0; i < 0; i++)
        {
            //Add active quests
            activeQuest.Add(quest[i]);
            //Find and remove it from future quests
            futureQuestList.Remove(quest[i]);
        }
    }


    /// <summary>
    /// Retrieve the name of a quest based on a given index from ACTIVE quest list.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="quest"></param>
    public void RetrieveQuestInfo(int index, TMP_Text quest)
    {
        try
        {
                //Give the quest a name in the mini menu
                quest.text = activeQuest[index].questName;
        }
        catch
        {
            //If there is a value to show as a complete quest show it
            try
            {
                if (completeList[0] != null)
                {
                        quest.text = completeList[0].questName;
                }
            }
            //If there is only one quest then just hide the other text and make it nothing
            catch
            {
                quest.text = " ";
                Debug.Log("That quest doesn't exist at index: " + index);
            }
        }
    }

    /// <summary>
    /// Override to retrieve the name and description of a quest based on a given index from ACTIVE quest list.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="quest"></param>
    public void RetrieveQuestInfo(int index, TMP_Text quest, TMP_Text questdesc)
    {
        //Try to add the active quest to the ui
        try
        {
            quest.text = activeQuest[index].questName;
            questdesc.text = activeQuest[index].questDesc;
        }
        //If we can't it's null there is nothing in active list
        catch
        {
            //If there is a value to show as a complete quest show it
            try
            {
                if (completeList[0] != null)
                {
                        quest.text = completeList[0].questName;
                        questdesc.text = completeList[0].questDesc;
                }
            }
            //If there is only one quest then just hide the other text and make it nothing
            catch
            {
                quest.text = " ";
                Debug.Log("That quest doesn't exist at index: " + index);
            }
        }
    }

    #region Quest Information
    /// <summary>
    /// If the quest state is false that means it is not a completed quest
    /// </summary>
    /// <param name="questName"></param>
    /// <returns></returns>
    public bool CheckQuestState(Quest QuestToCheck)
    {
        string questName = QuestToCheck.questName;
        bool questState = false;
        foreach(Quest quest in completeList)
        {
            if(quest.questName == questName)
            {
                questState = true;
               
            }
            else
            {
                questState = false;
            }
        }

        return questState;
    }

    /// <summary>
    /// Trigger quest events
    /// </summary>
    /// <param name="quest"></param>
    /// <param name="eventName"></param>
    public void TriggerQuestEvent(Quest quest, string eventName)
    {
        if(eventName == "Complete" || eventName == "complete")
        {
            quest.CompleteQuest();
        }
    }

    /// <summary>
    /// Returns all quests
    /// </summary>
    /// <returns></returns>
    public List<Quest> GetAllQuests()
    {
        List<Quest> tempList = null;
        foreach(Quest quest in activeQuest)
        {
            tempList.Add(quest);
        }
        foreach (Quest quest in futureQuestList)
        {
            tempList.Add(quest);
        }
        foreach(Quest quest in completeList)
        {
            tempList.Add(quest);
        }
        return tempList;
    }

    /// <summary>
    /// Populates the quest scriptables
    /// </summary>
    public void PopulateQuestSciptables()
    {
        //Load all quests in the quest folder
        futureQuestList = new List<Quest>(Resources.LoadAll<Quest>("Scriptables/Quest"));

        //We don't want to effect the originals so we are instantly copying the quest into its self so it's a list of copiess
        for (int i = 0; i < futureQuestList.Count; i++)
        {

            //Clone the value
            Quest Temp = futureQuestList[i];
            futureQuestList[i] = Instantiate(Temp);
        }
    }



    #endregion
}
