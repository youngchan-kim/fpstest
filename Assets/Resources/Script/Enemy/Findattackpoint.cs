using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Findattackpoint : MonoBehaviour
{
    Transform EyeObject;
    Enemy enemy;
    float len;
    Transform attackPoint;
    void Start()
    {

        EyeObject = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0);
        //Debug.Log(transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).name);
        enemy = GetComponent<Enemy>();
        
        attackPoint = transform.GetChild(1);
        //Debug.Log(attackPoint.name);
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
            attackPoint = target.GetComponent<Player_Action>().EnemyIsLookHaedTr();
        else
            attackPoint.position = EyeObject.position + EyeObject.forward * len;
    }

void LooktheLine()
    {
        Debug.DrawLine(EyeObject.position, EyeObject.position+ EyeObject.forward*len, Color.blue);
    }
}
