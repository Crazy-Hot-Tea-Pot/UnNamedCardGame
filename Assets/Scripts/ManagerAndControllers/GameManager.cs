using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Extension method for recursive search
public static class TransformExtensions
{
    public static Transform FindRecursive(this Transform parent, string name)
    {
        if (parent.name == name)
            return parent;

        foreach (Transform child in parent)
        {
            Transform result = child.FindRecursive(name);
            if (result != null)
                return result;
        }

        return null;
    }
}
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

    public List<NewChip> playerHand;
    public List<NewChip> playerDeck;
    public List<NewChip> usedChips;
    //will get the chips from resources
    public List<NewChip> NewChips;
    // Default newChipInPlayerHand
    public GameObject ChipPrefab;

    //THIS HAS TO BE REMOVED WILL MAKE TEMPORARY FIX
    [Header("UI Veriables")]
    //UIVeriables
    public GameObject chipPanel;
    public GameObject uiCanvas;

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

    public enum Scenes
    {
        Title,        
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        Workshop,
        Loading,
        Settings
    }
    private Scenes targetScene;
    /// <summary>
    /// Scene to load.
    /// </summary>
    public Scenes TargetScene
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
    public Scenes currentScene;
    public Scenes previousScene;

    void Awake()
    {
        // Check if another instance of the GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep this object between scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicates
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        ShufflePlayerDeck();
        //DrawChip(DrawsPerTurn);

        //test
       SoundManager.StartBackgroundSound(BgSound.Background);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Initialize()
    {
        // Load all NewChip ScriptableObjects from "Scriptables/Cards/Attack"
        NewChips = new List<NewChip>(Resources.LoadAll<NewChip>("Scriptables/Cards"));

    }

    /// <summary>
    /// Adds chips to player deck.
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
        // THIS HAS TO BE REMOVE ITS TEMPORARY AS GAMEMANAGER SHOULND'T BE DOING ANYTHING WITH UI.
        if (chipPanel == null)
            chipPanel = FindGameObjectByName("Panel");

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
            StartCoroutine(chipComponent.ChipInstantiatedOnInActiveObject(Chip.ChipMode.Combat));
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
        //This is temporary this UI stuff can't be in GameManager
        uiCanvas = FindGameObjectByName("Canvas");
        

        //Enables combat UI
        uiCanvas.SetActive(true);
        DrawChip(drawsPerTurn);
        UpdateUI();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().InCombat = true;
    }
    /// <summary>
    /// Added this because of bad UI canvas system.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
        public GameObject FindGameObjectByName(string name)
        {
            Scene activeScene = SceneManager.GetActiveScene();
            GameObject[] rootObjects = activeScene.GetRootGameObjects();

            foreach (GameObject rootObject in rootObjects)
            {
                Transform result = rootObject.transform.FindRecursive(name);
                if (result != null)
                    return result.gameObject;
            }

            return null;
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

        // Add unused chips to playerdeck.
        foreach(var usedChips in usedChips)
        {
            playerDeck.Add(usedChips);
        }
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
        //Add the used chips to the list of used chips so they aren't deleted and go back to the deck and of game
        usedChips.Add(chip.GetComponent<Chip>().newChip);
        //Remove chip from player hand
        playerHand.Remove(chip.GetComponent<Chip>().newChip);
    }

    /// <summary>
    /// Initate change level.
    /// </summary>
    /// <param name="scene"></param>
    public void RequestScene(Scenes scene)
    {
        //Get Player
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        //Send Data to DataManager
        DataManager.Instance.GameData.health = player.Health;
        DataManager.Instance.GameData.maxHealth = player.MaxHealth;
        DataManager.Instance.GameData.scrap = player.Scrap;

        //clear chipNames
        DataManager.Instance.GameData.chipNames.Clear();

        //Send Chips to DataManager
        foreach (var chip in playerDeck)
        {
            // Save chip names
            DataManager.Instance.GameData.chipNames.Add(chip.chipName);
        }

        //Send Abilities to DataManager
        //TODO

        //Save
        DataManager.Instance.Save("AutoSave");

        TargetScene = scene;
        SceneManager.LoadScene(Scenes.Loading.ToString());        
    }
}