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

    private int maxAutoSave;
    //Constructor
    public DataSettings() {
        MaxAutoSave = 5;
    }
}

