using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public static class StaticRandomPosition
{
    public static Vector3 RandomPoint(Vector3 pos, float len)
    {
        Vector3 randomPoint = pos + Random.insideUnitSphere * len;

        NavMeshHit hit;
        while(NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)==false)
        {
            randomPoint = pos + Random.insideUnitSphere * len;
        }
        
        return hit.position;
    }
}
