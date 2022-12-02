using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that activates when a CMBoundingShape is looking for the player
public class CMPlayerFinder : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private CMBoundingShape cameraBounds = null;
    [SerializeField] private BoxCollider2D boxCollider = null;
    [SerializeField] private SpriteRenderer visualAid = null;
    private void Awake()
    {
        if(visualAid == null)
            visualAid = GetComponent<SpriteRenderer>();
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
            if (boxCollider == null)
            {
                Debug.Log(gameObject.name + " Does not have a collisionBounds set");
                gameObject.SetActive(false);
                return;
            }
        }
        if (cameraBounds == null)
        {
            cameraBounds = GetComponentInParent<CMBoundingShape>();
            if (cameraBounds == null)
            {
                Debug.Log(gameObject.name + " Does not have a collisionBounds set");
                gameObject.SetActive(false);
                return;
            }
        }

        if (visualAid != null)
            visualAid.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");

        if (collision.gameObject != Player.instance.gameObject)
            return;

        cameraBounds.FoundPlayer();
    }
}
