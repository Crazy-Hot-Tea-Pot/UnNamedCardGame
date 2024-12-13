using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootController : MonoBehaviour
{
    public TextMeshProUGUI lootDisplayName;
    public Image lootImage;
    public Button DropButton;
    public Button KeepButton;

    public NewChip NewChip
    {
        get
        {
            return newChip;
        }
        set
        {
            newChip = value;
        }
    }
    public Item NewItem
    {
        get
        {
            return newItem;
        }
        set
        {
            newItem = value;
        }
    }

    private Animator animator;
    private NewChip newChip;
    private Item newItem;

    /// <summary>
    /// Populate chips.
    /// </summary>
    /// <param name="chip"></param>
    public void PopulateLoot(NewChip chip)
    {
        NewChip = chip; 
        lootDisplayName.SetText(NewChip.chipName);
        lootImage.sprite= NewChip.chipImage;

        KeepButton.onClick.AddListener(Keep);
        DropButton.onClick.AddListener(Drop);
    }
    /// <summary>
    /// Populate items
    /// </summary>
    /// <param name="item"></param>
    public void PopulateLoot(Item item)
    {
        NewItem = item;
        lootDisplayName.SetText(NewItem.itemName);
        lootImage.sprite= NewItem.itemImage;

        KeepButton.onClick.AddListener(Keep);
        DropButton.onClick.AddListener(Drop);
    }

    private void Keep()
    {
        if(NewChip != null)
            UiManager.Instance.SelectedChipToReplace(NewChip);
        else
            UiManager.Instance.AddItemToInventory(NewItem);
    }
    private void Drop()
    {
        animator=GetComponent<Animator>();

        animator.SetTrigger("Shrink");

        if(NewChip==null)
            UiManager.Instance.DropLoot(NewItem);
        else
            UiManager.Instance.DropLoot(NewChip);

        Destroy(this.gameObject,1f);
    }
    void OnDestroy()
    {
        DropButton.onClick.RemoveListener(Drop);
        KeepButton.onClick.RemoveListener(Keep);
    }
}
