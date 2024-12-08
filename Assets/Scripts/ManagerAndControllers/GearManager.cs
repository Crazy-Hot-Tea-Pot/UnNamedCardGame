using System.Collections;
using System.Collections.Generic;
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

    public List<Item> PlayerCurrentGear = new List<Item>();

    public List<Item> AllGears = new List<Item>();

    private static GearManager instance;

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
    }

    /// <summary>
    /// Updates the list of playercurrentgear.
    /// </summary>
    public void UpdateGear()
    {
        foreach (Item item in AllGears)
        {
            if (item.IsEquipped)
                PlayerCurrentGear.Add(item);
        }
    }

    /// <summary>
    /// Return all gear by type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<Item> GetIAllGearByType(Item.ItemType type)
    {
        return AllGears.FindAll(item => item.type == type);
    }

    private void LoadAllGear()
    {
        // Load all Item ScriptableObjects in the Resources folder
        AllGears = new List<Item>(Resources.LoadAll<Item>("Scriptables/Gear"));

        // Debugging to confirm the items loaded
        foreach (var item in AllGears)
        {
            Debug.Log($"Loaded Item: {item.itemName}, Type: {item.type}");
        }

        if (AllGears.Count == 0)
        {
            Debug.LogWarning("No items found in the Gear folder!");
        }
    }    
}
