using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManagerScript : MonoBehaviour
{
    //public Text pointsText;
    public static gameManagerScript instance;

    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector2 spawnPosition;
    //[SerializeField] private bool random;

    //gameoverscreen 
    //public static bool GameIsOver = false;
    public GameObject goMenuUI;

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

    public void Setup(int score)
    {
        goMenuUI.SetActive(true);
      //  pointsText.text = score.ToString() + " POINTS";
    }

    //public void RestartButton()
    //{
        //SceneManager.LoadScene("SAMPLE_1");
        // get rid of restart
    //}

    public void ExitButton()
    {
        //before or after
        goMenuUI.SetActive(false);
        SceneManager.LoadScene("Menu");

    }

    public void OnSpawnPlayerPrefab()
    {
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}
