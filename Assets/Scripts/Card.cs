using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The card class creates the basic properties of the class
/// </summary>
public class Card : MonoBehaviour
{
    [SerializeField]
    private bool active;

    private Button imageButton;

    /// <summary>
    /// Card title is the title that the card will display in game and the ID of the card.
    /// </summary>
    public string CardTitle;
    /// <summary>
    /// Card description holds the data that will display in game explaining what the card should actually do.
    /// </summary>
    public string CardDescription;
    /// <summary>
    /// Uses holds the number of times a card can be used before card death
    /// </summary>
    public int Uses;
    /// <summary>
    /// This variable decides if the card is active or inactive
    /// </summary>
    public bool IsActive
    {
        get
        {
            return active;
        }
    }

    public Sprite CardImage;

    public enum CardAbilities
    {
        NoAbility,
        Weapon,
        Armor,
        Trinket
    }
    public CardAbilities CardAbility;

    /// <summary>
    /// The cost of the Card.
    /// For all cards.
    /// </summary>
    public int energyCost;

    /// <summary>
    /// How much damage the card will do.
    /// For Weapons.
    /// </summary>
    public int damageAmount;

    /// <summary>
    /// How much shield the card will do.
    /// For Armors.
    /// </summary>
    public int sheildAmount;

    /// <summary>
    /// How much power the card will do.
    /// For Trinkets.
    /// </summary>
    public int powerAmount;

    void Start()
    {
        this.gameObject.name= CardTitle+" Card";

        //Assign method to button
        imageButton = GetComponent<Button>();
        imageButton.onClick.AddListener(CardSelected);
        
    }
    void CardSelected()
    {
        Debug.Log(CardTitle + " Title");
        active = true;

        switch (CardAbility)
        {
            case CardAbilities.NoAbility:
                break;
            case CardAbilities.Weapon:
                if (GameManager.Instance.enemyList.Count == 0)                
                    Debug.Log("They are no enemies to do anything to.");               
                else
                {
                    GameManager.Instance.enemyList[0].GetComponent<Enemy>().TakeDamage(damageAmount);
                }    
                break;
            default:
                break;
        }
    }
}
