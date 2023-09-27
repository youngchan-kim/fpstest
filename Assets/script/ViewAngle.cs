using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewAngle : MonoBehaviour
{
    Vector3 TargetDir = new Vector3();
    Vector3 TargetDistanst = new Vector3();
    public float viewAngle;
    float dot;
    public Transform target;
    public Transform point;
    //앵글의 길이
    float angleRange = 30f;
    //인식 거리
    float len = 5f;
    // Update is called once per frame

    //바라보는 주체가 누구인지
    public void GetSeeUnit(int viewAngle)
    {
        //플레이어의 시야 값
        //Camera.main.transform.forward = Player.transform.forward;
        //viewAngle = 180 - Camera.main.fieldOfView;


        //플레이어로 향하는 target의 벡터
        TargetDistanst = point.position - transform.position;
        TargetDir = (TargetDistanst).normalized;



        //타겟과의 거리가 일정 거리보다 작은경우 실행
        if(TargetDistanst.magnitude <= len)
        {
            //기준이 되는 
            dot = Vector3.Angle(point.forward, TargetDir);
            if (dot > viewAngle)
            {

                target = point;
                Debug.Log(gameObject.name + "의 시야에 있습니다.");
            }
            else
            {
                target = null;
                Debug.Log(gameObject.name + "의 시야에 없습니다.");
            }
        }
        
    }

}


