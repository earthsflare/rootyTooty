using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gotHurtUI : MonoBehaviour
{
    public static gotHurtUI instance;

    [SerializeField] private float decreaseAlpha = 1f;
    [SerializeField] private float initialAlpha = 0.8f;

    [Header("References")]
    [SerializeField] private Image gotHurtScreen;

    private void Awake()
    {
        instance = this;
        gotHurtScreen = GetComponent<Image>();
    }

    public void gotHurt()
    {
        if(gotHurtScreen == null)
        {
            //Debug.Log("Hurt Screen Doesn't Exist");
            return;
        }

        var color = gotHurtScreen.color; // set variable color to color of image   
        color.a = initialAlpha; // change that color to show

        gotHurtScreen.color = color; //assign it back to the image

    }

    public void Update()
    {
        /*
        if (dead == true)
        {
            StartCoroutine(deathAnim());
        }
        */
        if (gotHurtScreen != null)
        {
            if (gotHurtScreen.GetComponent<Image>().color.a > 0)
            {
                var color = gotHurtScreen.GetComponent<Image>().color; // set variable color to color of image
                color.a -= decreaseAlpha * Time.deltaTime; // reduce alpha by 0.01 until it reaches 0 

                gotHurtScreen.GetComponent<Image>().color = color;
            }
        }
    }
}
