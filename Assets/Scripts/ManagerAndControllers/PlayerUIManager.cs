using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    /// <summary>
    /// Holds the ui for player screen like Health bars
    /// </summary>
    public GameObject uiPlayerCanvas;

    /// <summary>
    /// This variable holds the input actions for the player to allow for user input
    /// </summary>
    public PlayerInputActions inputActions;

    /// <summary>
    /// This input action holds input for opening the inventory UI
    /// </summary>
    private InputAction openInventory;

    /// <summary>
    /// This input action holds input for dropping Inventory Items
    /// </summary>
    private InputAction dropItem;


    /// <summary>
    /// A list for card selection
    /// </summary>
    public List<NewChip> selectionList;

    /// <summary>
    /// A short selection list for the final list of players picking cards
    /// </summary>
    public List<NewChip> shortSelection;

    //Options for the player to use when selecting new cards
    public GameObject panelDropCards;
    private GameObject optionOne;
    private GameObject optionTwo;
    private GameObject optionThree;
    public Image imageOne;
    public Image imageTwo;
    public Image imageThree;

    //Panel for deleting cards
    public GameObject panelCardDelete;

    //Ability ui images
    public Image attackUIImage;
    public Image utilityUIImage;
    public Image shieldUIImage;
    private Sprite blankAttackUIImage;
    private Sprite blankUtilityUIImage;
    private Sprite blankShieldUiImage;

    public Image imageBaseObject;

    #region InventoryUIVariables
    /// <summary>
    /// Inventory List for items in the players hands
    /// </summary>
    public List<GameObject> InventoryList;

    /// <summary>
    /// Holds the UI canvas for inventory
    /// </summary>
    public GameObject uiInventoryCanvas;

    /// <summary>
    /// Holds the chipPanel that the inventory UI must instantiate into
    /// </summary>
    public GameObject panelInventory;
    /// <summary>
    /// Holds the chipPanel that displays deck
    /// </summary>
    public GameObject panelDeck;
    /// <summary>
    /// Holds all active abilities
    /// </summary>
    public GameObject panelAbilty;

    /// <summary>
    /// A variable to tell us if the inventory UI is open
    /// </summary>
    private bool isActive = false;

    public bool IsActive
    {
        get
        {
            return isActive;
        }
        private set
        {
            isActive = value;
        }
    }

    /// <summary>
    /// Inventory button
    /// </summary>
    public GameObject buttonInv;

    /// <summary>
    /// Deck button
    /// </summary>
    public GameObject buttonDeck;
    #endregion

    #region UIResourcePools
    [Header("Player Stats UI")]
    /// <summary>
    /// Health bar UI element.
    /// </summary>
    public Image healthBar;
    public TextMeshProUGUI healthText;

    // Green (395E44 in RGB normalized)
    public Color fullHealthColor= new Color(0.23f, 0.37f, 0.27f);
    public Color lowHealthColor = Color.red;

    /// <summary>
    /// Shield bar Ui Element.
    /// </summary>
    public Image ShieldBar;
    public TextMeshProUGUI shiedText;

    public GameObject ShieldBarContainer;

    public Image energyBar;

    //Speed to fill the bar at.
    public float fillSpeed;
    #endregion

    private void Awake()
    {

        //assigns player input class
        inputActions = new PlayerInputActions();
        //Assigns the input for the player to open the UI for inventory
        openInventory = inputActions.Player.InventoryUI;
        //Assign the input for the player to drop items from the UI
        dropItem = inputActions.Player.DropItem;
        //Enables the UI
        openInventory.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Fills the inventory UI for deck
        //fillDeck();

        //Starting Inventory
        for(int i = 0; i < GameManager.Instance.Items.Count; i++)
        {
            AddToInventory(GameManager.Instance.Items[i]);
        }

        //Fill blank UI Sprites
        blankAttackUIImage = attackUIImage.sprite;
        blankShieldUiImage = shieldUIImage.sprite;
        blankUtilityUIImage = utilityUIImage.sprite;

        //Sets UI maximums for resource pools
        StartingPools();

        //Initalizatoin for fill
        Initialize();

        try
        {
            //Add the card buttons for drops and attach buttons
#pragma warning disable CS0618 // Type or member is obsolete is suppressed because I need this find child
            GameObject parent = GameObject.Find(panelDropCards.transform.FindChild("CardScreen").name);
            optionOne = GameObject.Find(parent.transform.FindChild("OptionOne").name);
            optionOne.AddComponent<Button>().onClick.AddListener(cardOptionOne);
            optionTwo = GameObject.Find(parent.transform.FindChild("OptionTwo").name);
            optionTwo.AddComponent<Button>().onClick.AddListener(cardOptionTwo);
            optionThree = GameObject.Find(parent.transform.FindChild("OptionThree").name);
            optionThree.AddComponent<Button>().onClick.AddListener(cardOptionThree);
            panelDropCards.SetActive(false);
        }
        catch {
            //Some where in this code its deactivating the canvas so here i reactivate it.
            // THIS IS TEMP AND NEEDS TO BE RESTRUCTURED
            Debug.Log("This stuff failed to find because UI is inactive.");
            uiPlayerCanvas.SetActive(true);
            GameObject parent = GameObject.Find(panelDropCards.transform.FindChild("CardScreen").name);
            optionOne = GameObject.Find(parent.transform.FindChild("OptionOne").name);
            optionOne.AddComponent<Button>().onClick.AddListener(cardOptionOne);
            optionTwo = GameObject.Find(parent.transform.FindChild("OptionTwo").name);
            optionTwo.AddComponent<Button>().onClick.AddListener(cardOptionTwo);
            optionThree = GameObject.Find(parent.transform.FindChild("OptionThree").name);
            optionThree.AddComponent<Button>().onClick.AddListener(cardOptionThree);
            //GameObject.Find("BtnEndTurn").SetActive(false);
            uiPlayerCanvas.transform.Find("BtnEndTurn").gameObject.SetActive(false);
            panelDropCards.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {       
        //On press open the inventroy UI if input is recieved or close if already in combat. WasPressedThisFrame() makes the input not be spammed it waits till the frame ends before recollecting
        if (openInventory.WasPressedThisFrame() && !GameManager.Instance.InCombat)
        {
            //Play sound for menu
            SoundManager.PlayFXSound(SoundFX.MenuSelection);

            if (uiInventoryCanvas.activeSelf == false)
            {
                OpenInventroy();
            }
            else if (uiInventoryCanvas.activeSelf == true)
            {
                CloseInventory();
            }
        }

    }

    #region InventoryUIStuff
    /// <summary>
    /// A method for the game to open the invnetory UI this method will also open the canvas of that UI if closed
    /// </summary>
    public void OpenInventroy()
    {
        //If the UI canvas is closed open it otherwise continue on
        if (uiInventoryCanvas.activeSelf == false)
        {
            //set ui canvas as active
            uiInventoryCanvas.SetActive(true);
            // Set player to ineracting so only click on UI.
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().IsInteracting = true;
        }

        //Tell the program the inventory is open
        IsActive = true;
        //Close playerUI
        uiPlayerCanvas.SetActive(false);
    }

    /// <summary>
    /// Closes the invnetory UI if the canvas is open
    /// </summary>
    public void CloseInventory()
    {
        //If canvas is open close it
        if (uiInventoryCanvas.activeInHierarchy == true)
        {
            //If deck panel is open
            if (panelDeck.activeInHierarchy == true)
            {
                //Switch to default screen
                switchMenuInventory();
            }
            //Close UI
            uiInventoryCanvas.SetActive(false);
            // Allow player to click on ground to move
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().IsInteracting = false;
        }

        //Tell the program the inventory is closed
        IsActive = false;
        //Open PlayerUI
        uiPlayerCanvas.SetActive(true);
    }

    /// <summary>
    /// Switches the UI menu to deck
    /// </summary>
    public void switchMenuDeck()
    {
        //Disables active chipPanel and activate decks
        panelInventory.SetActive(false);
        panelDeck.SetActive(true);

        if(panelDeck.activeInHierarchy)
            fillDeck();

        //Disable and enable buttons
        buttonDeck.SetActive(false);
        buttonInv.SetActive(true);
    }

    /// <summary>
    /// Switches the UI menu to Inventory
    /// </summary>
    public void switchMenuInventory()
    {
        //Disables active chipPanel and activate inventory
        panelDeck.SetActive(false);
        panelInventory.SetActive(true);
        //Disable and enable buttons
        buttonDeck.SetActive(true);
        buttonInv.SetActive(false);
    }

    /// <summary>
    /// Fills the deck in the UI in the PlayerManager to display deck inventory. ONLY USE AT START UP
    /// </summary>
    public void fillDeck()
    {
        //Removes all objects from panelDeck to repopulate.
        foreach (Transform child in panelDeck.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < GameManager.Instance.playerDeck.Count; i++)
        {
            GameObject chipTemp = Instantiate(GameManager.Instance.ChipPrefab, panelDeck.transform);
            Chip chipComponent = chipTemp.GetComponent<Chip>();
            chipComponent.SetChipModeTo(Chip.ChipMode.Inventory);

            //chipComponenet.SetChipModeTo(Chip.ChipMode.Inventory);

            chipComponent.newChip = GameManager.Instance.playerDeck[i];
        }    
    }

    /// <summary>
    /// Adds a card to the UI of the player inventory. NOTE THIS HAS NOTHING TO DO WITH FUNCTIONAL DECK INVENTORY THAT'S GAMEMANAGER
    /// </summary>
    public void AddCardToDeck(NewChip card)
    {
        Instantiate(card.chipImage, panelDeck.transform);
        //Add cards to the actual deck
        GameManager.Instance.playerDeck.Add(card);
    }

    /// <summary>
    /// Adds an item to the inventory UI
    /// </summary>
    /// <param name="item"></param>
    public void AddToInventory(Gear item)
    {
        //Create the UI image object
        Image itemObj = imageBaseObject;
        //Add the script and make the needed sciptable object get set
        if(itemObj.GetComponent<NewEquipment>() == null)
        {
            itemObj.AddComponent<NewEquipment>().equipmentButton = item;
        }
        else
        {
            itemObj.GetComponent<NewEquipment>().equipmentButton = item;
        }
        //Add a button
        itemObj.AddComponent<Button>();
        //Set the ui image to the correct image
        itemObj.sprite = item.image;
        //Attach an image to the gear
        Instantiate(itemObj, panelInventory.transform);

        item.EquiptItems();
    }

    /// <summary>
    /// Creates the UI ability on screen
    /// </summary>
    public void MakeActiveAbility(Ability ability)
    {
        //We have to renable the player canvas to add to it
        uiPlayerCanvas.SetActive(true);
        //Ability is an attack ability
        if (ability.abilityType == 1)
        {
            //Add the sprite image
            attackUIImage.sprite = ability.uiImage;
            //If we don't have a button attached
            if(attackUIImage.GetComponent<Button>() == null)
            {
                //Attach one
                attackUIImage.AddComponent<Button>();
            }
            //If we don't have the script attached attach one and give it the ability variable
            if(attackUIImage.GetComponent<NewAbility>() == null)
            {
                //Attach one
                attackUIImage.AddComponent<NewAbility>().abilityButton = ability;
            }
            //If we have the script attached for example from equiping and unequiping but unequiping glitched
            else if(attackUIImage.GetComponent<NewAbility>() != null)
            {
                //Just change the variable
                attackUIImage.GetComponent<NewAbility>().abilityButton = ability;
            }
            
        }
        //Ability is a shield ability
        else if(ability.abilityType == 2)
        {
            //Add the sprite image
            shieldUIImage.sprite = ability.uiImage;
            //If we don't have a button attached
            if (shieldUIImage.GetComponent<Button>() == null)
            {
                //Attach one
                shieldUIImage.AddComponent<Button>();
            }
            //If we don't have the script attached attach one and give it the ability variable
            if (shieldUIImage.GetComponent<NewAbility>() == null)
            {
                //Attach one
                shieldUIImage.AddComponent<NewAbility>().abilityButton = ability;
            }
            //If we have the script attached for example from equiping and unequiping but unequiping glitched
            else if (shieldUIImage.GetComponent<NewAbility>() != null)
            {
                //Just change the variable
                shieldUIImage.GetComponent<NewAbility>().abilityButton = ability;
            }
        }
        //Ability is a utility
        else if(ability.abilityType == 3)
        {
            //Add the sprite image
            utilityUIImage.sprite = ability.uiImage;
            //If we don't have a button attached
            if (utilityUIImage.GetComponent<Button>() == null)
            {
                //Attach one
                utilityUIImage.AddComponent<Button>();
            }
            //If we don't have the script attached attach one and give it the ability variable
            if (utilityUIImage.GetComponent<NewAbility>() == null)
            {
                //Attach one
                utilityUIImage.AddComponent<NewAbility>().abilityButton = ability;
            }
            //If we have the script attached for example from equiping and unequiping but unequiping glitched
            else if (utilityUIImage.GetComponent<NewAbility>() != null)
            {
                //Just change the variable
                utilityUIImage.GetComponent<NewAbility>().abilityButton = ability;
            }
        }
        else
        {
            Debug.LogError("This is not an ability type");
        }
        if (uiInventoryCanvas.activeInHierarchy)
        {
            //Close player UI
            uiPlayerCanvas.SetActive(false);
        }
    }

    /// <summary>
    /// Resets the UI ability to blank
    /// </summary>
    /// <param name="ability"></param>
    public void MakeDeactiveAbility(Ability ability)
    {
        //We have to renable the player canvas to delete from it
        uiPlayerCanvas.SetActive(true);
        //Attack ability
        if(ability.abilityType == 1)
        {
            //Destroy the script
            Destroy(attackUIImage.GetComponent<NewAbility>());
            //Empty the image source
            attackUIImage.sprite = blankAttackUIImage;
        }
        //Shield
        else if(ability.abilityType == 2)
        {
            //Destroy the script
            Destroy(shieldUIImage.GetComponent<NewAbility>());
            //Empty the image source
            shieldUIImage.sprite = blankShieldUiImage;
        }
        //Utility
        else if(ability.abilityType == 3)
        {
            //Destroy the script
            Destroy(utilityUIImage.GetComponent<NewAbility>());
            //Empty the image source
            utilityUIImage.sprite = blankUtilityUIImage;
        }
        //Hide it quick enough the player doesn't see
        uiPlayerCanvas.SetActive(false);
    }

    /// <summary>
    /// This function allows items to be removed from the UI it requires a object first and then a parent name Deck or Inventory as a string
    /// </summary>
    /// <param name="card"></param>
    public void RemoveFromUI(GameObject remObject, string parent)
    {
        if (parent == "Inventory")
        {
            //Destroys the game object from the UI element
            Destroy(GameObject.FindGameObjectWithTag(remObject.tag));
        }
        else if (parent == "Deck")
        {
            //Destroys the game object from the UI element
            foreach(Transform child in panelDeck.transform)
            {
                if(child.name == remObject.name)
                {
                    Destroy(child);
                }
            }
        }
    }

    #endregion

    #region Resource Pool Methods

    /// <summary>
    /// Updates the UI for player Health
    /// </summary>
    public void UpdateHealth()
    {        
        float currentHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Health;
        float maxHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().MaxHealth;

        float targetHealthPercentage = currentHealth / maxHealth;

        StopCoroutine(UpdateHealthOverTime(targetHealthPercentage));

        // Start the coroutine to smoothly update the health bar
        StartCoroutine(UpdateHealthOverTime(targetHealthPercentage));
    }
    private IEnumerator UpdateHealthOverTime(float targetFillAmount)
    {
        // While the bar is not at the target fill amount, update it
        while(Mathf.Abs(healthBar.fillAmount - targetFillAmount) > 0.001f)
        {
            // Lerp between current fill and target fill by the fill speed
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFillAmount, 1f * Time.deltaTime);

            // Lerp the color based on the health percentage
            healthBar.color = Color.Lerp(lowHealthColor, fullHealthColor, healthBar.fillAmount);

            // Display percentage as an integer (0 to 100)
            int percentage = Mathf.RoundToInt(healthBar.fillAmount * 100);
            healthText.SetText(percentage + "%");

            // Ensure the fill value gradually updates each frame
            yield return null;
        }

        // Ensure it snaps to the exact target amount at the end
        healthBar.fillAmount = targetFillAmount;
        healthBar.color = Color.Lerp(lowHealthColor, fullHealthColor, healthBar.fillAmount);

        // Display percentage as an integer (0 to 100)
        int finalPercentage = Mathf.RoundToInt(targetFillAmount * 100);
        healthText.SetText(finalPercentage + "%");
    }
    /// <summary>
    /// Updates the UI for player shield
    /// </summary>
    public void UpdateShield()
    {       
        var playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        if (playerController.Shield == 0 && playerController.MaxShield == 0)
        {
            ShieldBarContainer.SetActive(false);
        }
        else
        {
            ShieldBarContainer.SetActive(true);

            // Reset the ShieldBar fill amount to 0 if it's being activated for the first time
            if (ShieldBar.fillAmount == 0)
                    ShieldBar.fillAmount = 0;

            // Calculate the target shield percentage
            float shieldPercentage = (float)playerController.Shield / playerController.MaxShield;

            //ShieldBar.fillAmount = shieldPercentage;

            StopCoroutine(UpdateShieldOverTime(shieldPercentage));

            // Start the coroutine to smoothly update the shield bar
            StartCoroutine(UpdateShieldOverTime(shieldPercentage));
        }
    }
    private IEnumerator UpdateShieldOverTime(float targetFillAmount)
    {
        // While the difference between the current fill amount and the target is greater than the threshold
        while (Mathf.Abs(ShieldBar.fillAmount - targetFillAmount) > 0.001f)
        {
            // Lerp between current fill and target fill by the fill speed
            ShieldBar.fillAmount = Mathf.Lerp(ShieldBar.fillAmount, targetFillAmount, 0.5f * Time.deltaTime);
            
            // Display percentage as an integer (0 to 100)
            int percentage = Mathf.RoundToInt(ShieldBar.fillAmount * 100);
            shiedText.SetText(percentage + "%");


            // Ensure the fill value gradually updates each frame
            yield return null;
        }

        // Ensure it snaps to the exact target amount at the end
        ShieldBar.fillAmount = targetFillAmount;

        // Display percentage as an integer (0 to 100)
        int finalPercentage = Mathf.RoundToInt(targetFillAmount * 100);
        shiedText.SetText(finalPercentage + "%");


        if (ShieldBar.fillAmount <= 0f)
            ShieldBarContainer.SetActive(false);
    }

    //Sets variables to initalize fill speed
    public void Initialize()
    {
        fillSpeed = 0.5f;
    }

    //Provides the ability to update energy right away
    public void InstantUpdateEnergy(float currentEnergy)
    {
        energyBar.fillAmount = currentEnergy;
    }

    /// <summary>
    /// Call this to change the Energy Bar UI. this method accepts both UI and Float no need to change before.
    /// </summary>
    /// <param name="currentEnergy"></param>
    /// <param name="maxEnergy"></param>
    public void UpdateEnergy(float currentEnergy, float maxEnergy)
    {
        if (energyBar != null)
        {
            // Normalize the energy value to a 0-1 range
            float tempTargetFillAmount = currentEnergy / maxEnergy;

            //Clap at 0 and 1 so don't go over
            tempTargetFillAmount = Mathf.Clamp01(tempTargetFillAmount);

            StopCoroutine(FillEnergyOverTime(tempTargetFillAmount));

            StartCoroutine(FillEnergyOverTime(tempTargetFillAmount));
        }
        else
        {
            Debug.LogError("Energy Bar Image component not found.");
        }
    }


    /// <summary>
    /// Fill EnergyBar by amount over time.
    /// </summary>
    /// <param name="targetFillAmount"></param>
    /// <returns></returns>
    private IEnumerator FillEnergyOverTime(float targetFillAmount)
    {

        // While the bar is not at the target fill amount, update it
        while (Mathf.Abs(energyBar.fillAmount - targetFillAmount)>0.001f)
        {
            // Lerp between current fill and target fill by the fill speed
            energyBar.fillAmount = Mathf.Lerp(energyBar.fillAmount, targetFillAmount, fillSpeed * Time.deltaTime);

            // Ensure the fill value gradually updates each frame
            yield return null;
        }

        // Ensure it snaps to the exact target amount at the end
        energyBar.fillAmount = targetFillAmount;

    }    

    /// <summary>
    /// UI starting points that set the sliders to the maximum of the variables
    /// </summary>
    public void StartingPools()
    {
        //healthBar.maxValue = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().MaxHealth;
        UpdateShield();
    }
    #endregion

    #region CardDropUIandFunction
    /// <summary>
    /// This method adds enemies cards to the list
    /// </summary>
    public void AddChipChoices(NewChip chip)
    {
        selectionList.Add(chip);
    }

    /// <summary>
    /// This method allows us to draw cards from enemy and use this to present a list of available cards to the player
    /// </summary>
    public void DropCard()
    {
        //If our short list isn't too big
        if (shortSelection.Count < 3 || shortSelection == null)
        {
            Debug.Log("Got here");
            //Roll a random number of the list length 3 times
            for (int i = 0; i < 3; i++)
            {
                NewChip holder;

                //If there can be no repeats
                if (selectionList.Count > 3 && selectionList != null)
                {
                    //Get a random number
                    int roll = GameManager.Instance.Roll(1, selectionList.Count - 1);
                    //Assign holder that random roll
                    holder = selectionList[roll];
                    //Fail out counter
                    int failOut = 0;
                    bool pass = false;
                    //No repeats
                    while (!pass)
                    {
                        if (shortSelection.Contains(holder))
                        {
                            failOut++;
                            //Redo random number
                            int reroll = GameManager.Instance.Roll(1, selectionList.Count - 1);
                            //Replace holder so the loop can exit
                            holder = selectionList[reroll];

                            //If we have looped 5 times forget it and move on a duplicate is better then an infinite loop. This is most likely to happen if there are too many duplicating cards
                            //Especially if fighting two of the same enemies
                            if (failOut == 5)
                            {
                                break;
                            }
                        }
                        else
                        {
                            pass = true;
                        }
                    }
                    //Add to the short list
                    shortSelection.Add(holder);
                }
                //If the list is too small then continue from here or if the list is sectin 3 then there is no reason to do any math for rerolling it's a waste of time
                else if (selectionList.Count < 3 && selectionList != null || selectionList.Count == 3 && selectionList != null)
                {
                    holder = selectionList[i];
                    //Add to the short list
                    shortSelection.Add(holder);
                }
                else if (selectionList == null)
                {
                    Debug.Log("No selection");
                }
            }
        }

        //Update the image sprites
        imageOne.sprite = shortSelection[0].chipImage;
        imageTwo.sprite = shortSelection[1].chipImage;
        imageThree.sprite = shortSelection[2].chipImage;

    }

    /// <summary>
    /// Card drop UI is opened
    /// </summary>
    public void openDropUI()
    {
        // Set player to ineracting so only click on UI.
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().IsInteracting = true;
        //Display UI
        panelDropCards.SetActive(true);
        //Visually and actually draws the chip for the UI that can be used in the 3 card selection
        DropCard();
    }

    /// <summary>
    /// Card drop UI is closed
    /// </summary>
    public void closeDropUI()
    {
        panelDropCards.SetActive(false);
        //Open panel card delete for next section
        panelCardDelete.SetActive(true);
        // Allow player to click on ground to move
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().IsInteracting = false;
    }

    /// <summary>
    /// Closes the swap panel
    /// </summary>
    public void CloseSwapPanel()
    {
        panelCardDelete.SetActive(false);
    }

    /// <summary>
    /// This works just like the fill deck method except that it fills the panelDropCards with cards
    /// </summary>
    public void FillCardSwap()
    {
        closeDropUI();
        //Removes all objects from panelCardDelete to repopulate.
        foreach (Transform child in panelCardDelete.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < GameManager.Instance.playerDeck.Count; i++)
        {
            //Creates the chip object in card delete pannel
            GameObject chipTemp = Instantiate(GameManager.Instance.ChipPrefab, panelCardDelete.transform);            
            //chipTemp.GetComponent<Button>().interactable = true;
            //Destroy button component to then replace
            chipTemp.GetComponent<Button>().onClick.RemoveAllListeners();
            //Adds a listener on a button that on click will use the swap chip variable
            chipTemp.GetComponent<Button>().onClick.AddListener(() => ChipSwap(chipTemp));
            Chip chipComponenet = chipTemp.GetComponent<Chip>();
            //Make it interactable again
            chipComponenet.SetChipModeTo(Chip.ChipMode.Delete);            
            chipComponenet.newChip = GameManager.Instance.playerDeck[i];
        }
        ClearSelectionList();
    }

    /// <summary>
    /// A method to remove a card when clicked
    /// </summary>
    public void ChipSwap(GameObject deleteableObj)
    {
        //Remove item from the ui of deck
        RemoveFromUI(deleteableObj, "Deck");
        //Remove item from the actual game manager deck
        GameManager.Instance.playerDeck.Remove(deleteableObj.GetComponent<Chip>().newChip);

        CloseSwapPanel();
    }

    /// <summary>
    /// Adds card option one
    /// </summary>
    public void cardOptionOne()
    {
        //Add the card to the deck
        AddCardToDeck(shortSelection[0]);

        //Closes the card drop ui panel and opens a new panel to allow the player to swap a card
        FillCardSwap();
    }
    /// <summary>
    /// Adds card option two
    /// </summary>
    public void cardOptionTwo()
    {
        //Add the card to the deck
        AddCardToDeck(shortSelection[1]);

        //Closes the card drop ui panel and opens a new panel to allow the player to swap a card
        FillCardSwap();
    }

    /// <summary>
    /// Adds card option three
    /// </summary>
    public void cardOptionThree()
    {
        //Add the card to the deck
        AddCardToDeck(shortSelection[2]);

        //Closes the card drop ui panel and opens a new panel to allow the player to swap a card
        FillCardSwap();
    }

    /// <summary>
    /// Clears the selection list
    /// </summary>
    public void ClearSelectionList()
    {
        //Empty the list
        selectionList.Clear();
        shortSelection.Clear();
    }
    #endregion
}
