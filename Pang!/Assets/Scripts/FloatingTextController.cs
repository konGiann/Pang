using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingTextController : MonoBehaviour
{
    public float DestroyAfterSeconds = 1f;        
    public TextMeshProUGUI TextToShow;
    [Range(0f, 10f)]
    public float MovementSpeed = 6;

    Vector2 movement;

    public void Init(float time, string text)
    {
        DestroyAfterSeconds = time;
        TextToShow.text = text;
    }

    private void Awake()
    {
        movement = new Vector2(transform.position.x, transform.position.y);
    }

    private void Update()
    {         
        movement.y += 0.1f * MovementSpeed * Time.smoothDeltaTime;
        transform.position = movement;
    }

    private void Start()
    {
        StartCoroutine(DestroyObjectAfterSeconds(DestroyAfterSeconds));
    }

    private IEnumerator DestroyObjectAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
