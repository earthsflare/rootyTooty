using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // for switching scenes

public class SceneSwitch : MonoBehaviour
{

    public static string prevScene;
    public static string currentScene;
    [SerializeField] private GameObject playerPrefab;

    public void Awake()
    {
        if (playerPrefab == null)
        {
            //FindObjectOfType<PlayerMovement>().transform.position = nextLevelPosition;
        }
    }

    public virtual void Start()
    {

        currentScene = SceneManager.GetActiveScene().name;
    }

    public void SwitchScene(string sceneName)
    {

        prevScene = currentScene;
        SceneManager.LoadScene(sceneName);
    }
}