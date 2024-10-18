using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(EnemyShield))]
public class Enemy_4 : Enemy
{
    [Header("Enemy_4 Inscribed Fields")]
    public float duration = 4;
    private EnemyShield[] allShields;
    private EnemyShield thisShield;
    private Vector3 p0, p1;
    private float timeStart;
    // Start is called before the first frame update
    void Start()
    {
        allShields = GetComponentsInChildren<EnemyShield>();
        thisShield = GetComponent<EnemyShield>();

        //Initially set p0 & p1 to the current position (from Main.SPawnEnemy())
        p0 = p1 = pos;
        InitMovement();
    }

    void InitMovement()
    {
        p0 = p1;
        //Assign a new on-screen location to p1
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);

        //Make sure that it moves to a different quadrant of the screen
        if (p0.x * p1.x > 0 && p0.y * p1.y > 0)
        {
            if (Mathf.Abs(p0.x) > Mathf.Abs(p0.y))
            {
                p1.x *= -1;
            }
            else
            {
                p1.y *= -1;
            }
        }

        //Reset the time
        timeStart = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - timeStart) / duration;

        if(u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = u - 0.15f * Mathf.Sin(u * 2 * Mathf.PI);
        pos = (1 - u) * p0 + u * p1;
       
    }

    /// <summary>
    /// Enemy4 collisions are handled differently from other enemy subclasses 
    /// to enable protection by enemyshields
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;

        //Make sure this was hit by a projectilehero
        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if(p != null)
        {
            Destroy(otherGO);

            if (bndCheck.isOnScreen)
            {
                //Find gameobject of this enemy4 that was actually hit
                GameObject hitGO = collision.contacts[0].thisCollider.gameObject;
                if(hitGO == otherGO)
                {
                    hitGO = collision.contacts[0].otherCollider.gameObject;
                }

                //Get the damage amount from the Main WEAP_DICT
                float dmg = Main.GET_WEAPON_DEFINITION(p.type).damageOnHit;

                //Find the enemyshield that was hit (if there was one)
                bool shieldFound = false;
                foreach(EnemyShield es in allShields)
                {
                    if(es.gameObject == hitGO)
                    {
                        es.TakeDamage(dmg);
                        shieldFound = true;
                    }
                }
                if (!shieldFound) thisShield.TakeDamage(dmg);

                //If thisShield is still active, then it has not been destroyed
                if (thisShield.isActive) return;

                //This ship was destroyed so tell Main about it
                if (!calledShipDestroyed)
                {
                    Main.SHIP_DESTROYED(this);
                    calledShipDestroyed = true;
                }

                //Destroy Enemy4
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("Enemy_4 hit by non-ProjectileHero: " + otherGO.name);
        }
    }

   
}
