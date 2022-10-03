using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadlevel : MonoBehaviour
{
    public int iLevelToLoad; //number of level to load
    public string sLevelToLoad; //string name of level to load

    public Vector3 changeLevelPosition;
    public bool useIntegerToLoadLevel = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;

        if(collisionGameObject.name == "Player")
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        if(useIntegerToLoadLevel)
        {
            //Player.transform.position = changeLevelPosition;
            GameManager.instance.NextlevelPos = changeLevelPosition;
            SceneManager.LoadScene(iLevelToLoad);
        }
        else
        {
            //Player.transform.position = changeLevelPosition;
            GameManager.instance.NextlevelPos = changeLevelPosition;
            SceneManager.LoadScene(sLevelToLoad);
        }
        //GameManager.instance.nextLevelPosition = changeLevelPosition;
    }
    
}
