using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipManager : MonoBehaviour
{
    public static ChipManager Instance
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
    private List<NewChip> startingChips = new List<NewChip>();
    /// <summary>
    /// Making it read only to prevent future problems
    /// </summary>
    public IReadOnlyList<NewChip> StartingChips => startingChips;

    public List<Item> PlayerCurrentChips = new List<Item>();

    public List<Item> AllChips = new List<Item>();

    private static ChipManager instance;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
