using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    // fire starting points for each weapon
    public Transform anchorFirePoint;
    public Transform bulletFirePoint;

    // how fast can the player shoot with each weapon
    const float ANCHOR_FIRE_RATE = 1;
    const float BULLET_FIRE_RATE = .4f;

    // weapon prefabs
    public GameObject anchorPref;
    public GameObject bulletPref;    

    // simple enum to control which is the active weapon
    public enum Weapons
    {
        Anchor,
        Bullet
    }

    public Weapons ActiveWeapon = Weapons.Anchor;

    private bool _allowFire = true;
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && _allowFire)
        {
            switch (ActiveWeapon)
            {
                case Weapons.Anchor:
                    //fireRate = 1;
                    StartCoroutine(ShootAnchor());
                    break;

                case Weapons.Bullet:
                    //fireRate = .4f;
                    StartCoroutine(ShootBullet());
                    break;
                default:
                    break;
            }

        }
    }

    IEnumerator ShootAnchor()
    {
        _allowFire = false;
        Instantiate(anchorPref, anchorFirePoint.position, anchorFirePoint.rotation);
        yield return new WaitForSeconds(ANCHOR_FIRE_RATE);
        _allowFire = true;
    }

    IEnumerator ShootBullet()
    {
        _allowFire = false;
        Instantiate(bulletPref, bulletFirePoint.position, bulletFirePoint.rotation);
        yield return new WaitForSeconds(BULLET_FIRE_RATE);
        _allowFire = true;
    }
}
