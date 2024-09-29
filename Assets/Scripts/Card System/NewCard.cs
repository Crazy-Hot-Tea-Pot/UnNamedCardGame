using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card System/New Card")]
public class NewCard : ScriptableObject
{
    /// <summary>
    /// Name of card
    /// </summary>
    public string cardName;
    /// <summary>
    /// Description of card.
    /// </summary>
    public string description;
    /// <summary>
    /// Image for card.
    /// </summary>
    public Sprite cardImage;
    /// <summary>
    /// Energy cost of card.
    /// </summary>
    public int energyCost;

    public virtual void OnCardPlayed()
    {
        // This will be overridden by specific card types
        Debug.Log(cardName + " played.");
        GameObject.FindGameObjectWithTag("Player").
            GetComponent<PlayerController>().
            PlayedCardOrAbility(energyCost);
    }
}
