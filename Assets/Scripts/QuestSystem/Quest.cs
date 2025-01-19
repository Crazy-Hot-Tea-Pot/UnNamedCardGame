using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quest : ScriptableObject
{
    public string name;
    public bool complete = false;

    public void speaking(TMP_Text speaker, TMP_Text player)
    {
        RunQuest();
    }

    /// <summary>
    /// A method intended to be overwritten to run quests and meet their requirments
    /// </summary>
    public virtual void RunQuest()
    {

    }

    ///<summary>
    ///This method is always over written but allows for mono objects using collision detection to pass useful data script by script
    ///</summary>
    public virtual void TouchPassThrough()
    {

    }

    /// <summary>
    /// A method intended to complete quests in some cases it can be overwritten
    /// </summary>
    public virtual void CompleteQuest()
    {
        complete = true;
        name = name + " (Complete)";
        Debug.Log("Quest Complete: " + name);
    }

}

