using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorController : MonoBehaviour
{
    public float BulletSpeed;
    public float BulletDamage = 100;
    public GameObject FloatingText;

    // Rigidbody2D rb;
    SoundManager _sound;

    Vector3 expand; // to help up make anchor expand vertically
    bool isFullyExpanded; // check if it reached the end
    bool destroyObject;

    private void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
        _sound = SoundManager.sm;                
    }

    // Start is called before the first frame update
    void Start()
    {
        //rb.velocity = transform.up * BulletSpeed;          
    }

    private void Update()
    {
        if (!isFullyExpanded)
        {
            expand = transform.localScale;
            expand.y += 35f * Time.deltaTime;
            transform.localScale = expand; 
        }

        if (transform.localScale.y >= 20 && !destroyObject)
        {
            isFullyExpanded = true;
            destroyObject = true;
            Destroy(gameObject, 1.5f);
        }      
    }

    private void LateUpdate()
    {
        // destroy bullet even if it is not destroyed on collision and reset combo counter
        if (transform.position.y >= 9)
        {
            // play break combo effect if combo is active
            if (GameManager.gm.comboCounter > 0)
            {
                SoundManager.sm.audioController.PlayOneShot(SoundManager.sm.ComboBroke);
            }

            // reset combo
            GameManager.gm.comboCounter = 0;

            // reset combo UI
            GameManager.gm.UIComboCounter.text = "";

            // flawless victory is impossible now
            GameManager.gm.flawlessVictory = false;

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.tag == "Balloon")
        {
            // play ball split sound
            _sound.audioController.PlayOneShot(_sound.BallExplosion);

            // damage ball
            otherObject.GetComponent<BalloonController>().DamageBall(BulletDamage);

            // add points to player's score
            // apply bonus damage based on the y position of the ball
            otherObject.GetComponent<BalloonController>().ScoreValue += (int)Math.Ceiling(transform.position.y);
            GameManager.gm.AddScore(otherObject.GetComponent<BalloonController>().ScoreValue);

            // pop up points on canvas                        
            var FloatingObj = Instantiate(FloatingText, transform.position, Quaternion.identity);
            FloatingObj.GetComponent<FloatingTextController>().Init(1f, "+" + otherObject.GetComponent<BalloonController>().ScoreValue.ToString());

            GameManager.gm.comboCounter++;
            GameManager.gm.timeHit = Time.time;

            if (GameManager.gm.hitsToWin <= 0)
            {
                GameManager.gm.StartCoroutine(GameManager.gm.LoadNextLevel());
            }

            // destroy bullet
            Destroy(gameObject);
        }
    }
}
