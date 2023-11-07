using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCam : MonoBehaviour
{
    //public AimShaker aim;
    private float sensX = 400;
    private float sensY = 400;

    public Transform orientation;

    float xRotation;
    float yRotation;
    //public Transform  Spine;
    Transform playerBady, playerChestTr;
    Animator animator;
    public Transform cam;
    //플레이어의 애니매이션의 본을 움직여야 허리가 돌아간다.
    private void Start()
    {
        playerBady = transform.parent;
        animator = GetComponentInParent<Animator>();
        if (animator)
            playerChestTr = animator.GetBoneTransform(HumanBodyBones.UpperChest);

    }

    public void LateUpdate()
    {
        Operation_boneRitation();
    }

    Vector3 ChestOffset = new Vector3(0, -40, -100);
    Vector3 ChestDir = new Vector3();

    private void Operation_boneRitation()
    {
        //카메라가 보고있는 방향
        //ChestDir를 총이 바라보는 것으로 바꿔준다면 상체는 총이 바라보는 곳을 바라봄
        ChestDir = cam.position + cam.forward * 50f;
        playerChestTr.LookAt(ChestDir); //상체를 카메라 보는방향으로 보기
       
    }


    void Update()
    {
        //Debug.Log(transform.parent.name);
        //마우스 좌표를 받아오기
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        //마우스의 움직인 좌표값은 X좌표값은 Y축의 회전에 더해주고 Y좌표 값은 X축의 회전에 빼준다.
        //Debug.Log(aim.GetAimX() + "  " + aim.GetAimY());
        yRotation += (mouseX /*+ aim.GetAimX()*/);

        xRotation -= (mouseY /*+ aim.GetAimY()*/);
        //x축화전에 90도가 넘어가면 뒤집히지 않도록 고정해준다.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //마우스가 회전한 만큼PlayerRotate를 회전시킴
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        //플레이어의 몸의 회전을 y축으로만 회전함
        playerBady.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
