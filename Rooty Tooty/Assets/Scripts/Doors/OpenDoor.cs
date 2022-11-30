using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private bool playerDetected; // a boolean checking for if a player is detected in the range
    
    [SerializeField] private Vector2 nextDoorPos = Vector2.zero;

    [SerializeField] private LayerMask whatIsPlayer;// what to check for to change the bool

    [SerializeField]
    private string sceneName;

    SceneSwitch sceneSwitch;

    private static bool doorTraveled = false;

    private void Start()
    {
        if (!doorTraveled)
            return;

        sceneSwitch = FindObjectOfType<SceneSwitch>();
        levelManager.instance.animator.SetTrigger("FadeIn");
        doorTraveled = false;
    }

    private void Update()
    {
        playerDetected = Physics2D.OverlapBox(transform.position, transform.lossyScale, 0, whatIsPlayer);
        if (playerDetected == true)
        {
            doorTraveled = true;

            gameManagerScript.instance.SetSpawnPosition(nextDoorPos);
            levelManager.instance.FadeToLevel(sceneName);
        }
    }
}
