using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S { get; private set; }

    [Header("Inscribed")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;

    [Header("Dynamic")]
    [Range(0, 4)]
    public float shieldLevel = 1;

    private void Awake()
    {
        if (S == null)
            S = this;
        else
            Debug.LogError("Hero.Awake() - Attampted to assign second Hero.S!");
    }
   

    // Update is called once per frame
    void Update()
    {
        //pull in information from the Input class
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        //Change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += hAxis * speed * Time.deltaTime;
        pos.y += vAxis * speed * Time.deltaTime;
        transform.position = pos;

        //Rotate the ship to make it feel more dynamic
        transform.rotation = Quaternion.Euler(vAxis * pitchMult, hAxis * rollMult, 0);

    }
}
