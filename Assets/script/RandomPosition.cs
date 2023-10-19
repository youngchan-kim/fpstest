using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class RandomPosition : TargetCheck
{
    Vector3 randomPoint;
    public Vector3 RandomPoint(float len)
    {
        randomPoint = transform.position + Random.insideUnitSphere * len;

        NavMeshHit hit;
        while(NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)==false)
        {
            randomPoint = transform.position + Random.insideUnitSphere * len;
        }
        
        return hit.position;
    }


        
}
