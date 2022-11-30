using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class gameManagerScript : MonoBehaviour
{
    //public Text pointsText;
    public static gameManagerScript instance;

    [SerializeField] private GameObject prefab;
    [Header("Read Only")]
    [SerializeField] private Vector2 spawnPosition;
    //[SerializeField] private bool random;
    [SerializeField] private float freezeGameTime = 1f;

    public static bool GameIsFrozen = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        if (Player.instance == null)
        {
            OnSpawnPlayerPrefab();
        }
    }

    

    public void OnSpawnPlayerPrefab()
    {
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }

    public static void UndoDontDestroyOnLoad(GameObject g)
    {
        if (g == null)
            return;
        SceneManager.MoveGameObjectToScene(g, SceneManager.GetActiveScene());
    }

    public void freezeGame()
    {
        Time.timeScale = 0f;
        GameIsFrozen = true;
    }

    public void unfreezeGame()
    {
        Time.timeScale = 1f;
        GameIsFrozen = false;
    }
    void Update()
    {
        if (GameIsFrozen == true)
        {
            freezeGameTime -= Time.deltaTime;
            if (freezeGameTime <= 0)
            {
                unfreezeGame();
            }
        }
    }
}
