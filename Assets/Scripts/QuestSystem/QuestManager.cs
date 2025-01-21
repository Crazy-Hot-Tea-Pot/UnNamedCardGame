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
}
