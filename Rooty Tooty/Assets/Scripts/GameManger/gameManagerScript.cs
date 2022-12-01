using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class gameManagerScript : MonoBehaviour
{
    //public Text pointsText;
    public static gameManagerScript instance;

    //[SerializeField] private float freezeGameTime = 1f;
    public static bool GameIsFrozen = false;

    [SerializeField] private GameObject playerPrefab;
    [Header("Read Only")]

    #region Save Data
    [Header("Game Save Properties")]

    //Game File saves every time player enters a scene (if implemented, also when player manual saves)
    //GameManager saves every time player enters a scene or reaches a checkpoint 


    [SerializeField] private Vector2 spawnPosition = new Vector2(-18f, -4f);
    [SerializeField] private int spawnSceneIndex;

    [SerializeField] private int playerHealth = 5;

    [SerializeField] private bool doubleJumpUnlocked = false;
    [SerializeField] private bool fireballUnlocked = false;
    [SerializeField] private bool rollUnlocked = false;
    [SerializeField] private bool wallJumpUnlocked = false;

    public Vector2 SpawnPosition { get => spawnPosition; }

    public void SetSpawnPosition(Vector2 newPos) { spawnPosition = newPos; }
    public void SetSpawnSceneIndex(int index) { spawnSceneIndex = index; }
    #endregion

    #region Object References
    [Header("Object References")]
    [SerializeField] GameObject cameraManager = null;
    [SerializeField] Canvas healthUICanvas = null;
    [SerializeField] private MenuManager menuManager = null;

    #endregion

    private void Awake()
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

    private void Start()
    {
        StartUp(SceneManager.GetActiveScene().buildIndex);
    }
    public void StartUp(int sceneIndex)
    {
        spawnSceneIndex = sceneIndex;
        unfreezeGame();

        if (MenuManager.instance != null)
        {
            MenuManager.instance.Resume();
            MenuManager.GameIsOver = false;
        }
        if (levelManager.instance.IsLevelTitle(sceneIndex))
        {
            if (Player.instance != null)
            {
                Player.DeactivatePlayer();
            }
            if (MenuManager.instance != null)
            {
                menuManager = MenuManager.instance;
                menuManager.gameObject.SetActive(false);
            }
            if (cameraManager != null)
                cameraManager.SetActive(false);
            if (healthUICanvas != null)
                healthUICanvas.gameObject.SetActive(false);
        }
        else
        {
            if (Player.instance != null)
                Player.ActivatePlayer();
            if (menuManager != null)
                menuManager.gameObject.SetActive(true);
            if (cameraManager != null)
                cameraManager.SetActive(true);
            if (healthUICanvas != null)
                healthUICanvas.gameObject.SetActive(true);
        }
    }

    #region Save Data Related Function

    //Loads current local data and spawns in scene
    public void LoadGame()
    {
        freezeGame();

        //Update Player Data
        if(Player.instance != null)
        {
            if (Player.instance.Health != null)
                Player.instance.Health.SetLife(playerHealth);

            if (Player.instance.Jump != null)
                Player.instance.Jump.SetMaxJumps((doubleJumpUnlocked) ? 2 : 1);
            if (Player.instance.Aim != null)
                Player.instance.Aim.ToggleFireball(fireballUnlocked);
            if (Player.instance.Roll != null)
                Player.instance.ToggleRoll(rollUnlocked);
            if(Player.instance.WallJump != null)
                Player.instance.ToggleWallJump(wallJumpUnlocked);
        }

        levelManager.instance.LoadLevel(spawnSceneIndex);
    }

    //Resets current local data and save file
    public void ResetSave()
    {
        spawnPosition = new Vector2(-19f, -5.5801f);
        spawnSceneIndex = (int)LevelIndex.Level1;

        playerHealth = 5;

        rollUnlocked = false;
        doubleJumpUnlocked = false;
        fireballUnlocked = false;
        wallJumpUnlocked = false;
    }

    #endregion

    public void OnSpawnPlayerPrefab()
    {
        Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
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
