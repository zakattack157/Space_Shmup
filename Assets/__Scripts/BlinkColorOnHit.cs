using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BlinkColorOnHit : MonoBehaviour
{
    private static float blinkDuration = 0.1f;
    private static Color blinkColor = Color.red;

    [Header("Dynamic")]
    public bool showingColor = false;
    public float blinkCompleteTime;
    public bool ignoreOnCollisionEnter = false;

    private Material[] materials;
    private Color[] originalColors;
    private BoundsCheck bndCheck;


    void Awake()
    {
        bndCheck = GetComponentInParent<BoundsCheck>();
        //Get materials and colors for this GameObject and its children
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for(int i = 0; i<materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if (showingColor && Time.time > blinkCompleteTime) RevertColors();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (ignoreOnCollisionEnter) return;
        //Check for collisions with ProjectileHero
        ProjectileHero p = collision.gameObject.GetComponent<ProjectileHero>();
        if (p != null)
        {
            if (bndCheck != null && !bndCheck.isOnScreen)
            {
                return;
            }
            SetColors();
        }
    }

    ///<summary>
    ///Sets the Albedo color (i.e., the main color) of all materials in the
    ///materials array to blinkcolor, sets showingcolor to true, and sets the
    ///time that the colors should be reverted
    ///</summary>
    
    public void SetColors()
    {
        foreach (Material m in materials)
        {
            m.color = blinkColor;
        }
        showingColor = true;
        blinkCompleteTime = Time.time + blinkDuration;
    }

    ///<summary>
    ///Reverts all materials in the materials array back to their original color
    ///and sets showingColor to false.
    ///</summary>
    void RevertColors()
    {
        for (int i = 0; i<materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingColor = false;
    }
}
