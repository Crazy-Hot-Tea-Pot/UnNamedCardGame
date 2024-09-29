using UnityEngine;

[CreateAssetMenu(fileName = "NewDefenseCard", menuName = "Card System/Defense Card")]
public class DefenseCard : NewCard
{
    /// <summary>
    /// How much shield the card will give.
    /// </summary>
    public int shieldAmount;

    public override void OnCardPlayed()
    {
        base.OnCardPlayed();
        Debug.Log(cardName + " used to gain " + shieldAmount + " shield.");

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ApplyShieldToPlayer(shieldAmount);
    }
}
