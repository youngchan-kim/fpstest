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
        Left_start = new Vector3(Left.localPosition.x, Left.localPosition.y, Left.localPosition.z);
        Left_end = new Vector3(Left.localPosition.x, Left.localPosition.y, Left.localPosition.z - 3f);

        Right_start = new Vector3(Right.localPosition.x, Right.localPosition.y, Right.localPosition.z);
        Right_end = new Vector3(Right.localPosition.x, Right.localPosition.y, Right.localPosition.z + 3f);

    }

    public void OnTriggerEnter(Collider other)
    {
        Left.localPosition = Left_end;
        Right.localPosition = Right_end;
    }

    private void OnTriggerExit(Collider other)
    {
        Left.localPosition = Left_start;
        Right.localPosition = Right_start;
    }
}
