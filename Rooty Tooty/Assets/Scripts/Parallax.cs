using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private float startpos;
    [SerializeField] private float parallaxEffect;


    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        length = texture.width / sprite.pixelsPerUnit;
    }

    void FixedUpdate()
    {
        float temp = (1 - parallaxEffect);
        float dist = parallaxEffect;

        if(Camera.main != null)
        {
            temp = temp * Camera.main.transform.position.x;
            dist = dist * Camera.main.transform.position.x;
        }

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if(temp > startpos + length)
        {
            startpos += length;
        }
        else if(temp < startpos - length)
        {
            startpos -= length;
        }
    
    }
}
