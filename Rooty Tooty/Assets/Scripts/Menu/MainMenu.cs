using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // used whenever we want to change scenes in Unity

public class MainMenu : MonoBehaviour
{
    private static MainMenu instance = null;
    public static MainMenu Instance { get => instance; }

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
    // function that is called when the play button is pressed
    public void PlayGame ()
    {
        //Reset local data if gameManager exists (should exist)
        if (gameManagerScript.instance != null)
        {
            gameManagerScript.instance.ResetSave();
            // Have GameManager load the game
            gameManagerScript.instance.LoadGame();

        }
    }
    public void QuitGame ()
    {
        Debug.Log("Game has been quit");
        Application.Quit();
    }

}