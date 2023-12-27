using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Door_move : MonoBehaviour
{
    [SerializeField]
    public Transform Left, Right;
    Vector3 Left_start, Right_start;
    Vector3 Left_end, Right_end;
    public void Start()
    {
        Left_start = new Vector3(Left.position.x, Left.position.y, Left.position.z);
        Left_end = new Vector3(Left.position.x, Left.position.y, Left.position.z - 3f);

        Right_start = new Vector3(Right.position.x, Right.position.y, Right.position.z);
        Right_end = new Vector3(Right.position.x, Right.position.y, Right.position.z + 3f);

    }

    public void OnTriggerEnter(Collider other)
    {
        Left.position = Left_end;
        Right.position = Right_end;
    }

    private void OnTriggerExit(Collider other)
    {
        Left.position = Left_start;
        Right.position = Right_start;
    }
}
