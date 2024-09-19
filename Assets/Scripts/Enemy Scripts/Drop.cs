/// <summary>
/// Class for drops in game.
/// </summary>
[System.Serializable]
public class Drop
{
    private int energyCost;
    private int damage;
    private int amount;

    public int EnergyCost
    {
        get { return energyCost; }
    }
    public int Damage
    {
        get { return damage; }
    }

    // The name of the item (e.g., "Scrap", "Shiv", "Chip")
    public string dropName;
    // The type of the item (e.g., "Scrap", "Weapon", "Chip")
    public enum DropType { Weapon, Scrap };

    public DropType dropType;
    // The amount of the item dropped (for items like Scrap)
    public int Amount
    {
        get
        {
            return amount;
        }
    }

    // Constructor to initialize the Drop
    public Drop()
    {
    }
    /// <summary>
    /// Create Drop Type Weapon
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="energyCost"></param>
    /// <param name="damage"></param>
    public Drop(string itemName,DropType dropType, int energyCost, int damage)
    {
        dropName = itemName;
        this.dropType = dropType;
        this.energyCost = energyCost;
        this.damage = damage;
    }
    /// <summary>
    /// Create Scrap type drop
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="type"></param>
    /// <param name="amount"></param>
    public Drop(DropType type,int amount)
    {
        this.dropType = type;
        this.amount = amount;
    }    
}
