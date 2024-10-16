using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour //a
{
    //== Bezier curves===================================================================================\\

    /// <summary>
    /// While most Bezier curves are 3 or 4 points, it is possible to have any number of points using this recursive function.
    /// </summary>
    /// <param name="u"> The amount of interopolation [0..1]</param>
    /// <param name="points">An array of Vector3s to interpolate</param>

    static public Vector3 Bezier(float u, params Vector3[] points)  //b
    {
        //Set up the array
        Vector3[,] vArr = new Vector3[points.Length, points.Length];
        //Fill the last row of vArr with the elements of vList
        int r = points.Length - 1;
        for ( int c = 0; c < points.Length; c++)
        {
            vArr[r, c] = points[c];
        }

        //iterate over all remaining rows and interpolate points at each one
        for (r--; r >=0; r--)
        {
            for (int c = 0; c <= r; c++)
            {
                vArr[r, c] = Vector3.LerpUnclamped(vArr[r + 1, c], vArr[r + 1, c + 1], u);
            }
        }
        //When complete, vArr[0,0] holds the final interpolated value
        return vArr[0, 0];
    }

    
}
