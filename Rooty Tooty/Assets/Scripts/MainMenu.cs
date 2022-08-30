using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // used whenever we want to change scenes in Unity

public class MainMenu : MonoBehaviour
{
    // function that is called when the play button is pressed
    public void PlayGame ()
    {
        // load the next level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // loads the next level in the queue
        // done by getting the currently loaded level and increase it by 1
    }

    public void QuitGame ()
    {
        Debug.Log("Game has been quit");
        Application.Quit();
    }

    public void OptionsGame ()
    {

    }
}