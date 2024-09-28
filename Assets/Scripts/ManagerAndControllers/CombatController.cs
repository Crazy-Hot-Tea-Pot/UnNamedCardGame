using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Combadant
{
    public GameObject combadant;
    public bool attacked;
}

public class CombatController : MonoBehaviour
{

    [SerializeField]
    private int roundCounter;

    public List<Combadant> Combadants = new();

    /// <summary>
    /// Amount of rounds that has passed.
    /// </summary>
    public int RoundCounter
    {
        get
        {
            return roundCounter;
        }
        private set
        {
            roundCounter = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TODO check if all combatents attacked and then call the method NextRound
        if (false)
        {
            NextRound();
        }
    }

    /// <summary>
    /// Start Combat
    /// </summary>
    public void StartCombat()
    {
        RoundCounter = 1;

        //this is a test add to the list.
        Combadant playertest = new();
        playertest.combadant = GameObject.FindGameObjectWithTag("Player");
        playertest.attacked = false;
        Combadants.Add(playertest);

        //Set enemies to combat mode in combat zone
        foreach (GameObject combatEnemy in GameManager.Instance.enemyList)
        {
            Combadant test = new Combadant();
            test.combadant = combatEnemy;
            test.attacked = false;
            Combadants.Add(test);

            combatEnemy.GetComponent<Enemy>().InCombat = true;
        }        
    }
    /// <summary>
    /// When used an action or attack and change status in combat.
    /// </summary>
    /// <param name="gameObject"></param>
    public void TurnUsed(GameObject gameObject)
    {
        foreach (var combadent in Combadants)
        {
            if(combadent.combadant.name == gameObject.name)
            {
                combadent.attacked = true;
            }
        }
    }
    /// <summary>
    /// Check if I can make an action in this turn.
    /// TODO go through list of combadents and 
    /// if other combadents before the gameobject hasn't attacked yet then deny the gameobject from making an action.
    /// </summary>
    /// <param name="gameObject"></param>
    public bool CanIMakeAction(GameObject gameObject)
    {
        return true;
    }
    /// <summary>
    /// Move to next round.
    /// reset all objects in scene so they can attack.
    /// </summary>
    private void NextRound()
    {
        roundCounter++;


        foreach (Combadant combadant in Combadants)
        {
            combadant.attacked = false;
        }
    }
}
