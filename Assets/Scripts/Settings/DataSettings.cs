public class DataSettings
{
    private int maxAutoSave = 5;

    /// <summary>
    /// How many saves the player can make.
    /// </summary>
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
}

