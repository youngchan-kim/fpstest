using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Action : MonoBehaviour
{
    Animator anim;
    //아바타가 보는 방향
    public Transform attackPoint;

    //케릭터의 애니매이션 부위별 회전
    Transform ChestTr, UpperChestTr, HeadTr;


    //플레이어의 애니매이션의 본을 움직여야 허리가 돌아간다.
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        if (anim)
        {
            ChestTr = anim.GetBoneTransform(HumanBodyBones.Chest);
            UpperChestTr = anim.GetBoneTransform(HumanBodyBones.Spine);
            HeadTr = anim.GetBoneTransform(HumanBodyBones.Head);
        }
        attackPoint = transform.GetChild(1);
    }

    public void LateUpdate()
    {
        Operation_boneRitation();
    }


    private void Operation_boneRitation()
    {
/*        //총을 조준했을때 총이 앞으로 향하도록하기 위한 상체의 회전값
        ChestTr.Rotate(new Vector3(0, 45, 0));*/
        //상체는 공격할 곳을 바라봄
        UpperChestTr.LookAt(attackPoint);
        HeadTr.LookAt(attackPoint);
    }

    public void Initalize()
    {
        if (anim)
        {
            anim.SetFloat("Move", 0.0f);
            anim.Play("Movement");
        }
    }
    public void Move(float value)
    {
        if (anim) anim.SetFloat("Move", value);
    }

    public void Shoot(bool shooting)
    {
        anim.SetBool("IsShoot", shooting);
    }
    public void Die()
    {
        if (anim) anim.SetTrigger("IsDie");
    }
}