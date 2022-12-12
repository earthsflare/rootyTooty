using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    private static LevelIndex searchingLevelIndex = LevelIndex.TitleScreen;
    private static int searchingID = 0;
    private static bool isSearching = false;

    public static bool IsSearching { get => isSearching; }
    public static void StartSearching() { isSearching = true; }
    public static void StartSearching(LevelIndex levelIndex, int i) 
    {
        searchingLevelIndex = levelIndex;
        searchingID = i;
        isSearching = true; 
    }
    public static void UpdateSearch(LevelIndex levelIndex, int i)
    {
        searchingLevelIndex = levelIndex;
        searchingID = i;
    }

    [SerializeField] private LevelIndex lastLevel = LevelIndex.TitleScreen;
    [SerializeField] private int id = 0;

    private void Awake()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if(sprite != null)
            sprite.enabled = false;
    }
    private void Start()
    {
        if (lastLevel == LevelIndex.TitleScreen)
        {
            Debug.Log(gameObject.name + " Level Index not set");
            Destroy(gameObject);
        }
        else if (!isSearching)
            Destroy(gameObject);
        else if (searchingLevelIndex == lastLevel && searchingID == id)
        {
            gameManagerScript.instance.SetSpawnPosition(transform.position);
            Destroy(gameObject);
        }
        else if (searchingLevelIndex != lastLevel || searchingID != id)
            Destroy(gameObject);
    }
    private void Update()
    {
        if (searchingLevelIndex == LevelIndex.TitleScreen)
            return;

        else if(isSearching && searchingLevelIndex == lastLevel && searchingID == id)
        {
            gameManagerScript.instance.SetSpawnPosition(transform.position);
            ResetSearchParam();
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static void ResetSearchParam()
    {
        searchingLevelIndex = LevelIndex.TitleScreen;
        searchingID = 0;
    }
}
