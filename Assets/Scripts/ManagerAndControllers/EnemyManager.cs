using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    public List<GameObject> EnemiesInLevel;

    public List<GameObject> CombatEnemies
    {
        get
        {
            return combatEnemies;
        }
        set
        {
            combatEnemies = value;
        }
    }


    private static EnemyManager instance;
    private List<GameObject> combatEnemies;

    void Awake()
    {
        // Check if another instance of the GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep this object between scenes

        }
        else
        {
            Destroy(gameObject);  // Destroy duplicates
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnStartCombat += StartCombat;
        GameManager.Instance.OnEndCombat += EndCombat;
        GameManager.Instance.OnSceneChange += SceneChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Add Enemy to enimes list.
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemy(GameObject enemy)
    {
        CombatEnemies.Add(enemy);
    }
    /// <summary>
    /// Remove defeated enemy from game.
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEnemy(GameObject enemy)
    {
        EnemiesInLevel.Remove(enemy);
        CombatEnemies.Remove(enemy);
        
        Destroy(enemy);
    }
    private void GetAllEnemiesInLevel()
    {
        CombatEnemies.Clear();
        EnemiesInLevel.Clear();
        EnemiesInLevel.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }
    private void StartCombat()
    {
        Debug.Log("[ManagerName] Combat started.");
    }
    private void EndCombat()
    {
        CombatEnemies.Clear();
    }
    private void SceneChange(Levels newLevel)
    {
        Debug.Log($"[ManagerName] Scene changed to {newLevel}.");

        switch (newLevel)
        {
            case Levels.Title:
            case Levels.Loading:
            case Levels.WorkShop:
                break;
            default:                
                GetAllEnemiesInLevel();
                break;
        }
    }
    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStartCombat -= StartCombat;
            GameManager.Instance.OnEndCombat -= EndCombat;
            GameManager.Instance.OnSceneChange -= SceneChange;
        }
    }
}
