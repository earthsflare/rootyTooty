using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance = null;

    //Object References
    [SerializeField] private Camera mainCam = null;
    [SerializeField] private CinemachineVirtualCamera cmVC = null;
    [SerializeField] private CinemachineConfiner cmConfiner = null;

    //Old parent transform of current boundingShape
    private Transform oldParent = null;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            gameObject.SetActive(false);

        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += StartUp;
    }
    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= StartUp;

        if (instance != this)
            return;

        instance = null;
        //gameManagerScript.UndoDontDestroyOnLoad(gameObject);
    }
    public void StartUp(UnityEngine.SceneManagement.Scene current, UnityEngine.SceneManagement.Scene next)
    {
        if (gameManagerScript.instance != null)
            if (levelManager.instance.IsLevelTitle(next.buildIndex))
                return;

        while (Camera.main != mainCam)
        {
            Destroy(Camera.main.gameObject);
        }

        if (cmConfiner.m_BoundingShape2D != null)
            Destroy(cmConfiner.m_BoundingShape2D);
    }

    private void Awake()
    {
        if (mainCam == null)
            mainCam = GetComponentInChildren<Camera>();
        if(cmVC == null)
        {
            if (cmConfiner != null)
                cmVC = cmConfiner.GetComponent<CinemachineVirtualCamera>();
            else
                cmVC = GetComponentInChildren<CinemachineVirtualCamera>();
        }
        if(cmConfiner == null)
        {
            if (cmVC != null)
                cmConfiner = cmVC.GetComponent<CinemachineConfiner>();
            else
                cmConfiner = GetComponentInChildren<CinemachineConfiner>();
        }
    }
    private void Start()
    {
        if (Player.instance != null && cmVC != null)
            cmVC.Follow = Player.instance.transform;
    }
    public static void UpdateConfiner(PolygonCollider2D c)
    {
        if (instance.cmConfiner == null)
            return;

        //Put back old bounding box
        if(instance.cmConfiner.m_BoundingShape2D != null)
        {
            instance.cmConfiner.m_BoundingShape2D.transform.parent = instance.oldParent;
        }

        instance.oldParent = c.transform.parent;

        c.transform.parent = instance.transform;

        instance.cmConfiner.m_BoundingShape2D = c;
    }
}