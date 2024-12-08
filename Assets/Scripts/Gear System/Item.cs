using Microsoft.Unity.VisualStudio.Editor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Gear/Item")]
public class Item : ScriptableObject
{
    public enum ItemType { Weapon, Armor, Equipment }

    /// <summary>
    /// name of item.
    /// </summary>
    public string itemName;

    /// <summary>
    /// What item does
    /// </summary>
    public string itemDescription;

    /// <summary>
    /// Tip for player about item.
    /// </summary>
    public string itemTip;

    /// <summary>
    /// Image of item.
    /// </summary>
    public Sprite itemImage;

    /// <summary>
    /// What kind of type is the item.
    /// </summary>
    public ItemType type;

    /// <summary>
    /// List of abilities the item has.
    /// </summary>
    public List<ItemEffect> abilities;
    public bool IsEquipped
    {
        get
        {
            return isEquipped
;
        }
        set
        {
            isEquipped = value;

            //If false remove the passive effect from player.
            if (isEquipped)
            {
                switch (type)
                {
                    case ItemType.Equipment:
                        foreach (ItemEffect effect in abilities)
                        {
                            if (effect.IsPassiveEffect)
                                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ApplyEffect(effect.effectToApply);
                        }
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case ItemType.Equipment:
                        foreach (ItemEffect effect in abilities)
                        {
                            if (effect.IsPassiveEffect)
                                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().RemoveEffect(effect.effectToApply);
                        }
                        break;
                }
            }
        }
    }

    private bool isEquipped;
    public void ItemActivate()
    {

    }
}
