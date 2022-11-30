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
        //Find and set player position
        if (Player.instance != null)
            Player.instance.transform.position = gameManagerScript.instance.SpawnPosition;
        gameManagerScript.instance.SetSpawnSceneIndex(SceneManager.GetActiveScene().buildIndex);
    }

    public void FadeToLevel (string sceneName)
    {
        levelToLoad = sceneName;
        animator.SetTrigger("FadeOut");
    }
    public void OnFadeComplete()
    {
        
        SceneManager.LoadScene(levelToLoad);
        animator.SetTrigger("FadeIn");

    }
}
