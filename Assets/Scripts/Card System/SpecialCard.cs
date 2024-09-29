using UnityEngine;

/// <summary>
/// TODO
/// come back and finalize this scripable with more information.
/// </summary>
[CreateAssetMenu(fileName = "NewSpecialCard", menuName = "Card System/Special Card")]
public class SpecialCard : NewCard
{
    public string specialEffect;

    public override void OnCardPlayed()
    {
        base.OnCardPlayed();
        Debug.Log(cardName + " triggered a special effect: " + specialEffect);
    }
}
