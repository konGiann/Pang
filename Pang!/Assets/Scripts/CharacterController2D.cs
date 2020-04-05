using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
    // public editor properties
    public float moveSpeed = 3f;
    public int playerHealth = 1;    
    public bool playerCanMove = true;

    // player tracking
    bool _facingRight = true;
    bool _isRunning = false;

    // player motion
    float horizontalMovement;   

    // components references
    Transform _transform;
    Rigidbody2D _rb;
    Animator _anim;
    SoundManager _sound;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        _sound = SoundManager.sm;

        _anim = GetComponent<Animator>();
        if (_anim == null)
            Debug.LogWarning("There is no Animator attached to player object");
    }

    // Update is called once per frame
    void Update()
    {
        // get player input on the X axis
        horizontalMovement = Input.GetAxisRaw("Horizontal");

        if(horizontalMovement != 0)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
        _anim.SetBool("Running", _isRunning);

        
    }

    private void LateUpdate()
    {
        Vector3 localScale = _transform.localScale;

        // determine which direction the player is facing and flip sprite accordingly
        if (horizontalMovement > 0 && !_facingRight)
        {
            FlipPlayer();
        }
            
        else if (horizontalMovement < 0 && _facingRight)
        {
            FlipPlayer();
        }

    }

    private void FixedUpdate()
    {
        // move player
       _rb.velocity = new Vector2((horizontalMovement * moveSpeed) * Time.fixedDeltaTime, 0);
    }

    // respawn player
    public void RespawnPlayer()
    {        
        _anim.SetTrigger("Respawn");        

        // spawn player at starting position
        transform.position = new Vector3(0f, 0, 0f);                        
    }

    IEnumerator KillPlayer()
    {
        _sound.audioController.PlayOneShot(_sound.LostLife);

        // pause game play death animation and then resume
        GameManager.gm.PauseGame();
        _anim.SetTrigger("Death");
        yield return new WaitForSecondsRealtime(2f);
        GameManager.gm.ResumeGame();        

        // reset game stats
        GameManager.gm.ResetLevel();
    }

    public void ApplyDamage(int damage)
    {
        playerHealth -= damage;

        if(playerHealth <= 0)
        {
            StartCoroutine(KillPlayer());
        }
    }

    // flip sprite 
    private void FlipPlayer()
    {
        _facingRight = !_facingRight;

        transform.Rotate(0f, 180f, 0f);
    }
}
