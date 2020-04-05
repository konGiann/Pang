using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerShoot>().ActiveWeapon = PlayerShoot.Weapons.Bullet;

            // play sound

            Destroy(gameObject);
        }
    }
}
