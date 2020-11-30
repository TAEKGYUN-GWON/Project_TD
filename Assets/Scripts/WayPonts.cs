using System.Collections.Generic;
using UnityEngine;

public class WayPonts : MonoBehaviour
{
    
    public static List<Transform> points = new List<Transform>();

    private void Awake()
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            points.Add(transform.GetChild(i));
        }
    }
}
