using UnityEngine;

public class BalloonController : MonoBehaviour
{    
    [Range(-1f, 1f)]
    [Tooltip("Direction of the balloon on the x axis (left or right)")]
    public int BalloonDirection;
    public float Health = 100;
    public int ScoreValue = 10;
    public string BalloonType;
    public int DamageToPlayer = 100;
    public GameObject BallToSpawnOne;
    public GameObject BallToSpawnTwo;
    public Transform BallOnePosition;
    public Transform BallTwoPosition;

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        // get component references
        _rigidbody2D = GetComponent<Rigidbody2D>();           
    }
    
    void Start()
    {
        _rigidbody2D.AddForce(new Vector2((float)BalloonDirection * 2000, 0));        
    }

    public void DamageBall(float damage)
    {
        Health -= damage;

        if(Health <= 0)
        {            
            if (BalloonType != "Tiny")
            {
                SpawnBalls();
            }
            Destroy(gameObject);

            // reduce hits needed to win the level
            GameManager.gm.hitsToWin--;

            // check if it was the last balloon to win the level
            // we check this here to avoid using it in the update
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CharacterController2D>().ApplyDamage(DamageToPlayer);            
        }            
    }

    public void SpawnBalls()
    {        
        Instantiate(BallToSpawnOne, BallOnePosition.position, BallOnePosition.rotation);
        BallToSpawnOne.GetComponent<BalloonController>().BalloonDirection = 1;

        Instantiate(BallToSpawnTwo, BallTwoPosition.position, BallTwoPosition.rotation);
        BallToSpawnTwo.GetComponent<BalloonController>().BalloonDirection = -1;
    }
}
