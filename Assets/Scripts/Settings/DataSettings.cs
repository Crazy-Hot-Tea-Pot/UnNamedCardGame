using UnityEngine;
using static SettingsData;

[System.Serializable]
public class DataSettings
{
    //How many saves the Player can make.
    public int MaxAutoSave
    {
        get
        {
            return maxAutoSave;
        }
        set
        {
            maxAutoSave = value;
        }
    }

    [SerializeField]
    private int maxAutoSave;
    //Constructor
    public DataSettings(DataSettingsData data) {

        if(data.SettingsEdited)
            MaxAutoSave = 5;
        else
            MaxAutoSave=data.MaxAutoSaves;
    }

    /// <summary>
    /// Returns Data for saving.
    /// </summary>
    /// <returns></returns>
    public DataSettingsData GetDataToWrite()
    {
        return new DataSettingsData
        {
            SettingsEdited = true,
            MaxAutoSaves = MaxAutoSave,
        };
    }
}

