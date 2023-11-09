using Cinemachine;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Action : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;

    //조준하지 않았을때 플레이어가 봐야할 방향
    public Transform orientation;

    //조준한뒤 플레이어의 애니매이션 부위별 회전
    Transform playerChestTr, playerHeadTr, playerHipsTr;

    Vector3 ChestOffset = new Vector3(0, -40, 0);
    Vector3 ChestDir = new Vector3();
    Vector3 HipsDir = new Vector3(0, -40, 0);
    //gunSystem을 가진 오브젝트
    Transform gun;
    //해당 오브젝트의 총구
    GunSystem gunSystem;

    CinemachineVirtualCamera cam;
    Vector3 LookHead;
    //플레이어의 애니매이션의 본을 움직여야 허리가 돌아간다.
    private void Start()
    {
        //활성화된 총기를 알아야함
        //test용으로 첫번째 총기만 
        // Debug.Log(transform.parent.GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetChild(0).name);
        gun = transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetChild(0);
        gunSystem = gun.GetComponent<GunSystem>();
        cam = transform.GetComponent<Player>().Player_cam;


        anim = transform.GetChild(0).GetComponent<Animator>();
        if (anim)
        {
            playerChestTr = anim.GetBoneTransform(HumanBodyBones.Chest);
            playerHeadTr = anim.GetBoneTransform(HumanBodyBones.Head);
            playerHipsTr = anim.GetBoneTransform(HumanBodyBones.Hips);

        }
        //LookHead = activeGun.GetComponent<GunSystem>().GetAttackpoint();
    }

    public void LateUpdate()
    {
        Operation_boneRitation();
    }


    private void Operation_boneRitation()
    {

        //ChestDir를 총이 바라보는 것으로 바꿔준다면 상체는 총이 바라보는 곳을 바라봄
        ChestDir = gunSystem.GetMuzzleTr().position + gunSystem.GetMuzzleTr().forward * 100f;

        // 머리를 총구가 바라보는 방향 보기
        playerHeadTr.LookAt(gunSystem.GetMuzzleTr().position); 
        
        //총을 조준했을때 총이 앞으로 향하도록하기 위한 상체의 회전값
        playerChestTr.Rotate(new Vector3(0, 45, 0));

        
    }

    public void MoveAnim(float vertical, float horizon)
    {
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
    public void IsJump(bool  isjump)
    {               
        anim.SetBool("Jump", isjump);
    }

    public bool Aiming()
    {
        switch (anim.GetBool("Aim"))
        {
            case true:
                //Debug.Log(transform.position + transform.forward * 10f);
                cam.LookAt = orientation;
                orientation.position = playerHeadTr.position + playerHeadTr.forward * 5f;
                anim.SetBool("Aim", false);
                return false;
            case false:
                //Debug.Log(muzzlepoint.position + muzzlepoint.forward * 10f);
                cam.LookAt = gunSystem.GetAttackPointtr();
                anim.SetBool("Aim", true);
                return true;
        }
    }
    public void Attacking()
    {
        anim.SetTrigger("Atteck");
    }
    public void Reloading(bool isReload)
    {
        anim.SetBool("Reload", isReload);
    }

    public void PickUpMotion(bool ispickup)
    {
        anim.SetBool("Pickup", ispickup);
    }
    public void Drink_A_Heal(bool isdrink)
    {
        anim.SetBool("Drink", isdrink);
    }
}
