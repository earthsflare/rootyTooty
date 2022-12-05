using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // for switching scenes

public enum LevelIndex
{
    NULL_Value = -1,
    TitleScreen = 0,
    Level1 = 1,
    Level2 = 2,
    Level3 = 3,
    Level4 = 4,
    Level5 = 5,
    Level6 = 6,
    Level7 = 7,
    Level8 = 8,
}

//Controls player level transition, fading animation, and StartUp functions for Persistant scripts
public class levelManager : MonoBehaviour
{
    private static levelManager _levelManager;
    public static levelManager instance { get { return _levelManager; } }

    [Header("Fade Animation Properties")]
    [SerializeField] private float fadeAlphaSpeed;
    private bool doFadeIn = false;

    [Header("Object References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Canvas fadeCanvas;

    #region Level Enum Functions / Properties
    public int TitleLevelIndex { get => (int)LevelIndex.TitleScreen; }
    public bool OnTitleLevel { get => (int)LevelIndex.TitleScreen == SceneManager.GetActiveScene().buildIndex; }
    public bool IsLevelTitle(int levelIndex) { return (int)LevelIndex.TitleScreen == levelIndex; }
    #endregion

    private void Awake()
    {
        #region Setup Singleton
        if (_levelManager == null)
            _levelManager = this;
        else if (_levelManager != this)
            Destroy(this);
        #endregion

        animator.SetFloat("AnimationSpeed", fadeAlphaSpeed);
    }
    private void Start()
    {
        StartUp(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
        SceneManager.activeSceneChanged += StartUp;
    }
    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= StartUp;
    }
    private void StartUp(Scene current, Scene next)
    {
        if (gameManagerScript.instance != null)
        {
            //Initiate gameManagerScript startUp
            gameManagerScript.instance.StartUp(next.buildIndex);

            if (fadeCanvas != null)
            {
                if (OnTitleLevel)
                    fadeCanvas.gameObject.SetActive(false);
                else
                    fadeCanvas.gameObject.SetActive(true);
            }
        }

        //Activate fade In animation
        if (doFadeIn)
            StartCoroutine(FadeInLevel());
        else
        {
            //Find and set player position
            if (Player.instance != null)
            {
                Player.instance.transform.position = gameManagerScript.instance.SpawnPosition;
                Player.ActivatePlayer();
            }
        }  
    }
    public void FadeToLevel(LevelIndex sceneName)
    {
        StartCoroutine(FadeOutLevel(sceneName));
    }
    private IEnumerator FadeOutLevel(LevelIndex sceneName)
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(fadeAlphaSpeed);
        doFadeIn = true;
        LoadLevel((int)sceneName);
    }
    private IEnumerator FadeInLevel()
    {
        //Deactivate player until their spawn location is set
        Player.DeactivatePlayer();
        yield return (!SpawnLocation.IsSearching);

        //Save game after getting Spawn position
        gameManagerScript.instance.SaveGame();

        //Spawn in player
        Player.instance.transform.position = gameManagerScript.instance.SpawnPosition;
        Player.ActivatePlayer();

        //Activate Fade In Animation
        animator.SetTrigger("FadeIn");
        doFadeIn = false;
        yield return new WaitForSeconds(fadeAlphaSpeed);
    }
    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
