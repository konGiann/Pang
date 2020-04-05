using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    public int BounceForce = 10000;
    public int BalloonDirection = 1;
    public float ConstantSpeed = 0.1f;
    public float SmoothingFactor = 0.1f;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(2, 0, 0);
        Debug.Log("Starting velocity: " + rb.velocity);
    }

    private void Start()
    {
        
        //rb.velocity = new Vector3(2, 0, 0);
        
    }

    private void FixedUpdate()
    {
        if(rb.velocity.x < 2 && rb.velocity.x >= 0)
        {
            rb.velocity = new Vector3(2, 0, 0);
        }
        else if(rb.velocity.x < 0 && rb.velocity.x > -2)
        {
            rb.velocity = new Vector3(-2, 0, 0);
        }
        Debug.Log(rb.velocity);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Floor")
        {
            //rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * BounceForce);            
        }
    }
}
