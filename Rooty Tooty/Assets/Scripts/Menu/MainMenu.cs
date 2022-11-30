using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // used whenever we want to change scenes in Unity

public class MainMenu : MonoBehaviour
{

    private void Awake()
    {
        gameManagerScript.instance.SetTitleIndex(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void Start()
    {
        MenuManager.instance.Resume();
        MenuManager.GameIsOver = false;
    }


    // function that is called when the play button is pressed
    public void PlayGame ()
    {
        //Set next player position if  levelManager exists (after game over)
        if (gameManagerScript.instance != null)
            gameManagerScript.instance.SetSpawnPosition(new Vector2(-45f, -7f));
        
        // load the next level
        SceneManager.LoadScene(gameManagerScript.instance.TitleLevelIndex + 1); // loads the next level in the queue
        // done by getting the currently loaded level and increase it by 1
    }

    public void QuitGame ()
    {
        Debug.Log("Game has been quit");
        Application.Quit();
    }

}