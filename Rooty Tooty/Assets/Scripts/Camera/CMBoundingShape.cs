using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMBoundingShape : MonoBehaviour
{

    [Header("Object References")]
    //Bounds
    [SerializeField] private PolygonCollider2D cameraBounds = null;
    [SerializeField] private SpriteRenderer camBoundVisualAid = null;
    [SerializeField] private CMPlayerFinder playerFinder = null;

    public bool CameraBoundActive() 
    {
        if (cameraBounds == null)
            return false;
        return cameraBounds.gameObject.activeInHierarchy; 
    }
    private void Start()
    {
        playerFinder.gameObject.SetActive(true);
        if(camBoundVisualAid != null)
            camBoundVisualAid.enabled = false;
    }
    public void FoundPlayer()
    {
        //Ignore call if camerabounds is already enabled
        if (cameraBounds.gameObject.activeInHierarchy)
            return;

        cameraBounds.gameObject.SetActive(true);

        CameraManager.UpdateConfiner(cameraBounds);
    }
}
