using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    private float sensX = 400;
    private float sensY = 400;

    Transform playerBady;
    public AimShaker aim;
    float xRotation;
    float yRotation;
    private void Start()
    {
        //humanoid Player
        playerBady = transform.parent;
        //Debug.Log(playerBady.parent.GetChild(1).name);
        aim = playerBady.parent.GetChild(1).GetComponent<AimShaker>();
    }

    void Update()
    {
        //Debug.Log(transform.parent.name);
        //마우스 좌표를 받아오기
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        //마우스의 움직인 좌표값은 X좌표값은 Y축의 회전에 더해주고 Y좌표 값은 X축의 회전에 빼준다.
        //Debug.Log(aim.GetAimX() + "  " + aim.GetAimY());
        yRotation += mouseX;
        yRotation += aim.GetAimX();
        xRotation -= mouseY;
        xRotation -= aim.GetAimY();
        //x축화전에 90도가 넘어가면 뒤집히지 않도록 고정해준다.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //마우스가 회전한 만큼PlayerRotate를 회전시킴
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //플레이어의 몸의 회전을 y축으로만 회전함
        playerBady.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
