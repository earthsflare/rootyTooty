using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_LookAtPlayer : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera cVM = null;

    private void Start()
    {
        if (Player.instance != null && cVM != null)
            cVM.Follow = Player.instance.transform;
    }
}
