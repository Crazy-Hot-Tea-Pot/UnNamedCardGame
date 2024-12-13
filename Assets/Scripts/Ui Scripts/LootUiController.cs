using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootUiController : UiController
{
    public int LootScrap
    {
        get
        {
            return lootScrap;
        }
        set
        {
            lootScrap = value;
        }
    }
    public List<NewChip> LootChips
    {
        get
        {
            return lootChips;
        }
        set
        {
            lootChips = value;
        }
    }
    public List<Item> LootItems
    {
        get
        {
            return lootItems;
        }
        set
        {
            lootItems = value;
        }
    }

    [Header("Prefabs")]
    public GameObject ChipPrefab;
    public GameObject LootPrefab;

    [Header("Loot Stuff")]
    public GameObject LootGainDisplay;
    public TextMeshProUGUI LootGainInfo;
    public GameObject LootContainer;

    [Header("Selection stuff")]
    public GameObject SelectionDisplay;
    public GameObject SelectionContainer;
    public Button CancelButton;

    [Header("Animation values")]
    [Range(0.01f,10f)]
    public float Duration = 1f;
    

    private int lootScrap;
    private List<NewChip> lootChips=new();
    private List<Item> lootItems=new();
    private float elapsedTime=0f;
    private readonly int startValue = 0;
    private NewChip selectedChip;


    public override void Initialize()
    {
        Debug.Log("Loot Ui initialized");
        CancelButton.onClick.AddListener(()=>CancelSelection());
    }
    /// <summary>
    /// Put information into the screen.
    /// make sure to call after you have sent info to the Controller.
    /// </summary>
    public void UpdateLootScreen()
    {
        foreach (Transform child in LootContainer.transform)
        {
            Destroy(child.gameObject);
        }

        StartCoroutine(UpdateScrapLootGainInfo());
        StartCoroutine(SpawnLootWithDelay());
    }
    /// <summary>
    /// Bring up the selection so user can replace chip for new chip.
    /// </summary>
    /// <param name="selectedChip"></param>
    public void BringUpChipSelection(NewChip selectedChip)
    {
        this.selectedChip = selectedChip;

        SelectionDisplay.SetActive(true);

        foreach (Transform child in SelectionContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(NewChip newChip in ChipManager.Instance.PlayerDeck)
        {
            GameObject Chip = Instantiate(ChipPrefab, SelectionContainer.transform);

            Chip.GetComponent<Chip>().NewChip=newChip;

            Chip.GetComponent<Chip>().SetChipModeTo(global::Chip.ChipMode.Delete);
        }
    }
    /// <summary>
    /// Replace replaceChip with selectedChip and refresh LootScreen.
    /// </summary>
    /// <param name="replaceChip"></param>
    public void ReplaceChip(NewChip replaceChip)
    {
        ChipManager.Instance.DeleteChip(replaceChip);
        ChipManager.Instance.AddNewChipToDeck(selectedChip);

        LootChips.Remove(selectedChip);


        selectedChip = null;

        SelectionDisplay.SetActive(false);

        UpdateLootScreen();
    }
    private void CancelSelection()
    {
        SelectionDisplay.SetActive(false);

        UpdateLootScreen ();
    }
    private IEnumerator UpdateScrapLootGainInfo()
    {
        int targetValue=LootScrap;

        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;

            // Interpolate the value
            int currentScrap = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, elapsedTime / Duration));

            // Update the text
            LootGainInfo.SetText($"They have dropped {currentScrap} Scrap and the following chips and items.");

            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        LootGainInfo.SetText($"They have dropped {LootScrap} Scrap and the following chips and items.");
    }
    private IEnumerator SpawnLootWithDelay()
    {
        foreach (NewChip chip in lootChips)
        {
            // Instantiate the LootPrefab
            GameObject newLoot = Instantiate(LootPrefab, LootContainer.transform);

            // Populate the loot
            LootController lootController = newLoot.GetComponent<LootController>();
            lootController.PopulateLoot(chip);

            // Wait for 1 second before spawning the next
            yield return new WaitForSeconds(Duration);
        }

        foreach (Item item in lootItems)
        {
            // Instantiate the LootPrefab
            GameObject newLoot = Instantiate(LootPrefab, LootContainer.transform);

            // Populate the loot
            LootController lootController = newLoot.GetComponent<LootController>();
            lootController.PopulateLoot(item);

            // Wait for 1 second before spawning the next
            yield return new WaitForSeconds(Duration);
        }
    }
    void OnDestroy()
    {
        CancelButton.onClick.RemoveAllListeners();
    }
}
