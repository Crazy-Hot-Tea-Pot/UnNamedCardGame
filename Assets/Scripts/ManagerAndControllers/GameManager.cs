using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameData;
using static UnityEditor.Progress;


public class GameManager : MonoBehaviour
{
    public PlayerUIManager uiManager;


    //Since gameManager isn't acting really like a gameManager adding headers to make sense to me.
    [Header("Chip Manager stuff")]
    ///<summary>Hand limit</summary>
    public int handlimit;
    ///<summary>Deck limit</summary>
    public int decklimit;
    ///<summary>Draws per turn</summary>
    public int drawsPerTurn;

    /// <summary>
    /// list of items player has.
    /// Move this later!
    /// </summary>
    public List<Gear> Items = new List<Gear>();

    public List<NewChip> playerHand;
    public List<NewChip> playerDeck = new List<NewChip>();
    public List<NewChip> usedChips;
    //will get the Chips from resources
    public List<NewChip> NewChips;
    // Default newChipInPlayerHand
    public GameObject ChipPrefab;

    //UIVeriables
    public GameObject chipPanel;
    public GameObject uiCanvas;
    public GameObject uiContainer;

    [Header("Enemy Variables")]
    public List<GameObject> enemyList;

    /// <summary>
    /// Is the player in combat true is yes
    /// </summary>
    public bool InCombat
    {
        get
        {
            return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().InCombat;
        }
    }

    public static GameManager Instance
    {
        get;
        private set;
    }
    /// <summary>
    /// Gets Draws per turn amount.
    /// </summary>
    public int DrawsPerTurn
    {
        get
        {
            return drawsPerTurn;
        }
        private set
        {
            drawsPerTurn = value;
        }
    }

    private Levels targetScene;
    /// <summary>
    /// Scene to load.
    /// </summary>
    public Levels TargetScene
    {
        get
        {
            return targetScene;
        }
        private set
        {
            targetScene = value;
        }
    }

    public Levels CurrentLevel
    {
        get;
        private set;
    }
    public Levels PreviousScene
    {
        get;
        private set;
    }

    void Awake()
    {
        // Check if another instance of the GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep this object between scenes

            // Subscribe to the sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicates
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //Fill variables
        //uiContainer = GameObject.Find("BadCanvasSystem");
        //uiCanvas = uiContainer.transform.FindChild("Canvas").gameObject;
        //chipPanel = uiCanvas.transform.FindChild("Panel").gameObject;

        Initialize();
        ShufflePlayerDeck();
        //DrawChip(DrawsPerTurn);

        //test
       SoundManager.StartBackgroundSound(BgSound.Background);

    }

    private void Initialize()
    {
        // Load all NewChip ScriptableObjects from "Scriptables/Cards/Attack"
        NewChips = new List<NewChip>(Resources.LoadAll<NewChip>("Scriptables/Cards"));

    }

    /// <summary>
    /// Adds Chips to player deck.
    /// </summary>
    /// <param name="newChipToAdd"></param>
    public void AddChipToDeck(NewChip newChipToAdd)
    {
        playerDeck.Add(newChipToAdd);
    }

    ///<summary>Shuffles the player deck</summary>
    public void ShufflePlayerDeck()
    {
        //Makes sure there are atleast 3 cards and not null
        if (playerDeck.Count != 0 && playerDeck.Count != 1 && playerDeck != null)
        {
            //Cycles through the player deck
            for (int i = 0; i < playerDeck.Count - 1; i++)
            {
                //Collects random number for our cards
                int num1 = Roll(0, playerDeck.Count);
                int num2 = Roll(0, playerDeck.Count);
                //Holds the value of the next newChipInPlayerHand to be replaced
                NewChip placeHolder = playerDeck[num2 - 1];
                //Replaces newChipInPlayerHand 2 with newChipInPlayerHand 1
                playerDeck[num2 - 1] = playerDeck[num1 - 1];
                //Replaces newChipInPlayerHand 1 with the place holder (Chip 2)
                playerDeck[num1 - 1] = placeHolder;
                //Clears our place holder
                placeHolder = null;
            }
        }

    }

    ///<summary>Draws a newChipInPlayerHand</summary>
    public void DrawChip(int draws)
    {
        //Check if player deck has cards to draw
        if (playerDeck.Count != 0)
        {
            //How many cards need to be drawn
            //checks the hand limit and continues if possible otherwise nothing happens
            if (playerHand.Count < handlimit)
            {
                //How many cards need to be drawn
                for (int i = playerHand.Count; i < handlimit; i++)
                {

                    //take the first card on the top of the pile and add it to the players hand
                    playerHand.Add(playerDeck[0]);
                    playerDeck.RemoveAt(0);
                }
                UpdateUI();
            }
            //If limit reached
            else
            {
                Debug.Log("Limit Reached");
            }
        }
        else
        {
            Debug.LogError("Player Deck is Empty, Don't worry Sabastian this is on purpose.");
        }

    }

    ///<summary>>
    ///Generates a random number
    ///</summary
    public int Roll(int max, int min)
    {
        int random = Random.Range(min, max);
        return random;
    }

    //A method for updating the newChipInPlayerHand ui elements
    void UpdateUI()
    {
        foreach (Transform child in chipPanel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (NewChip newChipInPlayerHand in playerHand)
        {
            GameObject newChipInstance = Instantiate(ChipPrefab, chipPanel.transform);

            //Find the Chip component on the prefab instance
            Chip chipComponent = newChipInstance.GetComponent<Chip>();

            chipComponent.newChip = newChipInPlayerHand;
            chipComponent.Mode = Chip.ChipMode.Combat;
            //Apply name to newChipInPlayerHand.
            if (newChipInPlayerHand.chipName == "" || newChipInPlayerHand.chipName == null)
                Debug.LogWarning("Scriptable {chipName} is empty on " + newChipInPlayerHand.name + " and this will cause errors.");
            else
                newChipInstance.name = newChipInPlayerHand.chipName;
        }
    }


    /// <summary>
    /// A method that can be used to transition into combat when out of combat
    /// </summary>
    public void StartCombat()
    {
        //Fill variables Make Method
        uiContainer = GameObject.Find("BadCanvasSystem");
        uiCanvas = uiContainer.transform.Find("Canvas").gameObject;
        uiCanvas = uiCanvas.transform.Find("CardUI").gameObject;
        chipPanel = uiCanvas.transform.Find("Panel").gameObject;

        //Enables combat UI
        uiCanvas.SetActive(true);
        DrawChip(drawsPerTurn);
        UpdateUI();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().InCombat = true;
    }

    /// <summary>
    /// A method to transition out of combat
    /// </summary>
    public void EndCombat()
    {
        //Deactivates the UI for combat
        uiCanvas.SetActive(false);
        //Disables combat
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().InCombat = false;

        // Add unused Chips to playerdeck.
        foreach(var usedChips in usedChips)
        {
            playerDeck.Add(usedChips);
        }
        // Clear used chips
        usedChips.Clear();

        foreach(var leftOverChip in playerHand)
        {
            playerDeck.Add(leftOverChip);
        }
        playerHand.Clear();

        ShufflePlayerDeck();

        //Call fill deck to repopulate deck.
        //PlayerUIManager.Instance.fillDeck();
        GameObject.Find("PlayerUIManager").GetComponent<PlayerUIManager>().fillDeck();


    }

    /// <summary>
    /// Adds enemies to a list
    /// </summary>
    public void RememberEnemy(GameObject enemy)
    {
        //A for each loop to chceck if the element exists in the list
        //bool tempCleared = true;
        //foreach (GameObject tempCheck in enemyList)
        //{
        //    if (enemy.name == tempCheck.name)
        //    {
        //        tempCleared = false;
        //    }
        //}
        //Adds it to the list if cleared
        //if (tempCleared == true)
        //{
            //This adds enemies to the enemy list
            enemyList.Add(enemy);
        //}

        //Had to put this here cause this is after all enemies are added @_@
        //No longer needed
        //GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>().StartCombat();
    }

    /// <summary>
    /// Picks up a newChipInPlayerHand and adds it to the deck aswell as deck inventory
    /// </summary>
    /// <param name="chip"></param>
    public void PickUpChip(NewChip chip)
    {
       //If the player deck isn't at limit
        if (playerDeck.Count! < decklimit)
        {            
            //Add newChipInPlayerHand to deck
            playerDeck.Add(chip);

            //Add to the inventory UI
            uiManager.AddCardToDeck(chip);

            //Destroy the newChipInPlayerHand from the game world 
            Destroy(chip);
        }
        else
        {
            //In the future play a sound or have a visual effect for can't pick it up
            Debug.Log("Can't pick it up");
        }

    }

    /// <summary>
    /// called when newChipInPlayerHand has been used and to be removed.
    /// </summary>
    /// <param name="chip"></param>
    public void KillChip(GameObject chip)
    {
        //Add the used Chips to the list of used Chips so they aren't deleted and go back to the deck and of game
        usedChips.Add(chip.GetComponent<Chip>().newChip);
        //Remove chip from player hand
        playerHand.Remove(chip.GetComponent<Chip>().newChip);
    }

    /// <summary>
    /// Initate change level.
    /// </summary>
    /// <param name="level"></param>
    public void RequestScene(Levels level)
    {
        switch (CurrentLevel)
        {
            case Levels.Title:
            case Levels.Loading:
                break;
            default:
                DataManager.Instance.CurrentGameData.Level = level;
                //Get Player
                PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

                //Send Data to DataManager
                DataManager.Instance.CurrentGameData.Health = player.Health;
                DataManager.Instance.CurrentGameData.MaxHealth = player.MaxHealth;
                DataManager.Instance.CurrentGameData.Scraps = player.Scrap;

                //clear chipNames
                DataManager.Instance.CurrentGameData.Chips.Clear();

                //Save Chips
                foreach (var chip in playerDeck)
                {
                    ChipData chipSave = new ChipData
                    {
                        Name = chip.chipName,
                        IsUpgraded = chip.IsUpgraded,
                        DisableCounter = chip.DisableCounter
                    };
                    DataManager.Instance.CurrentGameData.Chips.Add(chipSave);
                }

                //Gears

                DataManager.Instance.CurrentGameData.Gears.Clear();

                foreach (var gear in Items)
                {
                    ItemData itemData = new ItemData();
                    itemData.GearName = gear.itemName;
                    itemData.isEquipped = gear.IsEquipted;
                    itemData.AmountOfAbilities = gear.AbilityList.Count;

                    foreach (var ability in gear.AbilityList)
                    {
                        AbilityData abilityData = new AbilityData();
                        abilityData.AbilityName = ability.abilityName;
                        abilityData.IsUpgraded = ability.isUpgraded;
                        itemData.ListOfAbilities.Add(abilityData);
                    }

                    DataManager.Instance.CurrentGameData.Gears.Add(itemData);
                }

                //Do a Auto Save
                DataManager.Instance.AutoSave();

                break;
        }
        
        TargetScene = level;
        SceneManager.LoadScene(Levels.Loading.ToString());        
    }

    /// <summary>
    /// Load Data after level loads
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        // Update the CurrentLevel based on the loaded level
        if (System.Enum.TryParse(scene.name, out Levels loadedScene))
        {
            // Track the previous level
            PreviousScene = CurrentLevel;

            CurrentLevel = loadedScene;

            switch (CurrentLevel)
            {
                case Levels.Title:
                    break;
                default:
                    playerDeck.Clear();
                    Items.Clear();
                    enemyList.Clear();

                    foreach (var chipSave in DataManager.Instance.CurrentGameData.Chips)
                    {
                        NewChip baseChip = Resources.Load<NewChip>($"Scriptables/Chips/{chipSave.Name}");
                        if (baseChip != null)
                        {
                            // Create a copy of the base chip and apply saved state
                            NewChip loadedChip = Instantiate(baseChip);

                            if (loadedChip.canBeUpgraded)
                                loadedChip.IsUpgraded = chipSave.IsUpgraded;

                            loadedChip.DisableCounter = chipSave.DisableCounter;
                            playerDeck.Add(loadedChip);
                        }
                        else
                        {
                            Debug.LogWarning($"Chip {chipSave.Name} not found in Resources.");
                        }
                    }
                    foreach(var gear in DataManager.Instance.CurrentGameData.Gears)
                    {
                        Gear newGear = Resources.Load<Gear>($"Scriptables/Equipment/{gear.GearName}");

                        Gear loadedGear = Instantiate(newGear);

                        loadedGear.IsEquipted = gear.isEquipped;
                        if(loadedGear != null)
                        {
                            if (gear.AmountOfAbilities > 0)
                            {
                                for (int i = 0; i < gear.AmountOfAbilities; i++)
                                {
                                    if (gear.ListOfAbilities[i].AbilityName == loadedGear.AbilityList[i].abilityName)
                                    {
                                        loadedGear.AbilityList[i].isUpgraded = gear.ListOfAbilities[i].IsUpgraded;
                                    }
                                }
                            }
                            Items.Add(loadedGear);
                        }
                    }

                    break;
            }
        }
    }
}