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
    public List<Quest> questList;
    public List<Quest> completeList;

    private TMP_Text textSpeaker;
    private TMP_Text textPlayer;

    //Validates Quest Complete
    public string nameTemp;

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
        //Load all the quest scriptables
        PopulateQuestSciptables();
        //We don't want to effect the originals so we are instantly copying the quest into its self so it's a list of copiess
        for (int i = 0; i < questList.Count; i++)
        {

            //Clone the value
            Quest Temp = questList[i];
            questList[i] = Instantiate(Temp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //This line tells the manager to stop if we run out of quests
        if(questList.Count != 0)
        {
            //If the current quest is complete
            if (questList[0].complete == true)
            {
                //Add to the complete list
                completeList.Add(questList[0]);
                //Remove the quest from the list
                questList.RemoveAt(0);
            }
            //If the quest is not complete then all the quests action
            else if (questList[0].complete == false)
            {
                questList[0].speaking(textSpeaker, textPlayer);
            }
        }
    }

    /// <summary>
    ///Retrieve quest information
    /// </summary>
    /// <param name="index"></param>
    /// <param name="quest"></param>
    /// <param name="description"></param>
    public void RetrieveQuestInfo(int index, TMP_Text quest, TMP_Text description)
    {
        //For some reason not having two try catches just will return null even with a != null check 
        try
        {
            quest.text = questList[index].questName;
            description.text = questList[index].questDesc;
        }
        catch
        {
            try
            {
                //If there is a value to show as a complete quest show it
                if (completeList[0] != null)
                {
                    //Name temp is a check that makes sure we aren't repeating quests this system can't check for repeats out of order but I don't think the program should ever do that
                    if (nameTemp != completeList[0].questName)
                    {
                        nameTemp = completeList[0].questName;
                        quest.text = completeList[0].questName;
                        description.text = completeList[0].questDesc;
                    }
                    else
                    {
                        quest.text = " ";
                        description.text = " ";
                    }
                }
            }
            //If there is only one quest then just hide the other text and make it nothing
            catch
            {
                quest.text = " ";
                description.text = " ";
                Debug.Log("That quest doesn't exist at index: " + index);
            }
        }
    }

    /// <summary>
    /// Retrieve the name and description of a quest based on a given index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="quest"></param>
    public void RetrieveQuestInfo(int index, TMP_Text quest)
    {
        try
        {
            quest.text = questList[index].questName;
        }
        catch
        {
            //If there is a value to show as a complete quest show it
            try
            {
                if (completeList[0] != null)
                {
                    //Name temp is a check that makes sure we aren't repeating quests this system can't check for repeats out of order but I don't think the program should ever do that
                    if (nameTemp != completeList[0].questName)
                    {
                        nameTemp = completeList[0].questName;
                        quest.text = completeList[0].questName;
                    }
                    else
                    {
                        quest.text = " ";
                    }
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

    #region Jayce Requested Features
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
        List<Quest> tempList;
        tempList = questList;
        foreach(Quest quest in completeList)
        {
            tempList.Add(quest);
        }
        return tempList;
    }

    /// <summary>
    /// Return all active quests but exclude quests that are complete or not yet active
    /// </summary>
    /// <returns></returns>
    public List<Quest> GetActiveQuests()
    {
        List<Quest> tempList;
        tempList = questList;
        return tempList;
    }

    /// <summary>
    /// Populates the quest scriptables
    /// </summary>
    public void PopulateQuestSciptables()
    {
        //Load all quests in the quest folder
        questList = new List<Quest>(Resources.LoadAll<Quest>("Scriptables/Quest"));
    }



    #endregion
}
