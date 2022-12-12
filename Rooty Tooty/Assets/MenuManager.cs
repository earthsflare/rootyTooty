using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public static bool GameIsPaused = false;

    public static bool GameIsOver = false;

    [Header("Object References")]
    //gameoverscreen 
    //public static bool GameIsOver = false;
    public GameObject goMenuUI;

    public GameObject pauseMenuUI;

    private EventSystem eventSystem = null;
    private StandaloneInputModule standaloneInputModule = null;

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

        if (eventSystem == null)
            eventSystem = GetComponent<EventSystem>();
        if (standaloneInputModule == null)
            standaloneInputModule = GetComponent<StandaloneInputModule>();
    }
    private void Start()
    {
        //This is so there's only one Event System on the scene at a time
        if(gameManagerScript.instance != null)
        {
            eventSystem.enabled = false;
            Destroy(standaloneInputModule);
        }
        else
            eventSystem.enabled = true;
    }
    void Update()
    {
        if (GameIsOver == false)
        {
            goMenuUI.SetActive(false);
            if (gameManagerScript.GameIsFrozen == true)
            {
                //Debug.Log("GameIsFrozen");
            }
            else
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
        }
        else
        {
            //Debug.Log("Works");
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
        goMenuUI.SetActive(false);
        SceneManager.LoadScene(levelManager.instance.TitleLevelIndex);
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
}
