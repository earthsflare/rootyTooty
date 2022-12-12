using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that activates when a CMBoundingShape is looking for the player
public class CMPlayerFinder : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private CMBoundingShape cmBoundingShapeScript = null;
    //Collider for OnTriggerEnter
    [SerializeField] private BoxCollider2D boxCollider = null;
    //Used for finding size of BoxCollider
    [SerializeField] private SpriteRenderer visualAid = null;
    private void Awake()
    {
        if(visualAid == null)
            visualAid = GetComponent<SpriteRenderer>();
        if(cmBoundingShapeScript == null)
            cmBoundingShapeScript = GetComponentInParent<CMBoundingShape>();
        if(boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        if (visualAid != null)
            visualAid.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != Player.instance.gameObject)
            return;
        Debug.Log("Collision with Camera");
        cmBoundingShapeScript.FoundPlayer();
    }
}
