using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // used whenever we want to change scenes in Unity

public class MainMenu : MonoBehaviour
{
    private static MainMenu instance = null;
    public static MainMenu Instance { get => instance; }

    [Header("Object References")]
    [SerializeField] private TMP_Text newGameTxt;
    private void Awake()
    {
        if(instance == null)
            instance = this;

        //Change newGame to reflect if save is a newgame or not
        if(newGameTxt != null)
        {
            if(SaveManager.LoadGameFromFile() == null)
            {
                gameManagerScript.instance.ToggleNewGame(false);
                newGameTxt.text = "New Game";
            }
            else
            {
                gameManagerScript.instance.ToggleNewGame(true);
                newGameTxt.text = "Continue";
            }
        }
    }
    // function that is called when the play button is pressed
    public void PlayGame ()
    {
        //Reset local data if gameManager exists (should exist)
        if (gameManagerScript.instance != null)
        {
            // Have GameManager load the game
            gameManagerScript.instance.StartGame();

        }
    }
    public void QuitGame ()
    {
        Debug.Log("Game has been quit");
        Application.Quit();
    }

}