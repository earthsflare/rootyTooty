using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManagerScript : MonoBehaviour
{
    //public Text pointsText;
    public static gameManagerScript instance;

    //[SerializeField] private float freezeGameTime = 1f;
    public static bool GameIsFrozen = false;

    [SerializeField] private GameObject prefab;
    [Header("Read Only")]

    #region Save Data
    [Header("Game Save Properties")]

    //Game File saves every time player enters a scene (if implemented, also when player manual saves)
    //GameManager saves every time player enters a scene or reaches a checkpoint 


    [SerializeField] private Vector2 spawnPosition = new Vector2(-19f, -5.5801f);
    [SerializeField] private int spawnSceneIndex;

    [SerializeField] private int playerHealth = 5;

    [SerializeField] private bool rollUnlocked = false;
    [SerializeField] private bool doubleJumpUnlocked = false;
    [SerializeField] private bool fireballUnlocked = false;
    [SerializeField] private bool wallJumpUnlocked = false;

    public Vector2 SpawnPosition { get => spawnPosition; }

    public void SetSpawnPosition(Vector2 newPos) { spawnPosition = newPos; }
    public void SetSpawnSceneIndex(int index) { spawnSceneIndex = index; }
    #endregion

    #region Not Saved Game Data
    [Header("Other Game Properties")]
    [SerializeField] private int titleSceneIndex;

    public int TitleLevelIndex { get => titleSceneIndex; }
    public void SetTitleIndex(int i) { instance.titleSceneIndex = i; }

    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        unfreezeGame();
    }

    #region Save Data Related Function

    //Loads current local data and spawns in scene
    public void LoadGame()
    {
        freezeGame();



        //Update Player Data

        //levelManager.instance.FadeToLevel();
    }

    //Resets current local data and save file
    public void ResetSave()
    {
        spawnPosition = new Vector2(-19f, -5.5801f);
        spawnSceneIndex = titleSceneIndex + 1;

        playerHealth = 5;

        rollUnlocked = false;
        doubleJumpUnlocked = false;
        fireballUnlocked = false;
        wallJumpUnlocked = false;
    }

    #endregion

    public void OnSpawnPlayerPrefab()
    {
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }

    public static void UndoDontDestroyOnLoad(GameObject g)
    {
        if (g == null)
            return;
        SceneManager.MoveGameObjectToScene(g, SceneManager.GetActiveScene());
        
    }

    public void freezeGame()
    {
        Time.timeScale = 0f;
        GameIsFrozen = true;
    }

    public void unfreezeGame()
    {
        Time.timeScale = 1f;
        GameIsFrozen = false;
    }
    
    /*
    void Update()
    {
        if (GameIsFrozen == true)
        {
            freezeGameTime -= Time.deltaTime;
            if (freezeGameTime <= 0)
            {
                unfreezeGame();
            }
        }
    }
    */
}
