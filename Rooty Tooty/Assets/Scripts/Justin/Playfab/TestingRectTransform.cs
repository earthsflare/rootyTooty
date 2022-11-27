using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingRectTransform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        Debug.Log("Rect Pos: " + rectTransform.anchoredPosition);
        Debug.Log("Width and Height: " + rectTransform.sizeDelta);

    }

}
