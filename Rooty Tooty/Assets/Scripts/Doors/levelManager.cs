using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // for switching scenes

//Controls player level transition and fading animation
public class levelManager : MonoBehaviour
{
    private static levelManager _levelManager;
    public static levelManager instance { get { return _levelManager; } }

    public Animator animator;

    private string levelToLoad;

    [SerializeField] private float fadeAlphaSpeed;
    //[SerializeField] private float initialAlpha;

    private void Awake()
    {
        animator.SetFloat("AnimationSpeed", fadeAlphaSpeed);
        #region Setup Singleton
        if (_levelManager == null)
            _levelManager = this;
        else if (_levelManager != this)
            Destroy(this);
        #endregion
    }

    private void Start()
    {
        StartUp();
    }

    private void StartUp()
    {
        //Find and set player position
        if (Player.instance != null)
            Player.instance.transform.position = gameManagerScript.instance.SpawnPosition;
        gameManagerScript.instance.SetSpawnSceneIndex(SceneManager.GetActiveScene().buildIndex);
    }

    public void FadeToLevel(string sceneName)
    {
        levelToLoad = sceneName;
        animator.SetTrigger("FadeOut");
    }
    public void OnFadeComplete()
    {

        SceneManager.LoadScene(levelToLoad);
        if (gameManagerScript.instance != null)
            gameManagerScript.instance.StartUp(SceneManager.GetSceneByName(levelToLoad).buildIndex);
        animator.SetTrigger("FadeIn");

    }

    public void LoadLevel(string sceneName)
    {
        if (gameManagerScript.instance != null)
            gameManagerScript.instance.StartUp(SceneManager.GetSceneByName(sceneName).buildIndex);
        StartUp();
        SceneManager.LoadScene(sceneName);
        
    }
    public void LoadLevel(int sceneIndex)
    {
        if (gameManagerScript.instance != null)
            gameManagerScript.instance.StartUp(sceneIndex);
        StartUp();
        SceneManager.LoadScene(sceneIndex);
        
    }
}
