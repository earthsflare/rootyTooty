using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // for switching scenes

public class SceneSwitch : MonoBehaviour
{
    // 
    public virtual void Start()
    {
        // 
    }

    public void SwitchScene(string sceneName)
    {
        // 
        SceneManager.LoadScene(sceneName);
    }
}
