using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
public enum TargetingType
{
    CombatController,
    Ability
}
public class Enemy : MonoBehaviour
{   

    public GameObject enemyTarget;
    /// <summary>
    /// Enemy current Target.
    /// </summary>
    public GameObject EnemyTarget
    {
        get { return enemyTarget; }
        protected set
        {
            enemyTarget = value;
        }
    }

    public float AttackRange;
    
    private float distanceToPlayer;

    public float DistanceToPlayer
    {
        get
        {
            distanceToPlayer= Vector3.Distance(transform.position, EnemyTarget.transform.position); ;
            return distanceToPlayer;
        }               
    }

    [Header("Enemy Components")]
    public Animator animator;
    public NavMeshAgent agent;    
    /// <summary>
    /// reference to player camera.
    /// </summary>
    public Camera playerCamera;

    [Header("Enemy statuss")]
    #region EnemyStatus
    private string enemyName;
    /// <summary>
    /// Returns name of enemy
    /// </summary>
    public virtual string EnemyName
    {
        get
        {
            if (enemyName == null || enemyName == "")
            {
                return this.gameObject.name;
            }
            else
                return enemyName;
        }
        protected set
        {
            enemyName = value;
            thisEnemyUI.SetEnemyName(EnemyName);
        }
    }
    private bool inCombat;
    /// <summary>
    /// Is the Enemy in Combat.
    /// </summary>
    public bool InCombat
    {
        get
        {
            return inCombat;
        }
        set
        {
            inCombat = value;
            //animator.SetBool("inCombat", value);
        }
    }

    /// <summary>
    /// Max Hp of Enemy
    /// </summary>
    public int maxHP;
    private int currentHp;
    /// <summary>
    /// Enemy Current Hp
    /// </summary>
    public int CurrentHP
    {
        get
        {
            return currentHp;
        }
        protected set
        {
            currentHp = value;

            //Update UI for enemy Health
            thisEnemyUI.UpdateHealth(currentHp,maxHP);

            if (currentHp <= 0)
            {
                currentHp = 0;
                Die();
            }
        }
    }

    private int shield;
    /// <summary>
    /// Enemy Shield Amount.
    /// </summary>
    public int Shield
    {
        get
        {
            return shield;
        }
        protected set
        {
            shield = value;

            if (shield > maxShield)
                maxShield = value;

            if (shield <= 0)
            {
                shield = 0;
                maxShield = value;
            }

            thisEnemyUI.UpdateShield(shield,maxShield);
        }
    }
    private int maxShield = 0;
    private bool isTargeted;
    /// <summary>
    /// Is enemy being targeted by player.
    /// When enemy is targeted by CombatController to change boarder.
    /// </summary>
    public bool IsTargeted
    {
        get
        {
            return isTargeted;
        }
        protected set
        {
            isTargeted = value;
            TargetIcon.SetActive(value);
        }
    }
    #endregion

    [Header("Boarder Effects")]    
    private List<TargetingType> activeTargetingTypes = new List<TargetingType>();
    private Color SelectedTarget = Color.red;
    private Color InAbilityRange = Color.yellow;

    [Header("Status Effects")]
    #region StatusEffects
    private int galvanizedStacks;
    /// <summary>
    /// Is enemy GalvanizedStacks.
    /// Added for animation or effect later.
    /// </summary>
    public bool IsGalvanized
    {
        get
        {
            return isGalvanized;
        }
        private set
        {
            isGalvanized = value;

            // remove Galvanized from panel
            if (!isGalvanized)
                thisEnemyUI.RemoveEffect(Effects.Buff.Galvanize.ToString());
        }
    }
    private bool isGalvanized;
    /// <summary>
    /// Amount of stacks of Galvanize the enemy have.
    /// </summary>
    public int GalvanizedStacks
    {
        get => galvanizedStacks;
        protected set
        {
            galvanizedStacks = value;
            if (galvanizedStacks <= 0)
                IsGalvanized = false;
            else if (galvanizedStacks >= 1)
                IsGalvanized = true;
        }
    }
    private int powerStacks;
    /// <summary>
    /// Amount of stacks of Power the enemy have.
    /// </summary>
    public int PowerStacks
    {
        get => powerStacks;
        protected set
        {
            powerStacks = value;
            if (powerStacks <= 0)
                IsPowered = false;
            else
                IsPowered = true;
        }
    }
    private bool isPowered;
    /// <summary>
    /// Is enemy powered.
    /// Added for animation or effect later.
    /// </summary>
    public bool IsPowered
    {
        get
        {
            return isPowered;
        }
        protected set
        {
            isPowered = value;

            if (!IsPowered)
                thisEnemyUI.RemoveEffect(Effects.Buff.Power.ToString());
        }
    }
    private int drainStacks;
    /// <summary>
    /// Amount of stacks of Drained the enemy have.
    /// </summary>
    public int DrainedStacks
    {
        get
        {
            return drainStacks;
        }
        protected set
        {
            drainStacks = value;
            if (drainStacks <= 0)
                IsDrained = false;
            else
                IsDrained = true;
        }
    }
    private bool isDrained;
    /// <summary>
    /// Is enemy Drained.
    /// Added for animation or effect later.
    /// </summary>
    public bool IsDrained
    {
        get => isDrained;
        private set
        {
            isDrained = value;

            //Remove drained from panel
            if (!IsDrained)
                thisEnemyUI.RemoveEffect(Effects.Debuff.Drained.ToString());
        }
    }
    #endregion

    [Header("Needed Assets")]
    public Shader outlineShader;
    private Shader defaultShader;
    private Renderer enemyRenderer;
    public GameObject damageTextPrefab;

    /// <summary>
    /// reference to enemy canvas.
    /// </summary>
    public EnemyUI thisEnemyUI;

    /// <summary>
    /// Reference to combat controller.
    /// </summary>
    public CombatController CombatController;

    /// <summary>
    /// This holds the cards we want to have dropped on death
    /// </summary>
    public List<NewChip> dropedCards;

    public GameObject TargetIcon;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerCamera = Camera.main;
        enemyRenderer = GetComponent<Renderer>();
        thisEnemyUI = this.gameObject.GetComponentInChildren<EnemyUI>();

        //Transform enemyCanvasTransform = transform.Find("EnemyCanvas");

        //thisEnemyUI=enemyCanvasTransform.GetComponent<EnemyUI>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        Initialize();
    }

    public virtual void FixedUpdate()
    {
        
    }
    public virtual void Update()
    {

        if (InCombat)
        {
            if(CombatController.CanIMakeAction(this.gameObject))
            {
                StartTurn();                
            }
        }
    }   

    public virtual void Initialize()
    {
        CurrentHP = maxHP;        
        gameObject.name = EnemyName;
        defaultShader=enemyRenderer.material.shader;

        CombatController = GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>();
        enemyTarget = GameObject.FindGameObjectWithTag("Player");

        //UpdateBorderColor();
    }
   /// <summary>
   /// Is called when enemy is attacked by player.
   /// </summary>
   /// <param name="damage"></param>
    public virtual void TakeDamage(int damage)
    {
        // Plays sound of taking damage
        SoundManager.PlayFXSound(SoundFX.DamageTaken, this.gameObject.transform);

        // if has shield
        if (Shield > 0)
        {
            if (damage >= Shield)
            {
                damage -= Shield;
                Shield = 0;
                Debug.Log("Enemy " + name + "Shield destroyed.");
            }
            else
            {
                // Reduce the shield by the damage amount
                Shield -= damage;
                // No remaining damage to apply to HP
                damage = 0;
            }
        }
        CurrentHP -= damage;

        DisplayDamageTaken(damage);
    }    
    /// <summary>
    /// Call when enemy die.
    /// </summary>
    public virtual void Die()
    {
        SoundManager.PlayFXSound(SoundFX.EnemyDefeated,this.gameObject.transform);

        Debug.Log($"{enemyName} has been defeated!");
        CombatController.RemoveCombadant(this.gameObject);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Call at end of enemyies turn.
    /// </summary>
    public virtual void EndTurn()
    {
        //Remove debuffs by 1
        DrainedStacks--;
    }

    /// <summary>
    /// Called when round ends
    /// </summary>
    public virtual void RoundEnd()
    {
        if (GalvanizedStacks > 0)
        {
            Shield += GalvanizedStacks;
            IsGalvanized = false;
        }
    }
    
    /// <summary>
    /// Apply Debuffs to Enemy
    /// </summary>
    /// <param name="debuffToApply"></param>
    /// <param name="debuffStacks"></param>
    public virtual void ApplyDebuff(Effects.Debuff debuffToApply, int debuffStacks)
    {
        //Plays Debuff sound effect
        SoundManager.PlayFXSound(SoundFX.Debuff, this.gameObject.transform);

        switch (debuffToApply)
        {
            case Effects.Debuff.Drained:
                DrainedStacks += debuffStacks;                
                break;
            default:
                break;
        }

        thisEnemyUI.AddEffect(debuffToApply.ToString());
    }

    /// <summary>
    /// Apply Buff to Enemy
    /// </summary>
    /// <param name="buffToApply"></param>
    /// <param name="buffStacks"></param>
    public virtual void ApplyBuff(Effects.Buff buffToApply, int buffStacks)
    {        
        switch (buffToApply)
        {
            case Effects.Buff.Galvanize:
                GalvanizedStacks += buffStacks;                
                break;
            case Effects.Buff.Power:
                PowerStacks+= buffStacks;                
                break;
        }
        thisEnemyUI.AddEffect(buffToApply.ToString());
    }       

    /// <summary>
    /// Give enemy shield.
    /// </summary>
    /// <param name="shieldAmount"></param>
    public virtual void ApplyShield(int shieldAmount)
    {
        //Restore Shield
        Shield += shieldAmount;

        Debug.Log("Shield Restored: " + shield);
    }

    /// <summary>
    /// Updates the targeting state of the enemy based on the specified targeting type and its active status.
    /// Adds the targeting type to the active list if it starts targeting, or removes it if it stops targeting.
    /// </summary>
    /// <param name="targetingType">The type of targeting to set (e.g., CombatController, Ability).</param>
    /// <param name="isTargeted">Indicates whether the specified targeting type is active (true) or inactive (false).</param>
    public void SetTarget(TargetingType targetingType, bool isTargeted)
    {
        if (isTargeted)
        {
            if (!activeTargetingTypes.Contains(targetingType))
            {
                // Add the targeting type.
                activeTargetingTypes.Add(targetingType);
            }
        }
        else
        {
            // Remove the targeting type.
            activeTargetingTypes.Remove(targetingType);
        }

        // Determine if CombatController is active and update IsTargeted.
        IsTargeted = activeTargetingTypes.Contains(TargetingType.CombatController);        
    }
   
    /// <summary>
    /// Base Perform Intent.
    /// Make sure the base is run after you perform an action.
    /// </summary>
    protected virtual void PerformIntent()
    {
        if (this.gameObject != null)
            CombatController.TurnUsed(this.gameObject);
    }

    /// <summary>
    /// Start Combat turn
    /// </summary>
    protected virtual void StartTurn()
    {
        //Remove Shield if there is shield
        if (Shield > 0)
            Shield = 0;

        //Remove Buffs
        PowerStacks--;
        GalvanizedStacks--;

        //Look at player
        this.gameObject.transform.LookAt(EnemyTarget.transform);

        //Check if player is in range
        if (DistanceToPlayer <= AttackRange)
        {
            agent.ResetPath();
            PerformIntent();
        }
        else
        {
            // move to player
            agent.SetDestination(EnemyTarget.transform.position);
        }
    }

    /// <summary>
    /// Display damage taken in game.
    /// </summary>
    /// <param name="damage"></param>
    protected virtual void DisplayDamageTaken(int damage)
    {
        // Instantiate the damage text prefab
        GameObject damageIndicator = Instantiate(damageTextPrefab, this.gameObject.transform.position + Vector3.up * 2f, Quaternion.identity);

        // Set the text to display the damage amount
        TextMeshPro textMesh = damageIndicator.GetComponent<TextMeshPro>();
        textMesh.text = $"-{damage}";

        // Ensure the text faces the camera
        damageIndicator.transform.LookAt(Camera.main.transform);
        damageIndicator.transform.Rotate(0, 180, 0); // Correct for backward text
    }
    [ContextMenu("Test")]
    public void Test()
    {
        DisplayDamageTaken(10);
    }
    /// <summary>
    /// Updates the enemy's border color and shader properties based on the currently active targeting types.
    /// The color and outline properties are determined by the highest-priority active targeting type.
    /// </summary>
    //private void UpdateBorderColor()
    //{ 
    //    if (activeTargetingTypes.Count == 0)
    //    {
    //        if (enemyRenderer.material.shader != defaultShader)
    //        {
    //            enemyRenderer.material.shader = defaultShader;
    //        }

    //        // No CombatController targeting.
    //        IsTargeted = false;
    //        return;
    //    }

    //    // Check for the highest-priority targeting type.
    //    TargetingType highestPriorityType = activeTargetingTypes[activeTargetingTypes.Count - 1];

    //    // Set shader only if it has changed.
    //    if (enemyRenderer.material.shader != outlineShader)
    //    {
    //        enemyRenderer.material.shader = outlineShader;
    //    }

    //    // Assign the color based on the targeting type.
    //    switch (highestPriorityType)
    //    {
    //        case TargetingType.CombatController:
    //            enemyRenderer.material.SetColor("_OutlineColor", SelectedTarget);
    //            //IsTargeted = true;
    //            break;
    //        case TargetingType.Ability:
    //            enemyRenderer.material.SetColor("_OutlineColor", InAbilityRange);
    //            break;
    //    }

    //    // Set the outline width.
    //    enemyRenderer.material.SetFloat("_OutlineWidth", 0.1f);
    //}

}