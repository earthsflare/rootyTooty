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

    void Update()
    {
        if (GameIsOver == false)
        {
            goMenuUI.SetActive(false);
            if (gameManagerScript.GameIsFrozen == true)
            {
                Debug.Log("GameIsFrozen");
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
}
