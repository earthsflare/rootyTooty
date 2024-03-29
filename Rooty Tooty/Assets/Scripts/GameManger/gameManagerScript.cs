using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//GameManager Controls Holds Game Save Data, and game freezing mechanic
public class gameManagerScript : MonoBehaviour
{
    // manages over a freeze/unfreeze game function (not in use)
    // 

    //public Text pointsText;
    public static gameManagerScript instance;
    public static bool GameIsFrozen = false;

    [Header("Game Properties")]
    //Is set in the inspector
    [SerializeField] private bool usingPlayFab = false;
    public bool UsingPlayFab { get => usingPlayFab; }

    [Header("Read Only")]
    #region Save Data
    [Header("Game Save Properties")]
    private bool newGame = true;
    //Game File saves every time player enters a scene (if implemented, also when player manual saves)
    //GameManager saves every time player enters a scene or reaches a checkpoint 
    [SerializeField] private Vector2 spawnPosition = new Vector2(-18f, -4f);
    [SerializeField] private int spawnSceneIndex;

    public bool NewGame { get => newGame; }
    public Vector2 SpawnPosition { get => spawnPosition; }

    public void ToggleNewGame(bool b) { newGame = b; }
    public void SetSpawnPosition(Vector2 newSpawn) { spawnPosition = newSpawn; }
    public void SetSpawnSceneIndex(int levelIndex) { spawnSceneIndex = levelIndex; }
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
        SetSpawnSceneIndex(sceneIndex);
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
            if (menuManager != null)
                menuManager.gameObject.SetActive(true);
            if (cameraManager != null)
                cameraManager.SetActive(true);
            if (healthUICanvas != null)
                healthUICanvas.gameObject.SetActive(true);
        }
    }

    #region Save Load 
    //Loads current local data and spawns in scene
    public void StartGame()
    {
        freezeGame();

        SaveData saveData;

        if (usingPlayFab)
        {
            if (!PlayfabManager.Instance.FinishedLoadingGame.HasValue)
            {
                StartCoroutine(StartGame_Playfab());
                return;
            }
            else if (!PlayfabManager.Instance.FinishedLoadingGame.Value)
                return;

            saveData = PlayfabManager.Instance.LoadedSave;
            PlayfabManager.Instance.SetLoadedSave(null);
            PlayfabManager.Instance.SetFinishedLoadingGame(null);
        }
        else
        {
            saveData = SaveManager.LoadGameFromFile();
        }

        if (newGame || saveData == null)
        {
            if (!newGame)
                Debug.Log("Save File not Found");

            spawnPosition = new Vector2(-19f, -5.5801f);
            spawnSceneIndex = (int)LevelIndex.Level1;

            //Update Player Data
            if (Player.instance != null)
            {
                Player.instance.Health.SetLife(5);
                Player.instance.ToggleRoll(true);
                Player.instance.ToggleWallJump(false);
                Player.instance.Jump.SetMaxJumps(1);
                Player.instance.Aim.ToggleFireball(false);
            }
        }

        if (saveData != null)
        {
            spawnPosition = new Vector2(saveData.spawnPosition[0], saveData.spawnPosition[1]);
            spawnSceneIndex = saveData.spawnSceneIndex;

            //Update Player Data
            if (Player.instance != null)
            {
                if (Player.instance.Health != null)
                    Player.instance.Health.SetLife(saveData.playerHealth);
                if (Player.instance.Roll != null)
                    Player.instance.ToggleRoll(saveData.rollUnlocked);
                if (Player.instance.WallJump != null)
                    Player.instance.ToggleWallJump(saveData.wallJumpUnlocked);
                if (Player.instance.Jump != null)
                    Player.instance.Jump.SetMaxJumps((saveData.doubleJumpUnlocked) ? 2 : 1);
                if (Player.instance.Aim != null)
                    Player.instance.Aim.ToggleFireball(saveData.fireballUnlocked);
            }
        }

        levelManager.instance.LoadLevel(spawnSceneIndex);
    }
    private IEnumerator StartGame_Playfab()
    {
        StartCoroutine(SaveManager.LoadGameFromPlayfab(PlayfabManager.Instance.UserPlayfabID));

        //Check if login was successful
        while(!PlayfabManager.Instance.ReLoginAttempt.HasValue)
            yield return null;
        if (!PlayfabManager.Instance.ReLoginAttempt.Value)
        {
            PlayfabManager.Instance.NotifyErrorLoadingGame();
            yield break;
        }

        //Check if save data was found
        while (!PlayfabManager.Instance.FinishedLoadingGame.Value)
            yield return null;

        StartGame();
    }

    //Saves game to file
    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        saveData.spawnPosition = new float[] { spawnPosition.x, spawnPosition.y };
        saveData.spawnSceneIndex = spawnSceneIndex;
        saveData.playerHealth = Player.instance.Health.getHealth();

        saveData.rollUnlocked = Player.instance.Roll.enabled;
        saveData.wallJumpUnlocked = Player.instance.WallJump.enabled;
        saveData.doubleJumpUnlocked = Player.instance.Jump.MaxJumps > 1;
        saveData.fireballUnlocked = Player.instance.Aim.FireballEnabled;

        if (usingPlayFab)
        {
            if (!PlayfabManager.Instance.gameSaved.HasValue)
            {
                StartCoroutine(SaveGame_Playfab(saveData));
            }
            return;
        }

        SaveManager.SaveGameToFile(saveData);
    }
    private IEnumerator SaveGame_Playfab(SaveData saveData)
    {
        StartCoroutine(SaveManager.SaveGameToPlayfab(saveData));
        while(!PlayfabManager.Instance.gameSaved.HasValue)
            yield return null;
        if (PlayfabManager.Instance.gameSaved.Value == true)
        {
            PlayfabManager.Instance.gameSaved = null;
            Debug.Log("Save Successful");
            yield break;
        }
        yield return new WaitForSeconds(1);
        Debug.Log("Trying to Save");
        StartCoroutine(SaveGame_Playfab(saveData));
    }
    //Resets current local data and save file
    public void ResetSave()
    {
        newGame = true;
    }
    #endregion

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
}
