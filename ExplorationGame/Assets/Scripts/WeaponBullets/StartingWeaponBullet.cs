using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingWeaponBullet : MonoBehaviour
{
    //Public Variables
    [Header("Weapon Scriptable Object Reference")]
    public WeaponsSO startingWepSO;

    [Header("Bullet Variables")]
    public float bulletSpeed;
    public float bulletLifeTime;
    public GameObject explosionEffect;

    //Private Variables
    private bool hasExploded = false;

    void Start()
    {
        //Setting the values of the starting weapon equal to the scriptable object;
        bulletSpeed = startingWepSO.wepBulletSpeed;
        bulletLifeTime = startingWepSO.wepRange;
    }

    void Update()
    {
        //Moves the bullet forward
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);

        //Gives the bullet a life time and counts down
        bulletLifeTime -= Time.deltaTime;
        if (bulletLifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision theCol)
    {
        //Check if the bullet collides with an object, to destroy itself
        if (theCol.gameObject.CompareTag("Wall") && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        //Explosion effect
        Instantiate(explosionEffect, transform.position, transform.rotation);

        //Get nearby Objects
        //Add force and damage

        //Destroy gameobject
        Destroy(gameObject);
    }
}
