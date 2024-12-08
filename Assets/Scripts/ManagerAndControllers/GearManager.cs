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

    /// <summary>
    /// List of Gear the player current has.
    /// </summary>
    public List<Item> PlayerCurrentGear = new List<Item>();

    public List<Item> AllGear = new List<Item>();

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
            if (item.IsEquipped)
            {
                PlayerCurrentGear.Add(item);
            }
        }
    }

    /// <summary>
    /// Return all gear by type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<Item> GetIAllGearByType(Item.ItemType type)
    {
        return AllGear.FindAll(item => item.type == type);
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
