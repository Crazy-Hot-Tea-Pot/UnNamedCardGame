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

    /// <summary>
    /// Have to manually Update this for now.
    /// TODO add a generator that auto populates list of enemy types.
    /// </summary>
    public enum EnemyType
    {
        Looter,
        SecurityDrone,
        Maintenancebot,
        TicketVendor,
        Garbagebot,
        GangLeader,
        Inspector
    }

    public List<GameObject> EnemiesInLevel;

    public List<GameObject> CombatEnemies
    {
        get
        {
            return combatEnemies;
        }
        private set
        {
            combatEnemies = value;
        }
    }


    private static EnemyManager instance;
    private List<GameObject> combatEnemies = new();
    [SerializeField] 
    private List<GameObject> enemyPrefabs;
    private Dictionary<EnemyType, GameObject> enemyPrefabDict = new();

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

        PopulateDictionary();
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
    /// Add Enemy to Combat Enimes list.
    /// </summary>
    /// <param name="enemy"></param>
    public void AddCombatEnemy(GameObject enemy)
    {
        CombatEnemies.Add(enemy);
    }

    /// <summary>
    /// Get Enemy Object for Enemy Type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetEnemyPrefab(EnemyType type)
    {
        return enemyPrefabDict.ContainsKey(type) ? enemyPrefabDict[type] : null;
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

    private void PopulateDictionary()
    {
        enemyPrefabDict.Clear();
        foreach (var prefab in enemyPrefabs)
        {
            if (prefab != null)
            {
                string normalizedPrefabName = prefab.name.Replace(" ", "").ToLower();
                foreach (EnemyType type in System.Enum.GetValues(typeof(EnemyType)))
                {
                    string normalizedEnumName = type.ToString().ToLower(); // Normalize enum name (lowercase)
                    if (normalizedPrefabName == normalizedEnumName)
                    {
                        enemyPrefabDict[type] = prefab;
                        break; // Stop looping once we find a match
                    }
                }
            }
        }
    }

    private void GetAllEnemiesInLevel()
    {
        CombatEnemies.Clear();
        EnemiesInLevel.Clear();
        EnemiesInLevel.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }
    /// <summary>
    /// TODO
    /// Get Current Level data from Story Manager and spawn those enemies into the scene.
    /// </summary>
    private void SpawnEnemiesForLevel()
    {

    }

    private void StartCombat()
    {

    }

    private void EndCombat()
    {
        CombatEnemies.Clear();
    }
    private void SceneChange(Levels newLevel)
    {

        switch (newLevel)
        {
            case Levels.Title:
            case Levels.Loading:
            case Levels.WorkShop:
                break;
            default:
                SpawnEnemiesForLevel();
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
