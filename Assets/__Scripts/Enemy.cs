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
    public float powerUpDropChance = 1f;

    protected bool calledShipDestroyed = false;
    protected BoundsCheck bndCheck;

    public void Awake()
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
    public void Update()
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

    void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;
        //Check for collision with ProjectileHero
        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if (p != null)
        {
            //Only damage this enemy if its on screen
            if (bndCheck.isOnScreen)
            {
                //Get damage amount from the Main WEAP_DICT.
                health -= Main.GET_WEAPON_DEFINITION(p.type).damageOnHit;
                if(health <= 0)
                {
                    if (!calledShipDestroyed)
                    {
                        calledShipDestroyed = true;
                        Main.SHIP_DESTROYED(this);
                    }
                    Destroy(this.gameObject);
                }
            }
            Destroy(otherGO);
        }
        else
        {
            print("Enemy hit by non-ProjectileHero: " + otherGO.name);
        }
    }
}
