using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class PowerUp : MonoBehaviour
{
    [Header("Inscribed")]
    //This is an unusual but hand use of Vector2's
    [Tooltip("x holds a min value and y a max value for a Random.Range() call.")]
    public Vector2 rotMinMax = new Vector2(15, 90);
    [Tooltip("x holds a min value and y a max value for a Random.Range() call.")]
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 10; //Powerup exists for # seconds
    public float fadeTime = 4; //Then it fades over # seconds

    [Header("Dynamic")]
    public eWeaponType _type; //Type of powerup
    public GameObject cube; //reference to the powercube child
    public TextMesh letter; //Reference to the text mesh
    public Vector3 rotPerSecond; //euler rotation speed for powercube
    public float birthTime; //The time it was instantiated
    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Material cubeMat;

    void Awake()
    {
        //Find the Cube reference (there's only a single child)
        cube = transform.GetChild(0).gameObject;
        //Find the TextMesh and other components
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeMat = GetComponent<Renderer>().material;

        //set a random velocity
        Vector3 vel = Random.onUnitSphere; // Get Random XYZ velocity
        vel.z = 0; //Flatten the vel to the XY plane
        vel.Normalize(); //Normalizing a vector3 sets its length to 1m

        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        //set the rotation of this powerup gameobject to R:[0,0,0]
        transform.rotation = Quaternion.identity;

        //Randomize rotpersecond for powercube using rotminmax x & y
        rotPerSecond = new Vector3(Random.Range(rotMinMax[0], rotMinMax[1]),
                                   Random.Range(rotMinMax[0], rotMinMax[1]),
                                   Random.Range(rotMinMax[0], rotMinMax[1]));

        birthTime = Time.time;
    }
   

    // Update is called once per frame
    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        //Fade out the powerup over time
        //Given the default values, a pwerup will exist for 10 seconds
        //and then fade out over 4 seconds
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }
        if(u > 0)
        {
            Color c = cubeMat.color;
            c.a = 1f - u;
            cubeMat.color = c;
            //Fade the letter too, just not as much
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if (!bndCheck.isOnScreen)
        {
            Destroy(gameObject);
        }
    }

    public eWeaponType type { get { return _type; } set { SetType(value); } }

    public void SetType(eWeaponType wt)
    {
        //Grab the weapondefinition from Main
        WeaponDefinition def = Main.GET_WEAPON_DEFINITION(wt);
        cubeMat.color = def.powerUpColor;
        //letter.color = def.color
        letter.text = def.letter;
        _type = wt;
    }

    ///<summary>
    ///This function is called by the hero class when a powerup is collected.
    ///</summary>
    ///<param name="target">The gameobject absorbing this powerup</param>
    public void AbosrbedBy(GameObject target)
    {
        Destroy(this.gameObject);
    }
}
