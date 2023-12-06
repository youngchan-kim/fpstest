using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Findattackpoint : MonoBehaviour
{
    [SerializeField]
    public Transform EyeObject, DirObject;
    Enemy enemy;
    float len;
    public Transform attackPoint;
    void Start()
    {

        EyeObject = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).Find("EyePos");
        //Debug.Log(transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).name);
        enemy = GetComponent<Enemy>();
        DirObject = transform.GetChild(0).GetChild(1).Find("Dir");
        attackPoint = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        len = enemy.maxlen();
        LooktheLine();
        //attackPoint.position = EyeObject.position + EyeObject.forward * len;
    }

    //머리가 바라보는 곳이 타겟이면 머리를 아니면 정면을
    public void LookTargetCheck(Transform target)
    {
        if (target != null)
        {
            attackPoint.position = target.GetComponent<Player_Action>().EnemyIsLookHaedTr().position;
        }
        else
        {
            attackPoint.position = DirObject.position;
        }

    }

    void LooktheLine()
    {
        Debug.DrawLine(EyeObject.position, EyeObject.position + EyeObject.forward * len, Color.blue);
    }
}
