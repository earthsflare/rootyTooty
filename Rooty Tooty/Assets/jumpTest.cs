using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpTest : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpHeight = 20f;
    float jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
