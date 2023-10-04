using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ViewAngle : MonoBehaviour
{
    Vector3 TargetDir = new Vector3();
    Vector3 TargetDistanst = new Vector3();
    float dot;
    public Transform target;
    public Transform point;

    //인식 거리

    float base_rotation_y;
    public Transform search_spot;
    //바라보는 주체가 누구인지

    //부채꼴모양 색칠
    bool isCollision = false;
    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);
    float angleRange;
    float radius;
    float This_Angle;
    public void GetSeeUnit(float viewAngle , float len)
    {
        //플레이어의 시야 값
        //Camera.main.transform.forward = Player.transform.forward;
        //viewAngle = 180 - Camera.main.fieldOfView;


        //탐색자를 향하는 타겟의 벡터
        //A가 타겟이고 B가 탐색자일때 B가 A로 가기 위한 방향은
        //A-B이다.
        TargetDistanst = point.position - transform.position;
        //타겟에게 향하는 방향
        TargetDir = (TargetDistanst).normalized;
        angleRange = viewAngle;
        radius = len;
        This_Angle = viewAngle + transform.rotation.eulerAngles.y;
        dot = Vector3.Angle(point.forward, TargetDir);
        Debug.Log(dot + "의 크기.");
        //타겟과의 거리가 일정 거리보다 작은경우 실행
/*        if (TargetDistanst.magnitude <= len)
        {
            *//*//기준이 되는 
            dot = Vector3.Angle(point.forward, TargetDir);
            Debug.Log(dot + "의 크기.");*//*

            if (dot < This_Angle)
            {
                search_spot = null;
                target = point;
                //Debug.Log(gameObject.name + "의 시야에 있습니다.");
                StopCoroutine("SearchTime");
                isCollision = true;
            }
            else
            {
                search_spot = target;
                target = null;
                //Debug.Log(gameObject.name + "의 시야에 없습니다.");
                isCollision = false;
            }
        }*/
        if (dot < This_Angle)
        {
            Debug.Log(gameObject.name + "의 시야에 있습니다.");
        }
        else
        {
            Debug.Log(gameObject.name + "의 시야에 없습니다.");
        }
        isCollision = false;
    }

    public void TargetFind()
    {

        base_rotation_y = transform.rotation.y;
        StartCoroutine("SearchTime");
        transform.rotation = Quaternion.Euler(transform.rotation.x, base_rotation_y, transform.rotation.z); 
    }

    IEnumerator SearchTime()
    {
        yield return new WaitForSeconds(3f);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + (float)Random.Range(-40, 40), transform.rotation.z);
    }

    private void OnDrawGizmos()
    {
        Handles.color = isCollision ? _red : _blue;
        // DrawSolidArc(시작점, 노멀벡터(법선벡터), 그려줄 방향 벡터, 각도, 반지름)
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, radius);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, radius);
    }
}


