using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//현재 적용할 필요가 없는 기능
public class Billboard : MonoBehaviour
{
    
    public Camera cam;
    //카메라를 기준으로 카메라가 움직인 뒤에 카메라가 바라보는 방향으로 item의 오브젝트가 움직이게 만듬
    private void LateUpdate()
    {
        //transform.forward = cam.transform.forward;
    }
}
