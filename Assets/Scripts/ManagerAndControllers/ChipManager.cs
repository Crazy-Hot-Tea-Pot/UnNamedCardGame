using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class managers all things chip related.
/// </summary>
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
    private List<NewChip> startingChips = new();
    /// <summary>
    /// Making it read only to prevent future problems
    /// </summary>
    public IReadOnlyList<NewChip> StartingChips => startingChips;

    /// <summary>
    /// Chips in the players hand.
    /// </summary>
    public List<NewChip> PlayerHand = new();

    /// <summary>
    /// Chips in the players deck
    /// </summary>
    public List<NewChip> PlayerDeck = new();

    /// <summary>
    /// Chips player has used.
    /// </summary>
    public List<NewChip> UsedChips = new();

    /// <summary>
    /// All the chips in the whole game.
    /// </summary>
    public List<NewChip> AllChips = new();

    public GameObject chipPrefab;

    [SerializeField]
    private int deckLimit = 8;

    [SerializeField]
    private int handLimit = 4;

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
        Initalize();
        GameManager.Instance.OnStartCombat += StartCombat;
        GameManager.Instance.OnEndCombat += EndCombat;
        GameManager.Instance.OnSceneChange += SceneChange;
    }

    void Initalize()
    {
        LoadAllChips();
    }
    // Update is called once per frame
    void Update()
    {
        
    }    
    public void AddChipToDeck(NewChip newChipToAdd)
    {
        PlayerDeck.Add(newChipToAdd);
    }
    /// <summary>
    /// Draws a chip from playerDeck.
    /// </summary>
    /// <param name="draws"></param>
    public void RefreshPlayerHand()
    {
        if (PlayerDeck.Count == 0)
        {
            Debug.LogWarning("Deck is empty!");
            return;
        }

        int tempChipsToDraw = handLimit - PlayerHand.Count;

        while (tempChipsToDraw > 0 && PlayerDeck.Count > 0)
        {
            PlayerHand.Add(PlayerDeck[0]);
            PlayerDeck.RemoveAt(0);
            tempChipsToDraw--;
        }
    }
    public void PickUpChip(NewChip chip)
    {
        if (PlayerDeck.Count < deckLimit)
        {
            PlayerDeck.Add(chip);
            Debug.Log($"Picked up chip: {chip.chipName}");
        }
        else
        {
            Debug.LogWarning("Deck limit reached. Cannot pick up more chips.");
        }
    }

    public void KillChip(GameObject chipObject)
    {
        Chip chipComponent = chipObject.GetComponent<Chip>();
        if (chipComponent != null)
        {
            UsedChips.Add(chipComponent.NewChip);
            PlayerHand.Remove(chipComponent.NewChip);
        }

        Destroy(chipObject);
    }

    public void DropChip(NewChip chip)
    {
        // Removes chip from the player's hand and places it in the used chips
        if (PlayerHand.Contains(chip))
        {
            PlayerHand.Remove(chip);
            UsedChips.Add(chip);
            Debug.Log($"[ChipManager] Dropped chip: {chip.chipName}");
        }
        else
        {
            Debug.LogWarning("[ChipManager] Chip not found in hand.");
        }
    }    

    /// <summary>
    /// Load all Chip ScriptableObjects in the Resources folder.
    /// </summary>
    private void LoadAllChips()
    {
        AllChips.Clear();       
        AllChips = new List<NewChip>(Resources.LoadAll<NewChip>("Scriptables/Chips"));
    }
    private void StartCombat()
    {
        ShufflePlayerDeck();
    }
    private void ShufflePlayerDeck()
    {
        if (PlayerDeck.Count > 1)
        {
            for (int i = 0; i < PlayerDeck.Count; i++)
            {
                int randomIndex = Random.Range(0, PlayerDeck.Count);
                NewChip temp = PlayerDeck[randomIndex];
                PlayerDeck[randomIndex] = PlayerDeck[i];
                PlayerDeck[i] = temp;
            }
        }
    }
    private void EndCombat()
    {
        // Move unused chips back to the deck
        foreach (var chip in PlayerHand)
        {
            PlayerDeck.Add(chip);
        }
        PlayerHand.Clear();

        // Move used chips back to the deck
        foreach (var chip in UsedChips)
        {
            PlayerDeck.Add(chip);
        }
        UsedChips.Clear();

        ShufflePlayerDeck();
        Debug.Log("[ChipManager] Combat ended. Deck shuffled.");
    }
    private void SceneChange(Levels newLevel)
    {
        switch (newLevel)
        {
            case Levels.Tutorial:
                ShufflePlayerDeck();
                break;
        }
    }
    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStartCombat -= StartCombat;
            GameManager.Instance.OnEndCombat -= EndCombat;
            GameManager.Instance.OnSceneChange -= SceneChange;
        }
    }
}
