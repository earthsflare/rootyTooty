using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMBoundingShape : MonoBehaviour
{

    [Header("Object References")]
    [SerializeField] private PolygonCollider2D cameraBounds = null;
    [SerializeField] private CMPlayerFinder playerFinder = null;

    public bool CameraBoundActive() 
    {
        if (cameraBounds == null)
            return false;
        return cameraBounds.gameObject.activeInHierarchy; 
    }
    private void Start()
    {
        //PlayerFinder should be a different child object of CMBoundingShape's parent
        if (playerFinder == null && transform.parent != null)
            transform.parent.gameObject.GetComponentInChildren<CMPlayerFinder>();
        
        if (cameraBounds == null)
        {
            cameraBounds = GetComponent<PolygonCollider2D>();
            if (cameraBounds == null)
            {
                Debug.Log(gameObject.name + " Does not have a cameraBounds set");
                gameObject.SetActive(false);
                return;
            }
        }
        if (playerFinder == null)
        {
            playerFinder = transform.parent.GetComponentInChildren<CMPlayerFinder>();
            if (playerFinder == null)
            {
                Debug.Log(gameObject.name + " does not have a PlayerFinder");
                gameObject.SetActive(false);
                return;
            }
        }

        cameraBounds.gameObject.SetActive(false);
        playerFinder.gameObject.SetActive(true);
    }
    public void FoundPlayer()
    {
        if (cameraBounds.gameObject.activeInHierarchy)
            return;

        cameraBounds.gameObject.SetActive(true);

        CameraManager.UpdateConfiner(cameraBounds);
    }
}
