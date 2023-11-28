using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Action : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;

    //조준하지 않았을때 플레이어가 봐야할 방향
    Transform LookDir;

    //조준한뒤 플레이어의 애니매이션 부위별 회전
    Transform playerChestTr, playerUpperChestTr;
    Transform HeadTr;

    //공격가능한 곳
    Transform ATKPoint;


    //gunSystem을 가진 오브젝트
    Transform gun;


    //플레이어의 애니매이션의 본을 움직여야 허리가 돌아간다.
    private void Start()
    {
        //활성화된 총기를 알아야함
        //test용으로 첫번째 총기만 
        
        gun = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0)
            .GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetChild(0);
        // cam = transform.GetComponent<Player>().Player_cam;


        anim = transform.GetChild(0).GetComponent<Animator>();
        if (anim)
        {
            playerChestTr = anim.GetBoneTransform(HumanBodyBones.Chest);
            playerUpperChestTr = anim.GetBoneTransform(HumanBodyBones.Spine);
            HeadTr = anim.GetBoneTransform(HumanBodyBones.Head);
        }
        //LookHead = activeGun.GetComponent<GunSystem>().GetAttackpoint();
        LookDir = transform.GetChild(0).GetChild(3);
        ATKPoint = transform.GetChild(1);
    }
    public Transform EnemyIsLookHaedTr()
    {
        return HeadTr;
    }
    public void LateUpdate()
    {
        Operation_boneRitation();
    }

    public void AnimStart()
    {
        Debug.Log(transform.GetChild(0).name);
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Operation_boneRitation()
    {

        //ChestDir를 총이 바라보는 것으로 바꿔준다면 상체는 총이 바라보는 곳을 바라봄
        //ChestDir = gunSystem.GetMuzzleTr().position + gunSystem.GetMuzzleTr().forward * 100f;

        //총을 조준했을때 총이 앞으로 향하도록하기 위한 상체의 회전값
        playerChestTr.Rotate(new Vector3(0, 45, 0));
        playerUpperChestTr.LookAt(ATKPoint);
        //HeadTr.LookAt(ATKPoint);
        //팔이 총을 기준으로 위치가 옮겨지는 Pivot코드 있어야함

    }
    public void MoveAnim(float vertical, float horizon)
    {

        /*if (vertical != 0 || horizon != 0)
        {
            Debug.Log("방향이 입력됨");
        }*/
        switch (vertical)
        {
            case 1:
                anim.SetFloat("B&F", 1);
                break;
            case 0:
                anim.SetFloat("B&F", 0);
                break;
            case -1:
                anim.SetFloat("B&F", -1);
                break;
        }
        switch (horizon)
        {
            case 1:
                anim.SetFloat("Dir", 1);
                break;
            case 0:
                anim.SetFloat("Dir", 0);
                break;
            case -1:
                anim.SetFloat("Dir", -1);
                break;
        }
    }
    public void Crouch()
    {
        switch (anim.GetBool("Crouch"))
        {
            case true:
                anim.SetBool("Crouch", false);
                break;

            case false:
                anim.SetBool("Crouch", true);
                break;
        }
    }
    public void IsJump()
    {
        anim.SetTrigger("Jump");
    }

    public bool Aiming()
    {
        switch (anim.GetBool("Aim"))
        {
            case true:
                anim.SetBool("Aim", false);
                return false;
            case false:
                anim.SetBool("Aim", true);
                return true;
        }
    }
    public void Attacking()
    {
        anim.SetTrigger("Atteck");
    }
    public void Reloading()
    {
        switch (anim.GetBool("Reload"))
        {
            case false:
                anim.SetBool("Reload", true);
                break;
            case true:
                anim.SetBool("Reload", false);
                break;
        }
    }

    public void PickUpMotion()
    {
        switch (anim.GetBool("Pickup"))
        {
            case false:
                anim.SetBool("Pickup", true);
                break;
            case true:
                anim.SetBool("Pickup", false);
                break;
        }
    }
    public void Drink_A_Heal()
    {
        switch (anim.GetBool("Drink"))
        {
            case false:
                anim.SetBool("Drink", true);
                break;
            case true:
                anim.SetBool("Drink", false);
                break;
        }
    }
}
