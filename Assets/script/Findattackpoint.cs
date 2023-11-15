using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Findattackpoint : MonoBehaviour
{
    Transform BaseObject;
    Enemy1 enemy1;
    float len;
    Transform attackPoint;
    float notGround;
    void Start()
    {

        BaseObject = transform.parent.parent.parent.parent.parent.parent.parent.parent;
        enemy1 = BaseObject.GetComponent<Enemy1>();
        
        attackPoint = BaseObject.GetChild(1);
        Debug.Log(attackPoint.name);
        notGround = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        len = enemy1.maxlen();
        LooktheLine();
        transform.LookAt(attackPoint);
    }
    //머리가 바라보는 곳이 땅인지 체크할 것
    public void LookTargetCheck(Transform target)
    {
        if (target != null)
            attackPoint = target.GetComponent<Player_Action>().EnemyIsLookHaedTr();
        else
            attackPoint.position = transform.position + transform.forward * len;
    }

void LooktheLine()
    {
        Debug.DrawLine(transform.position, transform.position+transform.forward*len, Color.blue);
    }
}
