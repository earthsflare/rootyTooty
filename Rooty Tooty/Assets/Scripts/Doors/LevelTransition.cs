using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    private bool playerDetected; // a boolean checking for if a player is detected in the range

    [Header("Properties")]
    [SerializeField] private LayerMask whatIsPlayer;// what to check for to change the bool
    [SerializeField] private LevelIndex nextScene;
    [SerializeField] private int spawnID;

    private bool doorTraveled = false;

    private void Update()
    {
        playerDetected = Physics2D.OverlapBox(transform.position, transform.lossyScale, 0, whatIsPlayer);
        if (playerDetected && !doorTraveled)
        {
            doorTraveled = true;
            SpawnLocation.StartSearching((LevelIndex)(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex), spawnID);
            levelManager.instance.FadeToLevel(nextScene);
        }
    }
}
