using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    public void DrawLine(Vector3 pos, Vector3 dir_normalized, float length, Color color)
    {
        Debug.DrawLine(pos, pos + dir_normalized * length, color);
    }
}
