using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 10f; //The movement speed is 10m/s
    public float fireRate = 0.3f; // Second/shot (Unused)
    public float health = 10; // Damage needed to destroy this enemy
    public int score = 100; // Points earned for destroying this

    private BoundsCheck bndCheck;

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();    
    }

    //This is a Property: A method that acts like a field
    public Vector3 pos
    {
        get
        {
            return this.transform.position;
        }
        set
        {
            this.transform.position = value;
        }
    }
  

    // Update is called once per frame
    void Update()
    {
        Move();

        //Check whether this Enemy has gone off the bottom of the screen
        if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown))
        {
            Destroy(gameObject);
        }


        //if (!bndCheck.isOnScreen)
        //{
        //    if ( pos.y < bndCheck.camHeight - bndCheck.radius)
        //    {
        //        // We're off the bottom, so destroy this GameObject
        //        Destroy(gameObject);
        //    }
        //}
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;
        if (otherGO.GetComponent<ProjectileHero>() != null)
        {
            Destroy(otherGO);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Enemy hit by non-ProjectileHero: " + otherGO.name);
        }
    }
}
