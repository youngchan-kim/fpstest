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


    // Update is called once per frame
    void Update()
    {
        //마우스 좌표를 받아오기
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        //마우스의 움직인 좌표값은 X좌표값은 Y축의 회전에 더해주고 Y좌표 값은 X축의 회전에 빼준다.
        //Debug.Log(aim.GetAimX() + "  " + aim.GetAimY());
        yRotation += (mouseX /*+ aim.GetAimX()*/);

        xRotation -= (mouseY /*+ aim.GetAimY()*/);
        //x축화전에 90도가 넘어가면 뒤집히지 않도록 고정해준다.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        //카메라의 회전과 오브젝트의 회전방향을 일치시키기 위한 단계
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        //Head.rotation = Quaternion.Euler(xRotation, 0, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        //카메라의 Y축 회전과 오브젝트의 y축 회전 방향을 
    }
}
