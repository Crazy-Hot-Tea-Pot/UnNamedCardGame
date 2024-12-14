using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class GearManager : MonoBehaviour
{
    public static GearManager Instance
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

    /// <summary>
    /// What the player starts with in a new game.
    /// </summary>
    [SerializeField]
    private List<Item> startingGear = new List<Item>();

    /// <summary>
    /// Making it read only to prevent future problems
    /// </summary>
    public IReadOnlyList<Item> StartingGear => startingGear;

    /// <summary>
    /// List of Gear the player current has.
    /// Max is 10.
    /// </summary>
    public List<Item> PlayerCurrentGear = new List<Item>();

    /// <summary>
    /// All gear in the game. Not what the player has.
    /// </summary>
    public List<Item> AllGear = new List<Item>();

    private static GearManager instance;
    [SerializeField]
    private int gearLimit = 10;

    void Awake()
    {
        if (Instance == null)
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
        LoadAllGear();
        UpdateGear();

        GameManager.Instance.OnStartCombat += StartCombat;
        GameManager.Instance.OnEndCombat += EndCombat;
        GameManager.Instance.OnSceneChange += SceneChange;
    }
    /// <summary>
    /// Updates the list of playercurrentgear.
    /// </summary>
    public void UpdateGear()
    {
        PlayerCurrentGear.Clear();

        foreach (Item item in AllGear)
        {
            if (item.IsEquipped || item.IsPlayerOwned)
            {
                PlayerCurrentGear.Add(item);
            }
        }
    }
    /// <summary>
    /// Add item to player gear.
    /// </summary>
    /// <param name="newItem"></param>
    public bool Acquire(Item newItem)
    {
        if (newItem == null)
            return false;

        // Check if the maximum limit is reached
        if (PlayerCurrentGear.Count >= gearLimit)
        {
            return false;
        }

        if (!newItem.IsPlayerOwned)
        {
            newItem.IsPlayerOwned = true;           
        }

        // Add the item regardless of duplicates, as long as the limit is not exceeded
        PlayerCurrentGear.Add(newItem);

        return true;

    }
    /// <summary>
    /// Remove item from player inventory
    /// </summary>
    /// <param name="itemToRemove"></param>
    public void RemoveItem(Item itemToRemove)
    {
        if(itemToRemove == null) 
            return;

        itemToRemove.IsPlayerOwned = false;
        itemToRemove.IsEquipped = false;
    }
    /// <summary>
    /// Return all gear by itemType
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<Item> GetIAllGearByType(Item.ItemType type)
    {
        return AllGear.FindAll(item => item.itemType == type);
    }
    /// <summary>
    /// Equip gear.
    /// Check if the item itemType is already equipped.
    /// </summary>
    /// <param name="item"></param>
    public bool EquipGear(Item item)
    {
        if (item == null)
        {
            return false;
        }

        // Check if an item of the same itemType is already equipped
        foreach (Item itemCheck in PlayerCurrentGear)
        {
            if (itemCheck.itemType == item.itemType && itemCheck.IsEquipped)
            {
                // Unequip the currently equipped item
                itemCheck.IsEquipped = false;
                break;
            }
        }

        // Equip the new item
        item.IsEquipped = true;

        // Update gear lists
        UpdateGear();

        return true;
    }
    /// <summary>
    /// Unequip an item of a specific itemType.
    /// </summary>
    public bool UnequipItem(Item.ItemType type)
    {
        foreach (Item item in PlayerCurrentGear)
        {
            if (item.itemType == type && item.IsEquipped)
            {
                item.IsEquipped = false;

                // Update gear lists
                UpdateGear();

                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Get currently equipped item by itemType.
    /// </summary>
    public Item GetEquippedItem(Item.ItemType type)
    {
        foreach (Item item in PlayerCurrentGear)
        {
            if (item.itemType == type && item.IsEquipped)
            {
                return item;
            }
        }
        return null;
    }
    /// <summary>
    /// Load all gear from scriptables
    /// </summary>
    private void LoadAllGear()
    {
        // Load all Item ScriptableObjects in the Resources folder
        AllGear = new List<Item>(Resources.LoadAll<Item>("Scriptables/Gear"));       
    }

    private void StartCombat()
    {

        UpdateGear();
    }

    private void EndCombat()
    {

        UpdateGear();
    }

    private void SceneChange(Levels newLevel)
    {

    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStartCombat -= StartCombat;
            GameManager.Instance.OnEndCombat -= EndCombat;
            GameManager.Instance.OnSceneChange -= SceneChange;
        }
    }
}
