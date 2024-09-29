using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackCard", menuName = "Card System/Attack Card")]
public class AttackCard : NewCard
{
    /// <summary>
    /// Amount of Damage Card will do.
    /// </summary>
    public int damage;

    public override void OnCardPlayed()
    {
        base.OnCardPlayed();
        Debug.Log(cardName +
            " used to deal " +
            damage +
            "damage to " +
            GameManager.Instance.enemyList[0].GetComponent<Enemy>().name);

        GameManager.Instance.enemyList[0].GetComponent<Enemy>().TakeDamage(damage);
    }
}
