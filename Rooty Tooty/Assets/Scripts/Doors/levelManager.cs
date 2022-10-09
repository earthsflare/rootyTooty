using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // for switching scenes

//Controls player level transition
public class levelManager : MonoBehaviour
{
    private static levelManager _levelManager;
    public static levelManager instance { get { return _levelManager; } }

    [SerializeField] private Vector2 nextLevelPosition = new Vector2(-45f, -7f);
    public void SetNextLevelPos(Vector2 p) { nextLevelPosition = p; }
    
    private void Awake()
    {
        #region Setup Singleton
        if (_levelManager == null)
            _levelManager = this;
        else if (_levelManager != this)
            Destroy(this);
        #endregion

        #region Find and set player position
        GameObject p = FindObjectOfType<PlayerMovement>().gameObject;
        if(p != null)
            p.transform.position = instance.nextLevelPosition;
        #endregion
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
