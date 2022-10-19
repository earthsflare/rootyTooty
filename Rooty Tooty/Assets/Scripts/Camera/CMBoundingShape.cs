using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMBoundingShape : MonoBehaviour
{
    [SerializeField] private static CMBoundingShape instance = null;
    public static CMBoundingShape I { get => instance; }

    [SerializeField] private PolygonCollider2D boundingCollider = null;
    public static PolygonCollider2D BoundingCollider { 
        get
        {
            if (instance == null)
                return null;
            return instance.boundingCollider;
        } 
    }

    private void OnEnable()
    {
        if (boundingCollider == null)
            boundingCollider = GetComponent<PolygonCollider2D>();

        if (instance == null && boundingCollider != null)
            instance = this;
        else if (instance != this)
            gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        if (instance == this)
            instance = null;
    }
}
