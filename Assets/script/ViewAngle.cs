using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ViewAngle : MonoBehaviour
{
    Vector3 TargetDir = new Vector3();
    Vector3 TargetDistanst = new Vector3();
    float angle;
    public Transform target;
    public Transform point;

    //인식 거리

    float base_rotation_y;
    //바라보는 주체가 누구인지

    //부채꼴모양 색칠
    bool isCollision = false;
    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);
    float angleRange;
    float radius;
    float This_Angle;
    // 탐색 장소
    public Vector3 Spot = Vector3.zero;
    Transform search_spot;
    //탐색을 위한 y축 회전값
    float y_rotation;
    //더할지 말지 관리
    bool y_Dir;
    bool isDelay = true;
    public void GetSeeUnit(float viewAngle , float len)
    {
        //플레이어의 시야 값
        //Camera.main.transform.forward = Player.transform.forward;
        //viewAngle = 180 - Camera.main.fieldOfView;


        //탐색자를 향하는 타겟의 벡터
        //A가 타겟이고 B가 탐색자일때 B가 A로 가기 위한 방향은
        //A-B이다.
        //타겟에게 향하는 방향
        if (target == null)
            TargetDir = (point.position - transform.position).normalized;
        else
            TargetDir = (target.position - transform.position).normalized;
        angleRange = viewAngle;
        radius = len;
        //첫번째 벡터에서 두번째 벡터로의 회전 각도입니다.
        //첫번째 벡터로 두번째 벡터가 회전하기 위한 각도를 리턴한다.
        //position값이 아니라 방향값이 들어와야한다.
        //탐색자로 부터의 포인트가 있는 각도를 구해준다. 
        //함수화 할것
        angle = Vector3.Angle(TargetDir, transform.forward );

        //타겟과의 거리가 일정 거리보다 작은경우 실행
        if (TargetDistanst.magnitude <= len)
        {
            //angle은 탐색자를 기준으로한 각도값 
            //viewAngle은 가변하지 않음
            //탐색자를 기준으로
            if (angle < viewAngle)
            {
                target = point;
                Spot = target.position;
                //search_spot = target;
                Debug.Log(gameObject.name + "의 시야에 있습니다.");
                StopCoroutine("SearchTime");
                isCollision = true;
            }
            else
            {
                target = null;
                Debug.Log(gameObject.name + "의 시야에 없습니다.");
                isCollision = false;
            }
        }
        else
        {
            target = null;
            Debug.Log(gameObject.name + "의 시야에 없습니다.");
        }
        isCollision = false;
    }

    public void TargetFind(Coroutine coroutine)
    {

       // base_rotation_y = transform.rotation.y;
        if (isDelay)
        { 
            coroutine = StartCoroutine(SearchTime());
            isDelay = false;
        }

       
    }

    public IEnumerator SearchTime()
    {
        while (true) 
        {
            yield return new WaitForSeconds(3f);
            switch (y_Dir)
            {
                case true:
                    if (y_rotation > -40)
                        y_rotation -= 10;
                    else
                        y_Dir = false;
                    break;
                case false:
                    if (y_rotation < 40)
                        y_rotation += 10;
                    else
                        y_Dir = true;
                    break;
            }
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + y_rotation, transform.rotation.z);
            isDelay=true;
        }

    }

    private void OnDrawGizmos()
    {
        Handles.color = isCollision ? _red : _blue;
        // DrawSolidArc(시작점, 노멀벡터(법선벡터), 그려줄 방향 벡터, 각도, 반지름)
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, radius);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, radius);
    }
}


