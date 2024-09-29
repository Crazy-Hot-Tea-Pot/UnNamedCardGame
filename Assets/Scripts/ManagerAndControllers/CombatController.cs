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

    [SerializeField]
    private string currentCombatant;

    private int currentCombatantIndex;

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

    // Tracks the index of the combatant whose turn it is
    public int CurrentCombatantIndex
    {
        get
        {
            return currentCombatantIndex;
        }
        private set
        {
            currentCombatantIndex = value;
            if(Combadants.Count != 0)
                CurrentCombatant = Combadants[currentCombatantIndex].combadant.name;

        }
    }
    public string CurrentCombatant
    {
        get
        {
            return currentCombatant;
        }
        private set
        {
            currentCombatant = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentCombatantIndex = 0;
        CurrentCombatant = "No Combat Yet";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.InCombat)
        {
            CheckIfAllAttacked();
        }        
    }
    /// <summary>
    /// Check if the current combatant has attacked.
    /// Move to the next combatant.
    /// If all combatants have attacked (end of list), reset for the next round.
    /// </summary>
    private void CheckIfAllAttacked()
    {
        if (Combadants[currentCombatantIndex].attacked)
        {
            currentCombatantIndex++;

            if (currentCombatantIndex >= Combadants.Count)
            {
                NextRound();
            }
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
    /// Check if it's the current combatant's turn in the list
    /// If it's not this combatant's turn, return false
    /// </summary>
    /// <param name="gameObject"></param>
    public bool CanIMakeAction(GameObject gameObject)
    {        
        if (Combadants[currentCombatantIndex].combadant == gameObject)
        {
            return !Combadants[currentCombatantIndex].attacked;
        }
        
        return false;
    }
    /// <summary>
    /// Move to next round.
    /// Increase round counter
    /// Reset the current combatant index to the start of the list
    /// reset all objects in scene so they can attack.
    /// </summary>
    private void NextRound()
    {
        RoundCounter++;

        currentCombatantIndex = 0;


        foreach (Combadant combadant in Combadants)
        {
            combadant.attacked = false;
        }
    }
}
