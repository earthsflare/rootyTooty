using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class gameManagerScript : MonoBehaviour
{
    //public Text pointsText;
    public static gameManagerScript instance;

    //a variable that keeps track of whether or not our game is paused
    public static bool GameIsPaused = false;

    public static bool GameIsOver = false;

    [SerializeField] private GameObject prefab;
    [Header("Read Only")]
    [SerializeField] private Vector2 spawnPosition;
    //[SerializeField] private bool random;


    [Header("Object References")]
    //gameoverscreen 
    //public static bool GameIsOver = false;
    public GameObject goMenuUI;

    public GameObject pauseMenuUI;

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
        if (Player.instance == null)
        {
            OnSpawnPlayerPrefab();
        }
    }

    void Update()
    {
        if (GameIsOver == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
        else
        {
            Debug.Log("Works");
        }
    }

    // when resuming, exit pause menu, continue time in our game, and change GameIsPaused to false
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    // when pausing, bring up pause menu, freeze time in our game, and change our GameIsPaused to true
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
        Resume();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }


    public void Setup()
    {
        goMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsOver = true;
        //  pointsText.text = score.ToString() + " POINTS";
    }

    public void ExitButton()
    {
        //before or after
        goMenuUI.SetActive(false);
        SceneManager.LoadScene("Menu");
    }

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
}
