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

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        if (instance != this)
            return;

        instance = null;
        gameManagerScript.UndoDontDestroyOnLoad(gameObject);
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
        if (CMBoundingShape.I != null && cmConfiner != null)
            cmConfiner.m_BoundingShape2D = CMBoundingShape.BoundingCollider;

    }
}
